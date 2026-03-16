using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDemoButtonController : MonoBehaviour
{
    [SerializeField] private int index;
    [SerializeField] private float fadeInDuration;
    [SerializeField] private float fadeOutDuration;
    [SerializeField] private float volume;
    [SerializeField] private float waitTime;

    public void PlayBGMButton()
    {
        StartCoroutine(SoundManager.PlayBGM(index, fadeInDuration));
    }

    public void StopBGMButton()
    {
        StartCoroutine(SoundManager.StopBGM(fadeOutDuration));
    }

    public void PauseBGMButton()
    {
        StartCoroutine(SoundManager.PauseBGM(fadeOutDuration));
    }

    public void ResumeBGMButton()
    {
        StartCoroutine(SoundManager.ResumeBGM(fadeInDuration));
    }

    public void ChangeBGMVolumeButton()
    {
        StartCoroutine(SoundManager.ChangeBGMVolume(volume, fadeInDuration));
    }

    public void ChangeBGMButton()
    {
        StartCoroutine(SoundManager.ChangeBGM(index, fadeOutDuration, waitTime, fadeInDuration));
    }

    public void CrossFadeBGMButton()
    {
        StartCoroutine(SoundManager.CrossFadeBGM(index, fadeInDuration));
    }

    public void PlaySEButton()
    {
        StartCoroutine(SoundManager.PlaySE(index, volume));
    }
}
