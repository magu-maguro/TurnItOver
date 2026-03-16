using UnityEngine;

public class GameStartButtonController : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    public void OnStartButtonClicked()
    {
        GameSettings.Instance.currentGameData = gameData;
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}
