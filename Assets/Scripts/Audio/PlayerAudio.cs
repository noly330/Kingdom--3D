using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip jumpAudioClip;
    public void PlaySFX(float volume)
    {
        AudioManager.instance.PlaySFX(jumpAudioClip, volume); 
    }
}
