using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//[RequireComponent(typeof(Collider2D))]
public class GameStartButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameData gameData;
    public void OnStartButtonClicked()
    {
        GameSettings.Instance.currentGameData = gameData;
        StartCoroutine(SoundManager.PlaySE(1));
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    public void PlaySE()
    {
        StartCoroutine(SoundManager.PlaySE(1));
    }

    //マウスホバー時に少し拡大
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * 1.1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }
}
