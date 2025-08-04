using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Labscape.Data;

namespace Labscape.Manager
{
    public class GameManager : MonoBehaviour
{
    // 싱글톤을 통해 다른 매니저들에 접근

    // 모든 외부 참조는 DI를 통해 접근.

    // 타이머 일시정지 플래그
    public bool IsTimerPaused { get; set; }

    // 타이머를 계산하고, 플레이어 사망 카운트를 기록만 한다.
    // 경과 시간 업데이트 이벤트 (UI_Stage에서 구독)
    public event Action<TimeSpan> OnTimerUpdated;

    // 사망 횟수 변경 이벤트 (UI_Stage에서 구독)
    public event Action<int> OnDeathCountChanged;

    private int _playerDeathCount;

    public int PlayerDeathCount
    {
        get { return _playerDeathCount; }
        set
        {
            _playerDeathCount = value;
            OnDeathCountChanged?.Invoke(_playerDeathCount);
        }
    }

    // 이 플래그가 true면 아직 초기화(클리어 재도전용 리셋)를 하지 않은 상태
    private bool _needsStageReset;

    // 내부: 누적된 경과 시간(초)
    private double _elapsedSeconds;

    // 마지막으로 깃발을 꽂은 위치
    public Vector3 FlagPosition { get; private set; }
    // 깃발 위치가 저장되어 있는지 여부
    public bool HasFlagPosition { get; private set; }

    // 외부에서 현재 누적된 경과 시간을 가져올 수 있도록 추가
    public TimeSpan ElapsedTime()
    {
        return TimeSpan.FromSeconds(_elapsedSeconds);
    }

    private void HandleDataLoaded(SaveData data)
    {
        try
        {
            // null 체크 추가 (GameManager 파괴 후 호출 방지)
            if (data == null || Managers.Data == null)
            {
                Debug.LogWarning("[GameManager] HandleDataLoaded - 데이터 또는 매니저가 null입니다.");
                return;
            }

            var scene = SceneManager.GetActiveScene();
            if (scene.name.StartsWith("Stage") && TryGetStageIndex(out var idx))
            {
                // 이미 클리어된 스테이지 재도전용으로만 초기화 플래그 켜기
                _needsStageReset = (idx < data.stageClears.Count && data.stageClears[idx]);
                OnSceneLoaded(scene, LoadSceneMode.Single);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[GameManager] HandleDataLoaded에서 예외 발생: {e.Message}\n{e.StackTrace}");
        }
    }

    /// <summary>
    /// GameManager 초기화
    /// </summary>
    public void Init()
    {
        Debug.Log("GameManager: Initialize");
        
        // 씬 이벤트 구독
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        // 데이터 로드 완료 이벤트 구독
        Managers.Data.OnLoaded += HandleDataLoaded;
    }




    // 플레이어 사망 시 호출.   
    public void OnPlayerDead()
    {
        // null 체크 추가
        if (Managers.Data == null)
        {
            Debug.LogWarning("[GameManager] DataManager가 null입니다. OnPlayerDead 처리를 건너뜁니다.");
            return;
        }

        // (1) 사망 카운트 증가 → 데이터에 반영
        PlayerDeathCount++;

        // (2) 현재 진행 중인 세션 사망 횟수 업데이트
        if (TryGetStageIndex(out int idx))
        {
            var data = Managers.Data.Current;
            if (data != null)
            {
            var deaths = data.currentDeathCounts;
            // 리스트 크기 보장
            while (deaths.Count <= idx) deaths.Add(0);
            deaths[idx] = PlayerDeathCount;
            Managers.Data.SaveDataOnChange(); // 로컬 저장


            }
        }

        //// (3) 업적 시스템에 플레이어 사망 알림
        //try
        //{
        //AchievementIntegration.OnPlayerDeath();
        //}
        //catch (Exception e)
        //{
        //    Debug.LogError($"[GameManager] 업적 시스템 사망 알림 실패: {e.Message}");
        //}

        // (4) 리스폰 연출 후 씬 재로드
        StartCoroutine(RespawnAfterDelay(3f));
    }

    // 스테이지 클리어 후 재도전 준비 플래그 설정
    public void OnStageCleared()
    {
        _needsStageReset = true;
        Debug.Log("[GameManager] 스테이지 클리어 완료 - 재도전 준비 플래그 설정");
    }

    // 플레이어 사망 후 delay초만큼 연출 대기한 다음 씬 재로드
    private IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // null 체크 추가
        if (Managers.Scene == null)
        {
            Debug.LogWarning("[GameManager] SceneManager가 null입니다. 기본 씬 로드를 시도합니다.");
            string sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneName);
            yield break;
        }

