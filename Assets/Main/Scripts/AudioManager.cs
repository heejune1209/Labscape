using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // 싱글톤 인스턴스 (전역 접근 가능)
    public static AudioManager instance;

    // 오디오 믹서 참조 (BGM, SFX 등 그룹별로 볼륨 제어)
    public AudioMixer theMixer;

    // 배경 음악으로 사용할 오디오 클립 배열
    public AudioClip[] clips;

    // 기본 볼륨 (플레이어 설정이 없을 경우 사용)
    float defaultVolume = 0.8f;

    // 배경 음악 재생을 위한 AudioSource
    private AudioSource audioSource;

    // 현재 재생 중인 클립의 인덱스
    private int currentClipIndex = 0;

    // 루프 구간 시작 인덱스 (반복 재생할 구간)
    private int loopStart = 2;

    // 루프 구간 종료 인덱스
    private int loopEnd = 6;

    // 효과음으로 사용할 오디오 클립 배열
    public AudioClip[] sfxClips;

    // 효과음 재생용 AudioSource (기본 AudioSource 외에 추가됨)
    private AudioSource sfxSource;

    // 동적으로 생성된 모든 효과음 AudioSource들을 관리하기 위한 리스트
    private List<AudioSource> sfxSources = new List<AudioSource>();

    // Awake: 인스턴스 초기화 및 싱글톤 패턴 구현
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // 씬 전환 시 파괴되지 않도록 설정
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 이미 인스턴스가 존재하면 중복된 오브젝트 삭제
            Destroy(gameObject);
        }

        // 해당 오브젝트에 부착된 AudioSource를 가져옴
        audioSource = GetComponent<AudioSource>();
        // AudioSource의 출력 그룹을 믹서 내 "BackGround Music" 그룹으로 설정
        audioSource.outputAudioMixerGroup = theMixer.FindMatchingGroups("BackGround Music")[0];

        // 효과음 재생을 위한 별도의 AudioSource 추가
        sfxSource = gameObject.AddComponent<AudioSource>();
    }

    // 배경 음악(BGM)을 재생하는 메서드
    public void PlayBGM(int index)
    {
        // 유효한 인덱스 범위인지 확인
        if (index < 0 || index >= clips.Length) return;

        // 지정한 인덱스의 클립을 할당하고 재생
        audioSource.clip = clips[index];
        audioSource.Play();
        currentClipIndex = index;
    }

    // 배경 음악 재생을 중지하는 메서드
    public void StopBGM()
    {
        audioSource.Stop();
    }

    // 동적으로 생성한 모든 효과음 AudioSource를 삭제하는 메서드
    public void Destroy()
    {
        foreach (AudioSource sfxSource in sfxSources)
        {
            Destroy(sfxSource);
        }
        sfxSources.Clear();
    }

    // 매 프레임마다 호출되며, 배경 음악이 끝나면 다음 클립을 재생하는 로직
    private void Update()
    {
        // 배경 음악이 재생 중이 아니면
        if (!audioSource.isPlaying)
        {
            // 첫 두 클립(인덱스 0,1)은 반복 재생
            if (currentClipIndex == 0 || currentClipIndex == 1)
            {
                PlayBGM(currentClipIndex);
            }
            // 지정된 루프 구간 내의 클립들은 순차적으로 재생 후 루프
            else if (currentClipIndex >= loopStart && currentClipIndex <= loopEnd)
            {
                currentClipIndex++;

                // 루프 구간 끝에 도달하면 루프 시작 인덱스로 재설정
                if (currentClipIndex > loopEnd)
                {
                    currentClipIndex = loopStart;
                }
                PlayBGM(currentClipIndex);
            }
        }
    }

    // 효과음(SFX)을 재생하는 메서드
    public void PlaySFX(int index)
    {
        // 유효한 인덱스 범위인지 확인
        if (index < 0 || index >= sfxClips.Length) return;

        // 새 AudioSource를 추가하여 효과음 재생
        AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.clip = sfxClips[index];
        // AudioSource의 출력 그룹을 믹서 내 "Effect Sound Group"으로 설정
        sfxSource.outputAudioMixerGroup = theMixer.FindMatchingGroups("Effect Sound Group")[0];

        // 효과음 재생 시작
        sfxSource.Play();

        // 클립 길이만큼 후에 AudioSource를 자동으로 삭제
        Destroy(sfxSource, sfxClips[index].length);
        // 리스트에 추가하여 관리
        sfxSources.Add(sfxSource);
    }

    // Start: 게임 시작 시 플레이어의 볼륨 설정을 로드하여 믹서에 적용
    void Start()
    {
        // 저장된 배경 음악 볼륨이 있으면 적용, 없으면 기본 볼륨 사용
        if (PlayerPrefs.HasKey("MusicVol"))
        {
            theMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
        }
        else
        {
            theMixer.SetFloat("MusicVol", defaultVolume);
        }

        // 저장된 효과음 볼륨이 있으면 적용, 없으면 기본 볼륨 사용
        if (PlayerPrefs.HasKey("SFXVol"))
        {
            theMixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVol"));
        }
        else
        {
            theMixer.SetFloat("SFXVol", defaultVolume);
        }
    }
}