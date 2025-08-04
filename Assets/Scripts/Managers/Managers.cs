using UnityEngine;
using UnityEngine.SceneManagement;
using Labscape.Manager;

/// <summary>
/// 게임 매니저로서 동작할 스크립트 - 모든 매니저를 중앙에서 관리하는 싱글톤
/// </summary>
public class Managers : MonoBehaviour
{
    #region Singleton Implementation
    static Managers s_Instance;
    static Managers Instance
    {
        get
        {
            Init();
            return s_Instance;
        }
    }
    #endregion

    #region Manager Instances
    // 각 매니저 인스턴스들
    private DataManager _data = new DataManager();
    private GameManager _game;  // MonoBehaviour이므로 나중에 컴포넌트로 추가
    private UIManager _ui = new UIManager();
    private ResourceManager _resource = new ResourceManager();
    private PoolManager _pool = new PoolManager();
    private SoundManager _sound = new SoundManager();
    private SceneManagerEX _scene = new SceneManagerEX();
    private CurrencyManager _currency = new CurrencyManager();
    private ItemManager _item = new ItemManager();
    //private AchievementManager _achievement = new AchievementManager();
    private StageManager _stage = new StageManager();
    #endregion

    #region Static Properties for Manager Access
    // 외부에서 접근할 수 있는 Static 프로퍼티들
    public static DataManager Data { get { return Instance._data; } }
    public static GameManager Game { get { return Instance._game; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static SceneManagerEX Scene { get { return Instance._scene; } }
    public static CurrencyManager Currency { get { return Instance._currency; } }
    public static ItemManager Item { get { return Instance._item; } }
    //public static AchievementManager Achievement { get { return Instance._achievement; } }
    public static StageManager Stage { get { return Instance._stage; } }
    #endregion

    #region Unity Lifecycle
    void Start()
    {
        Init();
    }

    void Update()
    {
        // 필요한 매니저들의 Update 호출
        _game?.OnUpdate();
    }
    #endregion

    #region Initialization
    static void Init()
    {
        if (s_Instance == null)
        {
            GameObject obj = GameObject.Find("@Managers");
            if (obj == null)
            {
                obj = new GameObject { name = "@Managers" };
                obj.AddComponent<Managers>();
            }

            // 씬 변경 시에도 삭제되지 않도록 설정
            DontDestroyOnLoad(obj);
            s_Instance = obj.GetComponent<Managers>();

            // 매니저들 순서대로 초기화
            s_Instance.InitializeManagers();
        }
    }

    private void InitializeManagers()
    {
        Debug.Log("Managers: Initializing all managers...");

        // GameManager는 MonoBehaviour이므로 컴포넌트로 추가
        _game = gameObject.AddComponent<GameManager>();

        // 기본 시스템 매니저들 먼저 초기화
        _resource?.Init();
        _pool?.Init();
        _sound?.Init();
        _scene?.Init();
        
        // 게임 로직 매니저들 초기화
        _data?.Init();
        _currency?.Init();
        _item?.Init();
        _ui?.Init();
        
        // 게임플레이 관련 매니저들 초기화
        //_achievement?.Init();
        _stage?.Init();
        
        // 게임 매니저는 마지막에 초기화 (다른 매니저들에 의존하므로)
        _game?.Init();

        Debug.Log("Managers: All managers initialized successfully!");
    }
    #endregion

    #region Cleanup
    public static void Clear()
    {
        Debug.Log("Managers: Clearing all managers...");
        
        // 매니저들 역순으로 정리
        Game?.Clear();
        Stage?.Clear();
        //Achievement?.Clear();
        Item?.Clear();
        Currency?.Clear();
        Data?.Clear();
        UI?.Clear();
        Sound?.Clear();
        Scene?.Clear();
        Pool?.Clear();
        Resource?.Clear();
    }

    void OnDestroy()
    {
        Clear();
    }
    #endregion
}