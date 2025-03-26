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

    public GameObject featherPrefab; // ������ ����
    public GameObject ShoesPrefab; // �ٸ� ������ ������
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
    public float itemDuration = 6f; // ������ ���ӽð�
    public float HourGlassDuration = 2.5f; // �𷡽ð� ���ӽð�
    //public CoolTime coolTime;

    public GameObject player;
    [HideInInspector]
    public Vector3 playerRespawnPosition; // ����� ����� ��ġ ����� ����

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

    public void UseItem(string itemName) // ������ ���
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
            // ������ ȿ��
            itemCount--;
            PlayerPrefs.SetInt(itemName, itemCount);
            PlayerPrefs.Save();

            UpdateItemCount();

            // ������ ������ ����
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
        // ���� ������Ʈ�� ��Ȱ��ȭ�մϴ�.
        ShoesPrefab.SetActive(false);
    }
    void Deactivatefeather()
    {
        // ���� ������Ʈ�� ��Ȱ��ȭ�մϴ�.
        featherPrefab.SetActive(false);
    }
    void DeactivateDetector()
    {
        // ���� ������Ʈ�� ��Ȱ��ȭ�մϴ�.
        Detector.SetActive(false);
        
    }
    void DeactivateHourglass()
    {
        // ���� ������Ʈ�� ��Ȱ��ȭ�մϴ�.
        HourGlass.SetActive(false);

    }



}
