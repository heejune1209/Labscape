using Labscape.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// 인게임 HUD: 현재/최고/데스 카운트, 타이머 표시
public class UI_Stage : UI_Scene
{

    // 화면에 띄울 텍스트 필드들
    enum Texts
    {
        DeathCountText,  // 플레이어 데스 카운트
        TimerText,       // 경과 시간
        BestDeathText,   // ← 최고(최소) 데스 카운트
        BestTimeText     // ← 최고(최단) 타임
    }

    private TMP_Text _deathCountText;
    private TMP_Text _timerText;
    private TMP_Text _bestDeathText;
    private TMP_Text _bestTimeText;


    private void Start()
    {
        Init();

        // 혹시몰라서 초기화 
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void Init()
    {
        base.Init();

        // 1) 바인딩
        Bind<TextMeshProUGUI>(typeof(Texts));

        _deathCountText = GetText((int)Texts.DeathCountText);
        _timerText = GetText((int)Texts.TimerText);
        _bestDeathText = GetText((int)Texts.BestDeathText);
        _bestTimeText = GetText((int)Texts.BestTimeText);

        // 2) 고정 UI (인벤토리)
        Managers.UI.ShowSceneUI<UI_Inventory>("UI_Inventory");

        // 3) GameManager 이벤트 구독
        Managers.Game.OnDeathCountChanged += UpdateDeathCount;
        Managers.Game.OnTimerUpdated += UpdateTimerText;

        // 4) StageManager 이벤트 구독 (Best 기록)
        Managers.Stage.OnBestDeathUpdated += OnBestDeathUpdated;
        Managers.Stage.OnBestTimeUpdated += OnBestTimeUpdated;

        // **초기값 한 번 뿌려주기**
        UpdateDeathCount(Managers.Game.PlayerDeathCount);
        UpdateTimerText(TimeSpan.Zero);

        // Best 기록은 이벤트에서 초기 DispatchAll 시 자동 업데이트됩니다.
        // 최초 한 번만, 로드된 저장 데이터로 Best 텍스트 갱신
        int stage = GetCurrentStageNum();
        // 데이터가 없으면 내부에서 -- 처리하니 안전
        OnBestDeathUpdated(stage, Managers.Stage.GetBestDeath(stage));
        OnBestTimeUpdated(stage, Managers.Stage.GetBestTime(stage));

    }

    void Update()
    {
        var tab = Keyboard.current.tabKey;

        // Tab 누를 때만 정보창 띄우기 (Warning 창이 떠 있으면 무시)
        if (tab.wasPressedThisFrame
            && !Managers.UI.IsPopupOpen<UI_Information>()
            && !Managers.UI.IsPopupOpen<UI_Warning>())
        {
            Managers.UI.ShowPopupUI<UI_Information>("UI_Information");
        }

        // Tab 뗄 때, 맨 위 팝업이 정보창이면 닫아주기
        if (tab.wasReleasedThisFrame)
        {
            // GetTopPopup()이 UI_Information인 경우에만
            if (Managers.UI.GetTopPopup() is UI_Information)
            {
                Managers.UI.ClosePopupUI();
            }

        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame &&
            Managers.UI.GetTopPopup() == null)
        {
            Managers.UI.ShowPopupUI<UI_Warning>("UI_Warning_Stage");
        }
    }

    // 데스 카운트 업데이트 핸들러
    private void UpdateDeathCount(int count)
    {
        if (_deathCountText != null)
            _deathCountText.text = $"Death : {count}";
    }

    // 타이머 텍스트 업데이트 핸들러
    private void UpdateTimerText(TimeSpan elapsed)
    {
        if (_timerText != null)
            _timerText.text = $"Time : {elapsed.Minutes:00} : {elapsed.Seconds:00}";
    }

    protected override void OnDestroy()
    {
        // 이벤트 해제
        Managers.Game.OnDeathCountChanged -= UpdateDeathCount;
        Managers.Game.OnTimerUpdated -= UpdateTimerText;
        Managers.Stage.OnBestDeathUpdated -= OnBestDeathUpdated;
        Managers.Stage.OnBestTimeUpdated -= OnBestTimeUpdated;
    }

    // 최소 데스
    private void OnBestDeathUpdated(int stage, int death)
    {
        if (GetCurrentStageNum() != stage) return;
        _bestDeathText.text = death < int.MaxValue
            ? $"Best Deaths : {death}"
            : "Best Deaths : --";
    }

    // 최단 타임
    private void OnBestTimeUpdated(int stage, float seconds)
    {
        if (GetCurrentStageNum() != stage) return;
        if (seconds < float.MaxValue)
        {
            var ts = TimeSpan.FromSeconds(seconds);
            _bestTimeText.text = $"Best Time   : {ts.Minutes:00}:{ts.Seconds:00}";
        }
        else
        {
            _bestTimeText.text = "Best Time   : -- : --";
        }
    }
    private int GetCurrentStageNum()
    {
        string name = SceneManager.GetActiveScene().name;
        if (name.StartsWith("Stage")
            && int.TryParse(name.Substring(5), out int n))
            return n;
        return 0;
    }

    // GoalTrigger에서 호출: 결과 팝업
    public void ShowResult()
    {
        var popup = Managers.UI.ShowPopupUI<UI_Result>("UI_Result");
        popup.ShowResult(Managers.Game.ElapsedTime());
    }
}