        // 현재 로드된 씬 이름을 가져와서
        string sceneName2 = SceneManager.GetActiveScene().name;

        // Define.Scene enum으로 변환 시도
        if (Enum.TryParse<Define.Scene>(sceneName2, out var sceneType))
        {
            // SceneManagerEx의 LoadScene 호출
            Managers.Scene.LoadScene(sceneType);
        }
        else
        {
            Debug.LogError($"[GameManager] 씬 이름 '{sceneName2}'을 Define.Scene으로 변환할 수 없습니다.");
            // 실패 시엔 기존 방식으로라도 한번 로드
            SceneManager.LoadScene(sceneName2);
        }
    }

    /// <summary>
    /// 깃발 사용 시 현재 위치 저장
    /// </summary>
    public void SaveFlagPosition(Vector3 pos)
    {
        // null 체크 추가
        if (Managers.Data == null)
        {
            Debug.LogWarning("[GameManager] DataManager가 null입니다. SaveFlagPosition 처리를 건너뜁니다.");
            return;
        }

        if (!TryGetStageIndex(out int idx)) return;

        var data = Managers.Data.Current;
        if (data != null)
        {
        var flags = data.stageFlagPositions;
            // 리스트 크기 보장 (null 대신 기본값 사용)
            while (flags.Count <= idx) flags.Add(new SerializableVector3());
            flags[idx] = new SerializableVector3(pos.x, pos.y, pos.z);

        Managers.Data.SaveDataOnChange();


        }

        FlagPosition = pos;
        HasFlagPosition = true;
    }

