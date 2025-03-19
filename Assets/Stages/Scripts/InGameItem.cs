using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class InGameItem : MonoBehaviour
{
    public TMP_Text itemFeatherCount;
    public TMP_Text itemShoesCount;
    public TMP_Text itemDetectorCount;
    public TMP_Text itemHourGlassesCount;

    public GameObject featherPrefab; // 프리팹 예시
    public GameObject ShoesPrefab; // 다른 아이템 프리팹
    public GameObject Detector;
    public GameObject HourGlass;
    public GameObject ChangeSprite;
    private Dictionary<string, float> itemCooldowns = new Dictionary<string, float>();
    private Dictionary<string, float> itemCooldownDurations = new Dictionary<string, float>
    {
        { "Feather", 6f },
        { "Shoes", 6f },
        { "Detector", 6f },
        { "HourGlass", 2.5f }
    };
    public float itemDuration = 6f; // 아이템 지속시간
    public float HourGlassDuration = 2.5f; // 모래시계 지속시간
    //public CoolTime coolTime;

    public GameObject player;
    [HideInInspector]
    public Vector3 playerRespawnPosition; // 깃발을 사용한 위치 저장용 변수

    // Start is called before the first frame update
    void Start()
    {       
        UpdateItemCount();

    }

    // Update is called once per frame
    void Update()
    {
         /*   
        if ()
        {
            UseItem("Feather");
        }
        else if ()
        {
            UseItem("Wing");
        }
        else if ()
        {
            UseItem("Lamp");
        }
        else if ()
        {
            UseItem("Flag");
        }
         */
    }

    void UpdateItemCount()
    {
        itemFeatherCount.text = PlayerPrefs.GetInt("Feather", 0).ToString();
        itemShoesCount.text = PlayerPrefs.GetInt("Shoes", 0).ToString();
        itemDetectorCount.text = PlayerPrefs.GetInt("Detector", 0).ToString();
        itemHourGlassesCount.text = PlayerPrefs.GetInt("HourGlass", 0).ToString();
    }

    public void UseItem(string itemName) // 아이템 사용
    {
        if (itemCooldowns.ContainsKey(itemName))
        {
            float cooldownEndTime = itemCooldowns[itemName];
            if (Time.time < cooldownEndTime)
            {
                return;
            }
        }
        int itemCount = PlayerPrefs.GetInt(itemName, 0);

        if (itemCount > 0)
        {
            // 아이템 효과
            itemCount--;
            PlayerPrefs.SetInt(itemName, itemCount);
            PlayerPrefs.Save();

            UpdateItemCount();

            // 아이템 프리팹 생성
            if (itemName == "Feather")
            {
                featherPrefab.SetActive(true);
                player.GetComponent<PlayerController>().ActivateFeather();
                
                Invoke("Deactivatefeather", itemDuration);
            }
            else if (itemName == "Shoes")
            {
                ShoesPrefab.SetActive(true);
                player.GetComponent<PlayerController>().ActivateShoes();

                Invoke("Deactivateshoes", itemDuration);
            }
            else if (itemName == "Detector")
            {
                Detector.SetActive(true);
                ChangeSprite _changeSp = ChangeSprite.GetComponent<ChangeSprite>();
                _changeSp.ActivateDetector();

                Invoke("DeactivateDetector", itemDuration);
            }
            else if (itemName == "HourGlass")
            {
                HourGlass.SetActive(true);
                player.GetComponent<PlayerController>().ActiveHourGlass();
                Invoke("DeactivateHourglass", HourGlassDuration);
            }
            float cooldownDuration = itemCooldownDurations[itemName];
            float cooldownEndTime = Time.time + cooldownDuration;
            itemCooldowns[itemName] = cooldownEndTime;

            StartCoroutine(StartItemCooldown(itemName, cooldownDuration));

        }
    }
    
    private IEnumerator StartItemCooldown(string itemName, float cooldownDuration)
    {
        yield return new WaitForSeconds(cooldownDuration);

        itemCooldowns.Remove(itemName);
    }


    void Deactivateshoes()
    {
        // 받은 오브젝트를 비활성화합니다.
        ShoesPrefab.SetActive(false);
    }
    void Deactivatefeather()
    {
        // 받은 오브젝트를 비활성화합니다.
        featherPrefab.SetActive(false);
    }
    void DeactivateDetector()
    {
        // 받은 오브젝트를 비활성화합니다.
        Detector.SetActive(false);
        
    }
    void DeactivateHourglass()
    {
        // 받은 오브젝트를 비활성화합니다.
        HourGlass.SetActive(false);

    }



}
