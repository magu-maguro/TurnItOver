using UnityEngine;

public class TitleManager : MonoBehaviour
{
    //インスペクタにデータ削除ボタン追加
    [ContextMenu("Delete Save Data")]
    private void DeleteSaveData()
    {
        PlayerPrefs.DeleteKey("BestScore");
        Debug.Log("Save data deleted.");
    }
}
