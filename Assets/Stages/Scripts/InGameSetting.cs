using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameSetting : MonoBehaviour
{
    
    public GameObject PauseUI;
    private Animator animator;
    // Start is called before the first frame update
       
    public void CloseUI()    // ��ư���� ���� ����, UI�ݴ� �޼ҵ� ȣ�� ���
    {
        PauseUI.GetComponent<Animator>();
        StartCoroutine("CloseAfterDelay1");
    }

    private IEnumerator CloseAfterDelay1()  // UIâ �ݴ� �ִϸ��̼� ����� 0.5�� �� �߰�
    {
        animator = PauseUI.GetComponent<Animator>();
        animator.SetTrigger("Close");
        yield return new WaitForSeconds(0.5f);
        PauseUI.SetActive(false);
        animator.ResetTrigger("Close");
    }
    public void LoadScene(string sceneName)
    {
        PlayerPrefs.SetString("nextScene", sceneName);
        SceneManager.LoadScene("LoadingScene");
    }
    public void InActiveUI()
    {
        PauseUI.SetActive(false);
        Time.timeScale = 1f;
    }
    public void ActiveUI()
    {
        PauseUI.SetActive(true);
        Time.timeScale = 0f;
    }
    public void GoToLobbyScene()
    {
        Time.timeScale = 1f;
        GameManager.Instance.GoToLobby();
        //PlayerPrefs.DeleteKey("PlayerRespawnX");
        //PlayerPrefs.DeleteKey("PlayerRespawnY");
        //PlayerPrefs.DeleteKey("PlayerRespawnZ");
        AudioManager.instance.Destroy();
        PlayerPrefs.SetString("nextScene", "Main Scene");
        SceneManager.LoadScene("LoadingScene");
        AudioManager.instance.PlayBGM(0);
    }
}
