using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 全シーンに１つだけ存在してほしいスクリプトなのでシングルトンやってます
/// Awake()メソッドで孫オブジェクトのAudioSourceをBGMとSEそれぞれ順番通りに取得し配列に入れています
/// BGMとSEを管理し、それらを使うためのコルーチンを提供する
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }

    public static List<AudioSource> BGMSources = new List<AudioSource>();
    public static List<AudioSource> SESources = new List<AudioSource>();

    [SerializeField] private Transform BGM_Parent;
    [SerializeField] private Transform SE_Parent;

    private static AudioSource currentBGM;

    void Awake()
    {
        //シングルトンってやつ！？
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        // BGM の AudioSource をリストに追加
        foreach (Transform child in BGM_Parent) BGMSources.Add(child.GetComponent<AudioSource>());

        // SE の AudioSource をリストに追加
        foreach (Transform child in SE_Parent) SESources.Add(child.GetComponent<AudioSource>());

    }
    void Start()
    {
        if (BGMSources.Count > 0)
        {
            currentBGM = BGMSources[0];
            currentBGM.Play();
        }
        else
        {
            Debug.LogWarning("No BGM sources available.");
        }
    }
    //------------------------以下他のスクリプトから利用可能なコルーチン------------------------

    //BGMを再生。引数はインデックスとフェードイン時間。
    public static IEnumerator PlayBGM(int index, float fadeDuration = 0f)
    {
        if (BGMSources == null || BGMSources.Count == 0 ||index < 0 || index >= BGMSources.Count || BGMSources[index].clip == null)
        {
            Debug.Log("PlayBGM : 設定がおかしいよ");
            yield break;
        }

        currentBGM = BGMSources[index];
        if (fadeDuration > 0f)
        {
            currentBGM.volume = 0f;
            currentBGM.Play();
            yield return currentBGM.DOFade(endValue: 1f, duration: fadeDuration).WaitForCompletion();
        }
        else
        {
            currentBGM.volume = 1f;
            currentBGM.Play();
        }

    }

    //BGMを停止。引数はフェードアウト時間。
    public static IEnumerator StopBGM(float fadeDuration = 0f)
    {
        if (currentBGM == null || currentBGM.clip == null)
        {
            Debug.Log("StopBGM : 設定がおかしいよ");
            yield break;
        }
        yield return currentBGM.DOFade(endValue: 0f, duration: fadeDuration).WaitForCompletion();
        currentBGM.Stop();
        //currentBGM.volume = 1f;
    }

    //BGMを一時停止。引数はフェードアウト時間。
    public static IEnumerator PauseBGM(float fadeDuration = 0f)
    {
        if (currentBGM == null || currentBGM.clip == null)
        {
            Debug.Log("PauseBGM : 設定がおかしいよ");
            yield break;
        }
        if (currentBGM.isPlaying)
        {
            if (fadeDuration > 0f)
            {
                yield return currentBGM.DOFade(0f, fadeDuration).WaitForCompletion();
            }
            currentBGM.Pause();
        }
    }

    //BGMを再開。引数はフェードイン時間。
    public static IEnumerator ResumeBGM(float fadeDuration = 0f)
    {
        if (currentBGM == null || currentBGM.clip == null)
        {
            Debug.Log("ResumeBGM : 設定がおかしいよ");
            yield break;
        }
        if (!currentBGM.isPlaying)
        {
            currentBGM.Play();
            if (fadeDuration > 0f)
            {
                yield return currentBGM.DOFade(1f, fadeDuration).WaitForCompletion();
            }
            else
            {
                currentBGM.volume = 1f;
            }
        }
    }

    //BGMのボリュームを変更。引数は変更後の音量（0～1）とフェード時間。
    public static IEnumerator ChangeBGMVolume(float volume, float fadeDuration = 0f)
    {
        if (currentBGM == null || currentBGM.clip == null)
        {
            Debug.Log("ChangeBGMVolume : 設定がおかしいよ");
            yield break;
        }
        yield return currentBGM.DOFade(endValue: volume, duration: fadeDuration).WaitForCompletion();
    }

    //現在のBGMを停止させてから次のBGM再生。引数はインデックス、フェードアウト時間、BGM間の待ち時間、フェードイン時間。
    public static IEnumerator ChangeBGM(int nextBGM, float fadeOutDuration = 0f, float waitTime = 0f, float fadeInDuration = 0f)
    {
        if (currentBGM == null || currentBGM.clip == null)
        {
            Debug.Log("ChangeBGM : currentBGM設定がおかしいよ");
            yield break;
        }
        if (nextBGM < 0 || nextBGM >= BGMSources.Count)
        {
            Debug.Log("ChangeBGM : nextBGM設定がおかしいよ");
            yield break;
        }
        if (currentBGM != BGMSources[nextBGM])
        {
            yield return currentBGM.DOFade(endValue: 0f, duration: fadeOutDuration).WaitForCompletion();
            currentBGM.Stop();
            yield return new WaitForSeconds(waitTime);

            yield return PlayBGM(nextBGM, fadeInDuration);
        }
    }

    //現在のBGMをフェードアウトしながら次のBGMをフェードイン。引数はインデックスとフェード時間。
    public static IEnumerator CrossFadeBGM(int nextBGM, float fadeDuration)
    {
        if (currentBGM == null || currentBGM.clip == null)
        {
            Debug.Log("CrossFadeBGM : currentBGM設定がおかしいよ");
            yield break;
        }
        if (nextBGM < 0 || nextBGM >= BGMSources.Count)
        {
            Debug.Log("CrossFadeBGM : nextBGM設定がおかしいよ");
            yield break;
        }

        AudioSource newBGM = BGMSources[nextBGM];

        if (currentBGM != newBGM)
        {
            newBGM.volume = 0f;
            newBGM.Play();

            Sequence seq = DOTween.Sequence();
            yield return seq.Append(currentBGM.DOFade(0f, fadeDuration))
                            .Join(newBGM.DOFade(1f, fadeDuration))
                            .WaitForCompletion();

            currentBGM.Stop();
            currentBGM = newBGM;

            //yield return seq.WaitForCompletion();
        }
    }

    //SEを再生。引数はインデックスと音量。
    public static IEnumerator PlaySE(int index, float volume = 1.0f)
    {
        if (SESources == null || SESources.Count == 0 || index < 0 || index >= SESources.Count || SESources[index].clip == null)
        {
            Debug.Log("PlaySE : 設定がおかしいよ");
            yield break;
        }

        SESources[index].PlayOneShot(SESources[index].clip, volume);
        yield return null;
    }
}
