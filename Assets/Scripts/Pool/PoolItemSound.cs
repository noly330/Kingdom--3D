using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolItemSound : PoolItemBase
{
    private AudioSource _audioSource;
    [SerializeField] private SoundType _soundType;
    [SerializeField] private AudioSO _audioSO;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public override void Spawn()
    {
        //当自身被激活的时候播放声音
        PlaySound();
    }

    private void PlaySound()
    {
        _audioSource.clip = _audioSO.GetAudioClip(_soundType);
        _audioSource.Play();
        StartRecycle();
    }

    private void StartRecycle()
    {
        TimerManager.Instance.TryGetOneTimer(_audioSource.clip.length *0.5f, DisableSelf);
    }

    private void DisableSelf()
    {
        _audioSource.Stop();
        this.gameObject.SetActive(false);
        ObjectPool.instance.ReturnPool(ObjectPoolType.FootEffect,gameObject);
    }
}

public enum SoundType
{
    //战斗相关
    Attack,Hit,

    //移动相关
    Foot,
}
