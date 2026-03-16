using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private GameData gameData;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private CardManager cardManager;
    [SerializeField] private TimerManager timerManager;
    [SerializeField] private CenterTextManager centerTextManager;
    private int timer = 100;

    [SerializeField] private GameObject infoText;
    //private bool gameStarted = false;
    private bool gameEnded = false;

    private PlayerInputActions inputActions;

    void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.PostGame.Retry.performed += _ => Retry();
        inputActions.PostGame.BackToTitle.performed += _ => BackToTitle();
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        gameData = GameSettings.Instance.currentGameData;
        timer = gameData.timeLimit;
        timerManager.SetupTimer(timer);
        infoText.SetActive(false);
        StartCoroutine(StartGame());
    }

    void Update()
    {
        
    }

    void OnDestroy()
    {
        inputActions.Dispose();
    }

    private IEnumerator StartGame()
    {
        // 3
        cardManager.SetupCards(gameData.cardNum);
        centerTextManager.ShowText("3");
        yield return new WaitForSeconds(1f);
        // 2
        //プレイヤー, CPU出現
        playerManager.SpawnPlayer(gameData.CPUNum, gameData.CPUAccuracy);
        centerTextManager.ShowText("2");
        yield return new WaitForSeconds(1f);
        // 1
        centerTextManager.ShowText("1");
        yield return new WaitForSeconds(1f);
        // start!
        centerTextManager.ShowText("Start!");
        //gameStarted = true;
        //プレイヤー入力受け付け,タイマー開始
        playerManager.AllowPlayerInput();
        yield return StartCoroutine(timerManager.UpdateTimer(timer));
        playerManager.DisallowPlayerInput();
        centerTextManager.ShowText("Finish!");
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(JudgeGame());
        yield return new WaitForSeconds(1f);
        //リトライorタイトルへ戻るの選択表示
        infoText.SetActive(true);
        inputActions.PostGame.Enable();
        gameEnded = true;
    }

    private IEnumerator JudgeGame()
    {
        // 勝敗判定
        yield return StartCoroutine(cardManager.GetFaceUpCardCount());
        yield return new WaitForSeconds(0.7f);
        // 結果表示
        if(cardManager.faceUpCount >= gameData.threshold)
        {
            centerTextManager.ShowResultText("You Win!");
        }
        else
        {
            centerTextManager.ShowResultText("You Lose...");
        }
    }

    private void Retry()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void BackToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
