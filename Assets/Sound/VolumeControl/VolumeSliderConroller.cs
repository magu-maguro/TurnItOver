using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// 音量調整のスライダー２つにアタッチされているスクリプト
/// 
/// </summary>
public class VolumeSliderConroller : MonoBehaviour
{
    private enum SoundType//switch文使いたいがための列挙型
    {
        BGM, SE
    }
    [SerializeField] private SoundType type;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider Slider;//自分
    [SerializeField] private TextMeshProUGUI ValueText;
    void Start()
    {
        //PlayerPrefsから音量の値を取得してもよいけど取り敢えずコメントアウトしとく
        //結局実装しなかったのでPlayerPrefs使いたかったら自分で追加してね

        //ミキサーのvolumeにスライダーのvolumeを入れる
        switch(type)
        {
            case SoundType.BGM:
                audioMixer.GetFloat("BGM", out float BGMVolume);
                Slider.value = BGMVolume;
                break;
            case SoundType.SE:
                audioMixer.GetFloat("SE", out float SEVolume);
                Slider.value = SEVolume;
                break;
        }
        
    }

    public void SetVolume(float volume)
    {
        //スライダーの値が変わるたびに呼ばれる
        //Maxとminが-60と0なのでこういう計算式になっていますが調整したかったら頑張って
        ValueText.text = ((volume+60)*100/60).ToString("0.0");
        switch(type)
        {
            case SoundType.BGM:
                audioMixer.SetFloat("BGM", volume);
                break;
            case SoundType.SE:
                audioMixer.SetFloat("SE", volume);
                break;
        }
        //PlayerPrefs.SetFloat("Volume", value);
    }
}
