using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    private List<CardController> cards = new List<CardController>();
    public CardController cardPrefab;
    [SerializeField] private Vector2 spawnAreaSize = new Vector2(32f, 16f);
    [SerializeField] private float cardHeight = 7f;
    [SerializeField, Range(0f, 0.45f)] private float positionJitter = 0.35f;

    void Start()
    {
        SetupCards(20); // 20枚のカードを生成
    }

    public void SetupCards(int numberOfCards)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            bool isFaceUp = Random.value > 0.5f; // ランダムに表か裏を決める
            CardController newCard = Instantiate(cardPrefab, DecideRandomPos(i, numberOfCards), DecideRandomRotation(isFaceUp));
            cards.Add(newCard);
            newCard.transform.parent = this.transform;
            newCard.isFaceUp = isFaceUp;
        }
    }


    //========================
    // カードの初期位置をランダムに決める
    private Vector3 DecideRandomPos(int index, int totalCards)
    {
        float width = spawnAreaSize.x;
        float depth = spawnAreaSize.y;
        float areaAspect = width / depth;

        int columns = Mathf.CeilToInt(Mathf.Sqrt(totalCards * areaAspect));
        int rows = Mathf.CeilToInt(totalCards / (float)columns);

        float cellWidth = width / columns;
        float cellDepth = depth / rows;
        int column = index % columns;
        int row = index / columns;

        float xOffset = Random.Range(-cellWidth * positionJitter, cellWidth * positionJitter);
        float zOffset = Random.Range(-cellDepth * positionJitter, cellDepth * positionJitter);
        float x = -width * 0.5f + cellWidth * (column + 0.5f) + xOffset;
        float y = cardHeight + Random.Range(-1f, 1f);
        float z = -depth * 0.5f + cellDepth * (row + 0.5f) + zOffset;

        return new Vector3(x, y, z);
    }
    //カードの初期の向きをランダムに決める
    private Quaternion DecideRandomRotation(bool isFaceUp)
    {
        float yRotation = Random.Range(0f, 360f);
        float xRotation = isFaceUp ? 0f : 180f; // 表なら0度、裏なら180度
        return Quaternion.Euler(xRotation, yRotation, 0);
    }
}
