using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private GameData gameData;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private CardManager cardManager;
    [SerializeField] private TimerManager timerManager;
    private int timer = 100;


    private bool gameStarted = false;
    void Start()
    {
        Application.targetFrameRate = 60;
        gameData = GameSettings.Instance.currentGameData;
        timer = gameData.timeLimit;
        timerManager.SetupTimer(timer);
        StartCoroutine(StartGame());
    }

    void Update()
    {
        
    }

    private IEnumerator StartGame()
    {
        // 3
        cardManager.SetupCards(gameData.cardNum);
        yield return new WaitForSeconds(1f);
        // 2
        //プレイヤー, CPU出現
        playerManager.SpawnPlayer(gameData.CPUNum);
        yield return new WaitForSeconds(1f);
        // 1
        yield return new WaitForSeconds(1f);
        // start!
        gameStarted = true;
        //プレイヤー入力受け付け,タイマー開始
        playerManager.AllowPlayerInput();
        yield return StartCoroutine(timerManager.UpdateTimer(timer));
        playerManager.DisallowPlayerInput();
        yield return StartCoroutine(JudgeGame());
        //リトライorタイトルへ戻るの選択表示
    }

    private IEnumerator JudgeGame()
    {
        // 勝敗判定
        yield return new WaitForSeconds(1f);
        // 結果表示
        yield return new WaitForSeconds(1f);
    }
}
