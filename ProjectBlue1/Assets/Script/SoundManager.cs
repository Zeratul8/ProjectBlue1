using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    public enum BGMType
    {
        None = -1,
        Login,
        Main,
        Max
    }
    public enum VFXType
    {
        None = -1,
        SwordAttack,
        SwordAura,
        Damaged,
        SkeletonDie,

        Walk,

        Max
    }

    [SerializeField]
    AudioClip[] BGMClip;
    [SerializeField]
    AudioClip[] VFXClip;
    [SerializeField]
    AudioSource BGMAudio;
    [SerializeField]
    AudioSource VFXAudio;

    Dictionary<BGMType, List<AudioClip>> BGMdic = new Dictionary<BGMType, List<AudioClip>>();
    Dictionary<VFXType, List<AudioClip>> VFXDic = new Dictionary<VFXType, List<AudioClip>>();

    protected override void OnAwake()
    {
        Type bgmType = typeof(BGMType);
        Type vfxType = typeof(VFXType);

        List<AudioClip>[] bgmClips = new List<AudioClip>[(int)BGMType.Max];
        foreach (AudioClip clip in BGMClip)
        {
            string name = clip.name.Split('_')[1];

            BGMType type = (BGMType)Enum.Parse(bgmType, name);

            

            switch (type)
            {
                case BGMType.Login:
                    SetDictionary(BGMdic, bgmClips, type, clip);
                    break;
                case BGMType.Main:
                    SetDictionary(BGMdic, bgmClips, type, clip);
                    break;
                default:
                    break;
            }
        }

        List<AudioClip>[] vfxClips = new List<AudioClip>[(int)VFXType.Max];
        foreach (AudioClip clip in VFXClip)
        {
            string name = clip.name.Split('_')[1];

            VFXType type = (VFXType)Enum.Parse(vfxType, name);

            

            switch (type)
            {
                case VFXType.SwordAttack:
                    SetDictionary(VFXDic, vfxClips, type, clip);
                    break;
                case VFXType.SwordAura:
                    SetDictionary(VFXDic, vfxClips, type, clip);
                    break;
                case VFXType.Walk:
                    SetDictionary(VFXDic, vfxClips, type, clip);
                    break;
                case VFXType.Damaged:
                    SetDictionary(VFXDic, vfxClips, type, clip);
                    break;
                case VFXType.SkeletonDie:
                    SetDictionary(VFXDic, vfxClips, type, clip);
                    break;
                default:
                    break;
            }
        }
    }
    void SetDictionary(Dictionary<BGMType, List<AudioClip>> dic, List<AudioClip>[] audioClips, BGMType type, AudioClip clip)
    {
        dic.Remove(type);
        if (audioClips[(int)type] == null) audioClips[(int)type] = new List<AudioClip>();
        audioClips[(int)type].Add(clip);
        dic.Add(type, audioClips[(int)type]);
    }
    void SetDictionary(Dictionary<VFXType, List<AudioClip>> dic, List<AudioClip>[] audioClips, VFXType type, AudioClip clip)
    {
        dic.Remove(type);
        if (audioClips[(int)type] == null) audioClips[(int)type] = new List<AudioClip>();
        audioClips[(int)type].Add(clip);
        dic.Add(type, audioClips[(int)type]);
    }
    protected override void OnStart()
    {
        PlayBattleBGM();
    }

    public void PlayBattleBGM()
    {
        List<AudioClip> value = BGMdic[BGMType.Main];
        BGMAudio.clip = value[Random.Range(0, value.Count)];
        BGMAudio.loop = true;
        BGMAudio.Play();
    }

    public void Damaged()
    {
        List<AudioClip> value = VFXDic[VFXType.Damaged];
        int rand = Random.Range(0, value.Count);
        if (rand == 1)
            VFXAudio.volume = 0.5f;
        else
            VFXAudio.volume = 1f;
        VFXAudio.PlayOneShot(value[rand]);
    }
    public void ShootingAura()
    {
        List<AudioClip> value = VFXDic[VFXType.SwordAura];
        VFXAudio.PlayOneShot(value[Random.Range(0, value.Count)]);
    }
    public void SkeletonDie()
    {
        List<AudioClip> value = VFXDic[VFXType.SkeletonDie];
        VFXAudio.PlayOneShot(value[Random.Range(0, value.Count)]);
    }
    public void Walk()
    {
        List<AudioClip> value = VFXDic[VFXType.Walk];
        VFXAudio.clip = value[Random.Range(0, value.Count)];
        VFXAudio.loop = true;
        VFXAudio.Play();
    }
    public void StopVFX()
    {
        VFXAudio.Stop();
    }
}
