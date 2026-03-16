using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CenterTextManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI centerText;
    [SerializeField] private TextMeshProUGUI resultText;
    private Tween currentTween;
    void Start()
    {
        centerText.rectTransform.localPosition = Vector3.right * 1000;
        resultText.rectTransform.localPosition = Vector3.right * 1000;
    }

    public void ShowText(string text)
    {
        centerText.rectTransform.localPosition = Vector3.right * 1000;
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }

        centerText.text = text;
        currentTween = centerText.rectTransform.DOLocalMoveX(0, 0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            HideText();
        });
    }

    private void HideText()
    {
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }
        currentTween = centerText.rectTransform.DOLocalMoveX(-1000, 0.4f).SetEase(Ease.InCubic);
    }

    public void ShowResultText(string text)
    {
        resultText.rectTransform.localPosition = Vector3.right * 1000;
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }

        resultText.text = text;
        currentTween = resultText.rectTransform.DOLocalMoveX(0, 0.5f).SetEase(Ease.OutCubic);
    }
}
