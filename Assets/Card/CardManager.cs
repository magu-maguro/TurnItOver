using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CardManager : MonoBehaviour
{
    private List<CardController> cards = new List<CardController>();
    public CardController cardPrefab;
    [SerializeField] private Vector2 spawnAreaSize = new Vector2(32f, 16f);
    [SerializeField] private float cardHeight = 7f;
    [SerializeField, Range(0f, 0.45f)] private float positionJitter = 0.35f;

    [SerializeField] private TextMeshProUGUI faceUpCountText;
    [SerializeField] private TextMeshProUGUI faceDownCountText;
    public int faceUpCount { get; private set; }

    [Header("Effect")]
    [SerializeField] private ParticleSystem flipEffectPrefab;
    private List<ParticleSystem> flipEffectInstances = new List<ParticleSystem>();// object pool的な
    private int initialEffectPoolSize = 5;

    void Start()
    {
        faceUpCountText.gameObject.SetActive(false);
        faceDownCountText.gameObject.SetActive(false);

        // エフェクトのオブジェクトプールを初期化
        for (int i = 0; i < initialEffectPoolSize; i++)
        {
            ParticleSystem effect = Instantiate(flipEffectPrefab, transform);
            effect.Stop();
            flipEffectInstances.Add(effect);
        }
    }

    public void SetupCards(int numberOfCards)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            bool isFaceUp = Random.value > 0.5f; // ランダムに表か裏を決める
            CardController newCard = Instantiate(cardPrefab, DecideRandomPos(i, numberOfCards), DecideRandomRotation(isFaceUp));
            cards.Add(newCard);
            // flipを購読
            newCard.onFlip.AddListener(() => PlayFlipEffect(newCard.transform.position));
            newCard.transform.parent = this.transform;
            newCard.isFaceUp = isFaceUp;
        }
    }

    public IEnumerator GetFaceUpCardCount()
    {
        faceUpCountText.gameObject.SetActive(true);
        faceDownCountText.gameObject.SetActive(true);
        faceUpCount = 0;
        int faceDownCount = 0;
        foreach (var card in cards)
        {
            card.FloatCard();
            if (card.isFaceUp)
            {
                faceUpCount++;
                faceUpCountText.text = "You: "+faceUpCount;
                faceUpCountText.transform.DOScale(1.2f, 0.1f).OnComplete(() =>
                {
                    faceUpCountText.transform.DOScale(1f, 0.1f);
                });
            }
            else
            {
                faceDownCount++;
                faceDownCountText.text = "Enemy: " + faceDownCount;
                faceDownCountText.transform.DOScale(1.2f, 0.1f).OnComplete(() =>
                {
                    faceDownCountText.transform.DOScale(1f, 0.1f);
                });
            }
            yield return new WaitForSeconds(0.2f);
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

    //===========================================

    private void PlayFlipEffect(Vector3 position)
    {
        position.y -= 2f;
        // オブジェクトプールから使用可能なエフェクトを探す
        ParticleSystem effect = flipEffectInstances.Find(e => !e.isPlaying);
        if (effect != null)
        {
            effect.transform.position = position;
            effect.Play();
            // 終わったら止める
        }
        else
        {
            // もし全てのエフェクトが使用中なら、新たにインスタンス化する（必要に応じて）
            effect = Instantiate(flipEffectPrefab, position, Quaternion.identity, transform);
            effect.Play();
            flipEffectInstances.Add(effect);
        }
    }
}
