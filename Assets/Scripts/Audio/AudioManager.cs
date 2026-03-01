using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource BGM_Source;
    public AudioSource SFX_Source;

    public AudioClip[] BGM_Clips;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        
        StartCoroutine(IE_PlayBGM(BGM_Clips[RandomBGM()]));
    }

    IEnumerator IE_PlayBGM(AudioClip audioClip)
    {
        BGM_Source.clip = audioClip;;
        BGM_Source.Play();
        float playBgmTime = audioClip.length + 1f;

        yield return new WaitForSecondsRealtime(playBgmTime);
        StartCoroutine(IE_PlayBGM(BGM_Clips[RandomBGM()]));

    }
    public void PlaySFX(AudioClip audioClip,float volume)
    {
        SFX_Source.volume = volume;
        SFX_Source.PlayOneShot(audioClip);
    }

    private int RandomBGM()
    {
        return Random.Range(0, BGM_Clips.Length);
    }

}