    /// <summary>
    /// 씬 로드 시 처리: 클리어 여부에 따라 초기화 또는 진행 복원
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        try
        {
            Debug.Log($"[GameManager] OnSceneLoaded 호출 - Scene: {scene.name}, Mode: {mode}");
            
            // null 체크 추가 (GameManager 파괴 후 호출 방지)
            if (Managers.Data == null)
            {
                Debug.LogWarning("[GameManager] DataManager가 null입니다. 이벤트 처리를 건너뜁니다.");
                return;
            }

        // Stage 씬이 아니라면 타이머만 일시정지
        if (!scene.name.StartsWith("Stage") || !TryGetStageIndex(out int idx))
        {
            Debug.Log($"[GameManager] 스테이지 씬이 아님 - Scene: {scene.name}");
            IsTimerPaused = true;
            return;
        }
     
        var stageScene = FindObjectOfType<StageScene>();
        var sd = Managers.Data.Current;
        
        // SaveData null 체크 추가
        if (sd == null)
        {
            Debug.LogWarning("[GameManager] SaveData가 null입니다. 기본 설정으로 진행합니다.");
            IsTimerPaused = false;
            return;
        }

        // ── 깃발 위치 복원 & 유효성 검사 ──
        if (idx < sd.stageFlagPositions.Count)
        {
            Vector3 saved = sd.stageFlagPositions[idx].ToVector3();
            bool valid = stageScene != null
                ? stageScene.IsValidFlagPos(saved)
                : (saved != Vector3.zero);

            if (valid)
            {
                FlagPosition = saved;
                HasFlagPosition = true;
                Debug.Log($"[GameManager] 유효한 깃발 위치 복원: {saved}");
            }
            else if (stageScene != null)
            {
                // 무효 시 기본 위치로 대체
                FlagPosition = stageScene.GetDefaultSpawnPos();
                HasFlagPosition = true;
                Debug.Log($"[GameManager] 무효한 깃발 위치, 기본 위치로 대체: {FlagPosition}");
            }

            // 플레이어 리스폰
            var player = GameObject.FindWithTag("Player");
            if (player != null)
                player.transform.position = FlagPosition;
        }
        else
        {
            Debug.Log($"[GameManager] 깃발 위치 없음 - stageFlagPositions.Count: {sd.stageFlagPositions.Count}, idx: {idx}");
        }

        // 한 번만 초기화: 클리어된 스테이지 재도전
        Debug.Log($"[GameManager] 스테이지 {idx + 1} 처리 - needsStageReset: {_needsStageReset}, " +
                 $"isCleared: {(idx < sd.stageClears.Count && sd.stageClears[idx])}");
        
        if (_needsStageReset
            && idx < sd.stageClears.Count && sd.stageClears[idx])
        {
            // 재도전 시 모든 관련 데이터 초기화
            _elapsedSeconds = 0;
            PlayerDeathCount = 0;
            HasFlagPosition = false;

            // current 값들도 함께 초기화 (서버 동기화를 위해)
            while (sd.currentPlayTimes.Count <= idx) sd.currentPlayTimes.Add(0f);
            sd.currentPlayTimes[idx] = 0f;
            
            while (sd.currentDeathCounts.Count <= idx) sd.currentDeathCounts.Add(0);
            sd.currentDeathCounts[idx] = 0;
            
            // 깃발 위치도 초기화 (재도전 시 깨끗한 상태로 시작)
            while (sd.stageFlagPositions.Count <= idx) sd.stageFlagPositions.Add(new SerializableVector3());
            sd.stageFlagPositions[idx] = new SerializableVector3(0f, 0f, 0f);
            
            // 즉시 저장 및 델타 생성
            Managers.Data.SaveDataOnChange();

            // 다음부터는 초기화하지 않도록
            _needsStageReset = false;
            
            Debug.Log($"[GameManager] 스테이지 {idx + 1} 재도전 - 모든 기록 초기화 완료 (current 값들 및 깃발 위치 초기화)");
        }
        else
        {
            // 첫 진입(처음 도전) 혹은 리스폰(죽음 후 재도전) 시,
            // 저장된 시간·데스카운트 그대로 복원
            _elapsedSeconds = idx < sd.currentPlayTimes.Count
                ? sd.currentPlayTimes[idx] : 0f;
            PlayerDeathCount = idx < sd.currentDeathCounts.Count
                ? sd.currentDeathCounts[idx] : 0;
                
            Debug.Log($"[GameManager] 스테이지 {idx + 1} 기록 복원 - " +
                     $"Time: {_elapsedSeconds:F2}s, Deaths: {PlayerDeathCount}");
        }

        // 타이머·리스폰 구독·UI 갱신…
        OnTimerUpdated?.Invoke(TimeSpan.FromSeconds(_elapsedSeconds));
        SubscribePlayerDeath();
        IsTimerPaused = false;
        
        // 업적 시스템에 스테이지 시작 알림 (예외 처리)
        try
        {
        //AchievementIntegration.OnStageStart();
        }
        catch (Exception e)
        {
            //Debug.LogError($"[GameManager] 업적 시스템 스테이지 시작 알림 실패: {e.Message}\n{e.StackTrace}");
        }
        
            Debug.Log($"[GameManager] OnSceneLoaded 완료 - Scene: {scene.name}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[GameManager] OnSceneLoaded에서 예외 발생 - Scene: {scene.name}, Error: {e.Message}\n{e.StackTrace}");
            
            // 예외 발생 시에도 기본 설정은 유지
            IsTimerPaused = false;
        }
    }


    /// <summary>
    /// 씬 언로드 시 현재 플레이 기록 저장
    /// </summary>
    private void OnSceneUnloaded(Scene scene)
    {
        // null 체크 추가
        if (Managers.Data == null)
        {
            Debug.LogWarning("[GameManager] DataManager가 null입니다. OnSceneUnloaded 처리를 건너뜁니다.");
            return;
        }

        if (!scene.name.StartsWith("Stage") || !TryGetStageIndex(out int idx))
            return;

        var data = Managers.Data.Current;
        if (data != null)
        {
        var plays = data.currentPlayTimes;
        // 리스트 크기 보장
        while (plays.Count <= idx) plays.Add(0f);
        plays[idx] = (float)_elapsedSeconds;
        Managers.Data.SaveDataOnChange();

        // 유효한 플레이 시간만 델타 전송 (5초 이상, 실제 게임플레이)
        if (_elapsedSeconds >= 5.0f && _elapsedSeconds < float.MaxValue)
        {
            // Removed: Managers.Data.GenerateDelta($"currentPlayTimes_{idx + 1}", plays[idx]);
            Debug.Log($"[GameManager] 플레이 시간 델타 전송: Stage {idx + 1}, Time: {_elapsedSeconds:F2}s");
        }
        else
        {
            Debug.Log($"[GameManager] 무효한 플레이 시간으로 델타 전송 건너뜀: Stage {idx + 1}, Time: {_elapsedSeconds:F2}s");
        }
        }
    }

    // Health.OnDead 이벤트에 GameManager.OnPlayerDead 연결
    void SubscribePlayerDeath()
    {
        var player = GameObject.FindWithTag("Player");
        if (player == null) return;

        //var health = player.GetComponent<Health>();
        //if (health == null) return;

        //health.OnDead -= OnPlayerDead;
        //health.OnDead += OnPlayerDead;
    }

    // 현재 씬이 "StageN" 형태인지 확인하고 인덱스 반환
    bool TryGetStageIndex(out int idx)
    {
        var name = SceneManager.GetActiveScene().name;
        if (name.StartsWith("Stage") && int.TryParse(name.Substring(5), out var n))
        {
            const int MAX_GAME_STAGES = 10; // 실제 게임에 존재하는 스테이지 수
            
            // 유효한 스테이지 번호 범위 확인 (1~10)
            if (n >= 1 && n <= MAX_GAME_STAGES)
        {
            idx = n - 1;
            return true;
            }
            else
            {
                Debug.LogWarning($"[GameManager] 유효하지 않은 스테이지 번호: {n} (최대: {MAX_GAME_STAGES})");
            }
        }

        idx = -1;
        return false;
    }
    
    /// <summary>
    /// 매프레임 업데이트 (Managers에서 호출)
    /// </summary>
    public void OnUpdate()
    {
        // 타이머 업데이트 (일시정지가 아닐 때만)
        if (!IsTimerPaused)
        {
            _elapsedSeconds += Time.deltaTime;
            TimeSpan elapsed = TimeSpan.FromSeconds(_elapsedSeconds);
            OnTimerUpdated?.Invoke(elapsed);
        }
    }
    
    /// <summary>
    /// MonoBehaviour OnDestroy
    /// </summary>
    void OnDestroy()
    {
        Clear();
    }
    
    /// <summary>
    /// GameManager 정리 (Managers에서 호출 또는 OnDestroy에서 호출)
    /// </summary>
    public void Clear()
    {
        Debug.Log("GameManager: Clear");
        
        // 코루틴 정리
        StopAllCoroutines();
        
        // Scene 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        
        // DataManager 이벤트 해제
        if (Managers.Data != null)
        {
            Managers.Data.OnLoaded -= HandleDataLoaded;
        }
        
        // 외부 구독자들이 남아있을 수 있으니 이벤트 초기화
        OnTimerUpdated = null;
        OnDeathCountChanged = null;
        
        Debug.Log("[GameManager] Clear 완료 - 모든 이벤트 해제됨");
    }
}}
