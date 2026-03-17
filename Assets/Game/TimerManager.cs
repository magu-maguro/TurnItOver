using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    public void SetupTimer(int timeLimit)
    {
        DisplayTime(timeLimit);
    }
    public IEnumerator UpdateTimer(int timeLimit)
    {
        int timer = timeLimit;
        DisplayTime(timer);
        while (timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer--;
            DisplayTime(timer);
        }
    }

    private void DisplayTime(int time)
    {
        // タイマーのUIを更新する処理をここに追加
        timerText.text = time.ToString();
        // 文字を一瞬拡大縮小
        timerText.transform.DOScale(1.2f, 0.1f).OnComplete(() =>
        {
            timerText.transform.DOScale(1f, 0.1f);
        });
    }
}
