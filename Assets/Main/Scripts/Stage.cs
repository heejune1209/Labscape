using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    public GameObject panel;
    public GameObject warningPanel;
    public Image[] stage1Gems;
    public Image[] stage2Gems;
    public Image[] stage3Gems;
    public Image[] stage4Gems;
    public Image[] stage5Gems;    

    void Start()
    {
        SetGemAlphas(stage1Gems, 1);
        SetGemAlphas(stage2Gems, 2);
        SetGemAlphas(stage3Gems, 3);
        SetGemAlphas(stage4Gems, 4);
        SetGemAlphas(stage5Gems, 5);
        
    }

    void SetGemAlphas(Image[] gems, int stageNumber)
    {
        for (int i = 0; i < gems.Length; i++)
        {
            string key = "Gem" + stageNumber + "_" + i;
            float alpha = PlayerPrefs.GetFloat(key, 0f);
            Color color = gems[i].color;
            color.a = alpha;
            gems[i].color = color;
        }
    }

    public void GoToStageScene(string stageName)
    {
        Time.timeScale = 1.0f;
        int stageNumber = int.Parse(stageName.Replace("Stage", ""));
        /*
        for (int i = 1; i < stageNumber; i++)
        {
            if (PlayerPrefs.GetInt("Stage" + i, 0) != 1)
            {
                warningPanel.SetActive(true);
                Time.timeScale = 0f;
                return;
            }
        }
        */
        PlayerPrefs.SetString("nextScene", stageName);
        AudioManager.instance.PlaySFX(0);
        SceneManager.LoadScene("LoadingScene");        
        AudioManager.instance.PlayBGM(1);
    }

    public void ActivatePanel()
    {
        panel.SetActive(true);
    }

    public void DeactivatePanel()
    {
        
        panel.SetActive(false);
    }

    

    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }
}
