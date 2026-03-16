
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Assets/Audio/AudioSO")]

public class AudioSO : ScriptableObject
{
    [SerializeField]
    private List<AudioData> audioDataList = new List<AudioData>();

    public AudioClip GetAudioClip(SoundType type)
    {
        if(audioDataList.Count == 0)
        {
            Debug.LogError("AudioSO 中没有音频数据");
        }
        switch (type)
        {
            case SoundType.Foot:
                return audioDataList[2].audioClip[Random.Range(0, audioDataList[2].audioClip.Length)];
        }

        return null;
    }
}

[System.Serializable]
public class AudioData
{
    public SoundType soundType;
    public AudioClip[] audioClip;
}