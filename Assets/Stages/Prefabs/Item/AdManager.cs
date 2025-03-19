using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] Button _showAdButton;
    [SerializeField] Button _cancelAdButton;
    public End endScript;
    [SerializeField] GameObject _adPanel;
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    string _adUnitId = null; // 지원되지 않는 플랫폼의 경우 Null로 유지됩니다
    private string adWatchedKey = "AdWatched"; // PlayerPrefs 키

    void Awake()
    {
        // 현재 플랫폼의 광고 단위 ID를 가져옵니다:
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif

        // 광고가 표시될 준비가 될 때까지 버튼을 비활성화합니다:
        _showAdButton.interactable = false;
        _adPanel.SetActive(false); // 시작 시 광고 패널을 비활성화합니다.

        // _cancelAdButton에 클릭 리스너 추가
        _cancelAdButton.onClick.AddListener(() => _adPanel.SetActive(false));
    }

    // 광고를 보여줄 준비를 하고 싶을 때 이 공개 방법을 호출합니다.
    public void LoadAd()
    {
        // 중요! 초기화 후에만 내용을 로드합니다(이 예에서는 초기화를 다른 스크립트에서 처리합니다).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    // 광고가 성공적으로 로드되면 버튼에 청취자를 추가하고 활성화합니다:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);

        if (adUnitId.Equals(_adUnitId))
        {
            // 사용자가 다음을 클릭할 수 있도록 버튼을 활성화합니다:
            _showAdButton.interactable = true;
        }
    }

    // 사용자가 버튼을 클릭할 때 실행할 방법을 구현합니다:
    public void ShowAd()
    {
        // 버튼 비활성화:
        _showAdButton.interactable = false;
        // 광고 패널 비활성화:
        endScript.ResumeGame(_adPanel);
        // 그런 다음 광고를 보여줍니다:
        Advertisement.Show(_adUnitId, this);
    }

    // Show Listener's OnUnityAdsShowComplete 콜백 메소드를 구현하여 사용자가 보상을 받는지 확인합니다:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // 보상을 해주세요.
            SavePlayerPosition();
        }
    }

    private void SavePlayerPosition()
    {
        Vector3 savePosition = transform.position;
        PlayerPrefs.SetFloat("RespawnX", savePosition.x);
        PlayerPrefs.SetFloat("RespawnY", savePosition.y);
        PlayerPrefs.SetFloat("RespawnZ", savePosition.z);
        PlayerPrefs.SetInt(adWatchedKey, 1); // 보상형 광고를 시청했음을 저장
        PlayerPrefs.Save();
    }

    // Load and Show Listener 오류 콜백 구현:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // 오류 세부 정보를 사용하여 다른 광고를 로드할지 여부를 결정합니다.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // 오류 세부 정보를 사용하여 다른 광고를 로드할지 여부를 결정합니다.
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }

    void OnDestroy()
    {
        // 버튼 청취자 정리:
        _showAdButton.onClick.RemoveAllListeners();
        _cancelAdButton.onClick.RemoveAllListeners();
    }

    // 플레이어가 깃발과 닿았을 때 호출할 메서드   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && PlayerPrefs.GetInt(adWatchedKey, 0) == 0)
        {
            endScript.PauseGame(_adPanel);
            LoadAd(); // 광고를 로드합니다.
        }
    }
}
