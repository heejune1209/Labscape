using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Labscape.Data;
using System.Text;  
using System.Collections.Generic;
using Labscape.Items;

namespace Labscape.Manager
{
    /// <summary>
    /// 간단하고 효율적인 JSON 기반 데이터 관리 시스템
    /// 암호화 및 특정 데이터 변경 저장 지원
    /// </summary>
    public class DataManager
{
    // 저장 파일 경로
    private string _filePath;
    
    // 현재 게임 데이터
    public SaveData Current { get; private set; }
    
    // 데이터 관련 이벤트들 (Just_Climb 스타일)
    public event Action<SaveData> OnLoaded;        // 데이터 로드 완료
    public event Action<SaveData> OnSaved;         // 데이터 저장 완료
    public event Action<string, object> OnDataChanged; // 특정 데이터 변경 시
    
    // 애플리케이션 생명주기 관리
    private bool _applicationLifecycleRegistered = false;
    
    // 암호화 키 (간단한 XOR 암호화용)
    private const string ENCRYPTION_KEY = "LabscapeGame2024";

    #region 초기화 및 생명주기

    /// <summary>
    /// DataManager 초기화
    /// </summary>
    public void Init()
    {
        Debug.Log("DataManager: Initialize");
        
        // 파일 경로 초기화
        _filePath = Path.Combine(Application.persistentDataPath, "save.dat");
        
        // 데이터 로드
        LoadData();
        
        // 애플리케이션 생명주기 이벤트 등록
        RegisterApplicationLifecycleEvents();
    }

    /// <summary>
    /// 애플리케이션 생명주기 이벤트 등록
    /// </summary>
    private void RegisterApplicationLifecycleEvents()
    {
        if (!_applicationLifecycleRegistered)
        {
            Application.quitting += OnApplicationQuit;
            Application.focusChanged += OnApplicationFocus;
            _applicationLifecycleRegistered = true;
            Debug.Log("DataManager: 애플리케이션 생명주기 이벤트 등록 완료");
        }
    }

    /// <summary>
    /// 애플리케이션 생명주기 이벤트 해제
    /// </summary>
    private void UnregisterApplicationLifecycleEvents()
    {
        if (_applicationLifecycleRegistered)
        {
            Application.quitting -= OnApplicationQuit;
            Application.focusChanged -= OnApplicationFocus;
            _applicationLifecycleRegistered = false;
            Debug.Log("DataManager: 애플리케이션 생명주기 이벤트 해제 완료");
        }
    }

    /// <summary>
    /// 애플리케이션 종료 시 자동 저장
    /// </summary>
    private void OnApplicationQuit()
    {
        Debug.Log("DataManager: 애플리케이션 종료 감지 - 데이터 저장 중...");
        SaveData();
    }

