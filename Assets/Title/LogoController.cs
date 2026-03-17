using UnityEngine;
using DG.Tweening;

public class LogoController : MonoBehaviour
{
    Tween tween;
    [SerializeField] float amplitude = 0.5f;
    [SerializeField] float duration = 1f;
    void Start()
    {
        tween = transform.DOLocalMoveY(transform.localPosition.y + amplitude, duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    void OnDisable()
    {
        tween.Kill();
    }
}
