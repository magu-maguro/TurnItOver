using UnityEngine;

public class GameStartButtonController : MonoBehaviour
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
}
