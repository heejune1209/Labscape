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
    public AudioMixer theMixer;  // 오디오 믹서 
    public Slider musicSlider, sfxSlider; // 음악 및 사운드 이펙트 볼륨 슬라이더

    private void Start()
    {
        float vol = 0f;

        // 오디오 믹서에서 음악 볼륨 값을 가져와 슬라이더의 값으로 설정
        theMixer.GetFloat("MusicVol", out vol);
        musicSlider.value = vol;
        theMixer.GetFloat("SFXVol", out vol);
        sfxSlider.value = vol;

        Mathf.RoundToInt(musicSlider.value - 30).ToString(); // 초기값 50
        Mathf.RoundToInt(sfxSlider.value - 30).ToString(); // 초기값 50
    }
    public void SetMusicVol() // 오디오 믹서에 익스포스 파라메터에 MusicVol로 이름 설정
    {
        Mathf.RoundToInt(musicSlider.value + 80).ToString();
        theMixer.SetFloat("MusicVol", musicSlider.value);

        PlayerPrefs.SetFloat("MusicVol", musicSlider.value);
    }

    public void SetSFXVol() // 오디오 믹서에 익스포스 파라메터에 SFXVol으로 이름 설정
    {
        Mathf.RoundToInt(sfxSlider.value + 80).ToString();
        theMixer.SetFloat("SFXVol", sfxSlider.value);

        PlayerPrefs.SetFloat("SFXVol", sfxSlider.value);

    }
    public void MuteMusic() // 음악 소리 끄기
    {
        theMixer.SetFloat("MusicVol", -80); // 소리를 최저로 설정하여 음소거
        PlayerPrefs.SetFloat("MusicVol", -80); // 설정 저장
        musicSlider.value = -80; // 슬라이더 값 업데이트
    }

    public void MuteSFX() // 사운드 이펙트 소리 끄기
    {
        theMixer.SetFloat("SFXVol", -80); // 소리를 최저로 설정하여 음소거
        PlayerPrefs.SetFloat("SFXVol", -80); // 설정 저장
        sfxSlider.value = -80; // 슬라이더 값 업데이트
    }
    public void CloseUI()    // 버튼으로 로직 연결, UI닫는 메소드 호출 기능
    {
        animator = GetComponent<Animator>();
        StartCoroutine("CloseAfterDelay1");
    }

    private IEnumerator CloseAfterDelay1()  // UI창 닫는 애니메이션 실행과 0.5초 텀 추가
    {
        animator.SetTrigger("Close");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        animator.ResetTrigger("Close");
    }
    
    

    
}