    /// <summary>
    /// 애플리케이션 포커스 변경 시 자동 저장
    /// </summary>
    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            Debug.Log("DataManager: 애플리케이션 포커스 잃음 - 데이터 저장 중...");
            SaveData();
        }
    }

    #endregion

    #region 데이터 로드/저장

    /// <summary>
    /// 암호화된 JSON 파일에서 데이터 로드
    /// </summary>
    public void LoadData()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                // 암호화된 데이터 읽기
                string encryptedData = File.ReadAllText(_filePath);
                
                // 복호화
                string json = DecryptData(encryptedData);
                
                // JSON 파싱
                Current = JsonConvert.DeserializeObject<SaveData>(json);
                Debug.Log("DataManager: 데이터 로드 성공");
            }
            else
            {
                // 첫 실행 시 기본 데이터 생성
                Current = new SaveData();
                Debug.Log("DataManager: 새로운 게임 데이터 생성");
            }
            
            // 데이터 유효성 검사 및 초기화
            ValidateAndInitializeData();
            
            // 로드 완료 이벤트 발생
            OnLoaded?.Invoke(Current);
        }
        catch (Exception e)
        {
            Debug.LogError($"DataManager: 데이터 로드 실패 - {e.Message}");
            // 로드 실패 시 기본 데이터 생성
            Current = new SaveData();
            ValidateAndInitializeData();
            OnLoaded?.Invoke(Current);
        }
    }

    /// <summary>
    /// 현재 데이터를 암호화된 JSON 파일로 저장
    /// </summary>
    public void SaveData()
    {
        try
        {
            // JSON 직렬화
            string json = JsonConvert.SerializeObject(Current, Formatting.None);
            
            // 암호화
            string encryptedData = EncryptData(json);
            
            // 파일에 저장
            File.WriteAllText(_filePath, encryptedData);
            
            Debug.Log("DataManager: 데이터 저장 성공");
            
            // 저장 완료 이벤트 발생
            OnSaved?.Invoke(Current);
        }
        catch (Exception e)
        {
            Debug.LogError($"DataManager: 데이터 저장 실패 - {e.Message}");
        }
    }

    /// <summary>
    /// 특정 데이터가 변경되었을 때 즉시 저장
    /// </summary>
    public void SaveDataOnChange()
    {
        SaveData();
    }

    /// <summary>
    /// 특정 데이터 변경 이벤트 발생 (다른 매니저들이 호출)
    /// </summary>
    public void NotifyDataChanged(string key, object value)
    {
        OnDataChanged?.Invoke(key, value);
        Debug.Log($"DataManager: 데이터 변경 알림 - {key}: {value}");
    }

    #endregion

    #region 데이터 암호화

    /// <summary>
    /// 간단한 XOR 암호화
    /// </summary>
    private string EncryptData(string data)
    {
        if (string.IsNullOrEmpty(data)) return data;
        
        char[] encrypted = new char[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            encrypted[i] = (char)(data[i] ^ ENCRYPTION_KEY[i % ENCRYPTION_KEY.Length]);
        }
        
        // Base64로 인코딩하여 안전한 문자열로 변환
        byte[] bytes = Encoding.UTF8.GetBytes(encrypted);
        return System.Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// XOR 복호화
    /// </summary>
    private string DecryptData(string encryptedData)
    {
        if (string.IsNullOrEmpty(encryptedData)) return encryptedData;
        
        try
        {
            // Base64 디코딩
            byte[] bytes = Convert.FromBase64String(encryptedData);
            char[] data = Encoding.UTF8.GetChars(bytes);
            
            // XOR 복호화
            char[] decrypted = new char[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                decrypted[i] = (char)(data[i] ^ ENCRYPTION_KEY[i % ENCRYPTION_KEY.Length]);
            }
            
            return new string(decrypted);
        }
        catch (Exception e)
        {
            Debug.LogError($"DataManager: 복호화 실패 - {e.Message}");
            return string.Empty;
        }
    }

    #endregion

    #region 데이터 유효성 검사

    /// <summary>
    /// 데이터 유효성 검사 및 초기화
    /// </summary>
    private void ValidateAndInitializeData()
    {
        if (Current == null)
        {
            Current = new SaveData();
        }

        // 기본값 설정
        if (string.IsNullOrEmpty(Current.selectedCharacter))
            Current.selectedCharacter = "Default";
        
        if (Current.items == null)
            Current.items = new List<InventoryItem>();
        
        if (Current.stageClears == null)
            Current.stageClears = new List<bool>();
        
        if (Current.stageFlagPositions == null)
            Current.stageFlagPositions = new List<SerializableVector3>();
            
        if (Current.bestCoreRewards == null)
            Current.bestCoreRewards = new List<int>();
            
        if (Current.bestClearTimes == null)
            Current.bestClearTimes = new List<float>();
            
        if (Current.bestDeathCounts == null)
            Current.bestDeathCounts = new List<int>();
            
        if (Current.currentPlayTimes == null)
            Current.currentPlayTimes = new List<float>();
            
        if (Current.currentDeathCounts == null)
            Current.currentDeathCounts = new List<int>();
        
        Debug.Log("DataManager: 데이터 유효성 검사 완료");
    }

    #endregion

    #region 정리

    /// <summary>
    /// DataManager 정리
    /// </summary>
    public void Clear()
    {
        Debug.Log("DataManager: Clear");
        
        // 애플리케이션 생명주기 이벤트 해제
        UnregisterApplicationLifecycleEvents();
        
        // 마지막 저장
        if (Current != null)
        {
            SaveData();
        }
        
        // 이벤트 정리
        OnLoaded = null;
        OnSaved = null;
        OnDataChanged = null;
        
        // 데이터 정리
        Current = null;
    }

    #endregion
}}
