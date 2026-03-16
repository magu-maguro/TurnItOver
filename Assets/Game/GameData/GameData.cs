using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Scriptable Objects/GameData")]
public class GameData : ScriptableObject
{
    public int timeLimit = 20;
    public int cardNum = 20;
    public int threshold = 10;
    [Header("CPU Settings")]
    public int CPUNum = 1;
    public float CPUAccuracy = 0.5f;
}
