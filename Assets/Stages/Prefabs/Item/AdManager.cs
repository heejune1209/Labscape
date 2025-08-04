using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] Button _showAdButton;
    [SerializeField] Button _cancelAdButton;
    //public End endScript;
    [SerializeField] GameObject _adPanel;
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    string _adUnitId = null; // �������� �ʴ� �÷����� ��� Null�� �����˴ϴ�
    private string adWatchedKey = "AdWatched"; // PlayerPrefs Ű

    void Awake()
    {
        // ���� �÷����� ���� ���� ID�� �����ɴϴ�:
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif

        // ����� ǥ�õ� �غ� �� ������ ��ư�� ��Ȱ��ȭ�մϴ�:
        _showAdButton.interactable = false;
        _adPanel.SetActive(false); // ���� �� ���� �г��� ��Ȱ��ȭ�մϴ�.

        // _cancelAdButton�� Ŭ�� ������ �߰�
        _cancelAdButton.onClick.AddListener(() => _adPanel.SetActive(false));
    }

    // ����� ������ �غ� �ϰ� ���� �� �� ���� ����� ȣ���մϴ�.
    public void LoadAd()
    {
        // �߿�! �ʱ�ȭ �Ŀ��� ������ �ε��մϴ�(�� �������� �ʱ�ȭ�� �ٸ� ��ũ��Ʈ���� ó���մϴ�).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    // ����� ���������� �ε�Ǹ� ��ư�� û���ڸ� �߰��ϰ� Ȱ��ȭ�մϴ�:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);

        if (adUnitId.Equals(_adUnitId))
        {
            // ����ڰ� ������ Ŭ���� �� �ֵ��� ��ư�� Ȱ��ȭ�մϴ�:
            _showAdButton.interactable = true;
        }
    }

    // ����ڰ� ��ư�� Ŭ���� �� ������ ����� �����մϴ�:
    public void ShowAd()
    {
        // ��ư ��Ȱ��ȭ:
        _showAdButton.interactable = false;
        // ���� �г� ��Ȱ��ȭ:
        //endScript.ResumeGame(_adPanel);
        // �׷� ���� ����� �����ݴϴ�:
        Advertisement.Show(_adUnitId, this);
    }

    // Show Listener's OnUnityAdsShowComplete �ݹ� �޼ҵ带 �����Ͽ� ����ڰ� ������ �޴��� Ȯ���մϴ�:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // ������ ���ּ���.
            SavePlayerPosition();
        }
    }

    private void SavePlayerPosition()
    {
        Vector3 savePosition = transform.position;
        PlayerPrefs.SetFloat("RespawnX", savePosition.x);
        PlayerPrefs.SetFloat("RespawnY", savePosition.y);
        PlayerPrefs.SetFloat("RespawnZ", savePosition.z);
        PlayerPrefs.SetInt(adWatchedKey, 1); // ������ ����� ��û������ ����
        PlayerPrefs.Save();
    }

    // Load and Show Listener ���� �ݹ� ����:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // ���� ���� ������ ����Ͽ� �ٸ� ����� �ε����� ���θ� �����մϴ�.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // ���� ���� ������ ����Ͽ� �ٸ� ����� �ε����� ���θ� �����մϴ�.
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }

    void OnDestroy()
    {
        // ��ư û���� ����:
        _showAdButton.onClick.RemoveAllListeners();
        _cancelAdButton.onClick.RemoveAllListeners();
    }

    // �÷��̾ ��߰� ����� �� ȣ���� �޼���   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && PlayerPrefs.GetInt(adWatchedKey, 0) == 0)
        {
            //endScript.PauseGame(_adPanel);
            LoadAd(); // ����� �ε��մϴ�.
        }
    }
}
