using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    private Animator animator;
    public AudioMixer theMixer;  // ����� �ͼ� 
    public Slider musicSlider, sfxSlider; // ���� �� ���� ����Ʈ ���� �����̴�

    private void Start()
    {
        float vol = 0f;

        // ����� �ͼ����� ���� ���� ���� ������ �����̴��� ������ ����
        theMixer.GetFloat("MusicVol", out vol);
        musicSlider.value = vol;
        theMixer.GetFloat("SFXVol", out vol);
        sfxSlider.value = vol;

        Mathf.RoundToInt(musicSlider.value - 30).ToString(); // �ʱⰪ 50
        Mathf.RoundToInt(sfxSlider.value - 30).ToString(); // �ʱⰪ 50
    }
    public void SetMusicVol() // ����� �ͼ��� �ͽ����� �Ķ���Ϳ� MusicVol�� �̸� ����
    {
        Mathf.RoundToInt(musicSlider.value + 80).ToString();
        theMixer.SetFloat("MusicVol", musicSlider.value);

        PlayerPrefs.SetFloat("MusicVol", musicSlider.value);
    }

    public void SetSFXVol() // ����� �ͼ��� �ͽ����� �Ķ���Ϳ� SFXVol���� �̸� ����
    {
        Mathf.RoundToInt(sfxSlider.value + 80).ToString();
        theMixer.SetFloat("SFXVol", sfxSlider.value);

        PlayerPrefs.SetFloat("SFXVol", sfxSlider.value);

    }
    public void MuteMusic() // ���� �Ҹ� ����
    {
        theMixer.SetFloat("MusicVol", -80); // �Ҹ��� ������ �����Ͽ� ���Ұ�
        PlayerPrefs.SetFloat("MusicVol", -80); // ���� ����
        musicSlider.value = -80; // �����̴� �� ������Ʈ
    }

    public void MuteSFX() // ���� ����Ʈ �Ҹ� ����
    {
        theMixer.SetFloat("SFXVol", -80); // �Ҹ��� ������ �����Ͽ� ���Ұ�
        PlayerPrefs.SetFloat("SFXVol", -80); // ���� ����
        sfxSlider.value = -80; // �����̴� �� ������Ʈ
    }
    public void CloseUI()    // ��ư���� ���� ����, UI�ݴ� �޼ҵ� ȣ�� ���
    {
        animator = GetComponent<Animator>();
        StartCoroutine("CloseAfterDelay1");
    }

    private IEnumerator CloseAfterDelay1()  // UIâ �ݴ� �ִϸ��̼� ����� 0.5�� �� �߰�
    {
        animator.SetTrigger("Close");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        animator.ResetTrigger("Close");
    }
    
    

    
}
