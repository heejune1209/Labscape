using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ItemManager : MonoBehaviour
{
    public int gems;
    public int itemPrice;
    public int gold;
    public int gemToGoldRatio = 400; // ���г��� �ھ� ȯ��

    public TMP_Text gemText;
    public TMP_Text goldText;
    public TMP_Text[] itemCountTexts;

    public Image featherDescriptionImage;
    public Image flagDescriptionImage;
    public Image lampDescriptionImage;
    public Image wingDescriptionImage;

    public Button gemToGoldButton1;
    public Button gemToGoldButton10;

    public GameObject EmptyGoldPanel; // rh ������ �г�
    public GameObject EmptyGemsPanel;

    public GameObject ShopUI;
    private Animator animator;

    public class Item
    {
        public string name; 
        public int price;

        public Item(string name, int price)
        {
            this.name = name;
            this.price = price;
        }
    }

    public Item[] itemArray; 

    private Dictionary<string, Item> items; 
    private Dictionary<string, int> itemCounts;

    void Start()
    {
        PlayerPrefs.SetInt("Gem", 50); // ���⼭ ���г� ���� ���ð�����.(�����ڿ� �ƴҶ� �� �ڵ带 �ּ� ó���ؾ���.)
        gems = PlayerPrefs.GetInt("Gem", 0);
        gold = PlayerPrefs.GetInt("Gold", 0);
        items = new Dictionary<string, Item>(); 
        itemCounts = new Dictionary<string, int>();

        itemArray = new Item[]
   {
        new Item("Feather", 150),
        new Item("Shoes", 150),
        new Item("Detector", 200),
        new Item("HourGlass", 200),
   };
        foreach (Item item in itemArray)
        {
            items.Add(item.name, item);
            itemCounts[item.name] = PlayerPrefs.GetInt(item.name, 0); 
        }

        gemToGoldButton1.onClick.AddListener(() =>
        {
            if (gems >= 1)
            {
                ExchangeGemsToGold(1); // 1�� -> 100���
                AudioManager.instance.PlaySFX(1);
            }
            else
            {
                EmptyGemsPanel.SetActive(true);
                AudioManager.instance.PlaySFX(2);
            }
        });

        gemToGoldButton10.onClick.AddListener(() =>
        {
            if (gems >= 10)
            {
                ExchangeGemsToGold(10); // 10�� -> 1000���
                AudioManager.instance.PlaySFX(1);
            }
            else
            {
                EmptyGemsPanel.SetActive(true);
                AudioManager.instance.PlaySFX(2);
            }
        });

        UpdateUI();
    }

    void Update()
    {
        gems = PlayerPrefs.GetInt("Gem", 0);
        UpdateUI();

        
    }

    public void BuyItem(string itemName)
    {
        Item item = items[itemName];

        if (gold >= item.price)
        {
            gold -= item.price;
            PlayerPrefs.SetInt("Gold", gold);
            PlayerPrefs.Save();

            if (itemCounts.ContainsKey(itemName))
            {
                itemCounts[itemName]++;
            }
            else
            {
                itemCounts.Add(itemName, 1);
            }

            PlayerPrefs.SetInt(itemName, itemCounts[itemName]);
            PlayerPrefs.Save();

            UpdateUI();
        }
        else
        {
            AudioManager.instance.PlaySFX(2);
            EmptyGoldPanel.SetActive(true); // ��尡 �����ϸ� �г��� Ȱ��ȭ
        }
    }


    void UpdateUI()
    {
        gemText.text = ": " + gems;
        goldText.text = ": " + gold; 

        foreach (var item in items)
        {
            for (int i = 0; i < itemCountTexts.Length; i++)
            {
                //  ���� ǥ��
                if (itemCountTexts[i].name == item.Key)
                {
                    itemCountTexts[i].text = (itemCounts.ContainsKey(item.Key) ? itemCounts[item.Key].ToString() : "0");
                    break;
                }
            }
        }
    }

    public void ExchangeGemsToGold(int gemAmount)
    {
        // �� ������ ������� Ȯ��
        if (gems >= gemAmount)
        {
            gems -= gemAmount;
            gold += gemAmount * gemToGoldRatio; // ���� ���� ȯ��

            PlayerPrefs.SetInt("Gem", gems);
            PlayerPrefs.SetInt("Gold", gold); // ��� �� ����
            PlayerPrefs.Save();

            UpdateUI();
        }
    }

    public void CloseShopUI()    // ��ư���� ���� ����, UI�ݴ� �޼ҵ� ȣ�� ���
    {        
        StartCoroutine("CloseAfterDelay1");
    }

    private IEnumerator CloseAfterDelay1()  // UIâ �ݴ� �ִϸ��̼� ����� 0.5�� �� �߰�
    {
        animator = ShopUI.GetComponent<Animator>();
        animator.SetTrigger("Close");
        yield return new WaitForSeconds(0.5f);
        ShopUI.SetActive(false);
        animator.ResetTrigger("Close");
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    public void OFFGemsPanel()
    {
        EmptyGemsPanel.SetActive(false);
    }

    public void OFFGoldPanel()
    {
        EmptyGoldPanel.SetActive(false);
    }

    public void OnBuyButtonClick(string itemName)
    {
        BuyItem(itemName);
    }

    public void ShowFeatherDescription()
    {
        featherDescriptionImage.gameObject.SetActive(true);
    }

    public void ShowFlagDescription()
    {
        flagDescriptionImage.gameObject.SetActive(true);
    }

    public void ShowLampDescription()
    {
        lampDescriptionImage.gameObject.SetActive(true);
    }
    public void ShowWingDescription()
    {
        wingDescriptionImage.gameObject.SetActive(true);
    }

    public void HideFeatherDescription()
    {
        featherDescriptionImage.gameObject.SetActive(false);
    }

    public void HideFlagDescription()
    {
        flagDescriptionImage.gameObject.SetActive(false);
    }

    public void HideLampDescription()
    {
        lampDescriptionImage.gameObject.SetActive(false);
    }

    public void HideWingDescription()
    {
        wingDescriptionImage.gameObject.SetActive(false);
    }
}