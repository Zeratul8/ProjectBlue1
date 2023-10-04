using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsManager : MonoBehaviour
{
    [SerializeField]
    Button closeBtn;
    [SerializeField]
    Button backGround;
    [SerializeField]
    Slider bgmVolume;
    [SerializeField]
    Slider vfxVolume;

    private void Start()
    {
        closeBtn.onClick.AddListener(()=>gameObject.SetActive(false));
        backGround.onClick.AddListener(()=>gameObject.SetActive(false));
        bgmVolume.onValueChanged.AddListener((value) => SoundManager.Instance.SetBGMVolume(value));
        vfxVolume.onValueChanged.AddListener((value) => SoundManager.Instance.SetVFXVolume(value));
    }

}
