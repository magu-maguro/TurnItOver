using DG.Tweening;
using UnityEngine;

public class CpuController : MonoBehaviour
{
    PlayerMovement movement;
    private bool canMove = true;
    public float accuracy = 0.5f;
    private float nextJumpTime;

    [SerializeField] private CardManager cardManager;
    [SerializeField] private float optimalDistance = 1.5f;
    [SerializeField] private float minJumpCooldown = 0.3f;
    [SerializeField] private float maxJumpCooldown = 1.2f;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();

        cardManager = FindFirstObjectByType<CardManager>();

        InvokeRepeating(nameof(RandomMove), 0, 1);
    }

    void RandomMove()
    {
        if (cardManager == null)
        {
            movement.SetMoveInput(Vector2.zero);
            return;
        }

        CardController targetCard = GetNearestFaceUpCard();
        Vector2 optimalDir = GetOptimalDirection(targetCard);

        Vector2 dir = GetDirectionWithAccuracy(optimalDir);
        if (!canMove)
        {
            dir = Vector2.zero;
        }

        movement.SetMoveInput(dir);
    }

    void Update()
    {
        if (!canMove || cardManager == null)
        {
            return;
        }

        if (Time.time < nextJumpTime)
        {
            return;
        }

        CardController targetCard = GetNearestFaceUpCard();
        float normalizedAccuracy = Mathf.Clamp01(accuracy);
        float jumpDistanceThreshold = Mathf.Lerp(optimalDistance * 0.5f, optimalDistance, normalizedAccuracy);
        if (GetDistanceToCard(targetCard) < jumpDistanceThreshold && (float)Random.Range(0, 1) < normalizedAccuracy)
        {
            movement.TryJumpDrop();
            nextJumpTime = Time.time + GetJumpCooldown(normalizedAccuracy);
        }
    }

    private Vector2 GetDirectionWithAccuracy(Vector2 optimalDir)
    {
        if (optimalDir == Vector2.zero)
        {
            return Vector2.zero;
        }

        float normalizedAccuracy = Mathf.Clamp01(accuracy);
        float maxOffsetAngle = (1f - normalizedAccuracy) * 90f;
        float offsetAngle = Random.Range(-maxOffsetAngle, maxOffsetAngle);
        Vector3 rotated = Quaternion.Euler(0f, offsetAngle, 0f) * new Vector3(optimalDir.x, 0f, optimalDir.y);
        return new Vector2(rotated.x, rotated.z);
    }

    private Vector2 GetOptimalDirection(CardController targetCard)
    {
        if (targetCard == null)
        {
            return Vector2.zero;
        }

        Vector3 dirToCard = (targetCard.transform.position - transform.position).normalized;
        return new Vector2(dirToCard.x, dirToCard.z);
    }

    private CardController GetNearestFaceUpCard()
    {
        float minDistance = float.MaxValue;
        CardController nearestCard = null;
        Vector2 myPosition = new Vector2(transform.position.x, transform.position.z);

        foreach (var card in cardManager.GetComponentsInChildren<CardController>())
        {
            if (card.isFaceUp)
            {
                Vector2 cardPosition = new Vector2(card.transform.position.x, card.transform.position.z);
                float distance = Vector2.Distance(myPosition, cardPosition);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestCard = card;
                }
            }
        }
        return nearestCard;
    }

    private float GetDistanceToCard(CardController targetCard)
    {
        if (targetCard == null)
        {
            return float.MaxValue;
        }

        Vector2 myPosition = new Vector2(transform.position.x, transform.position.z);
        Vector2 targetPosition = new Vector2(targetCard.transform.position.x, targetCard.transform.position.z);
        return Vector2.Distance(myPosition, targetPosition);
    }

    private float GetJumpCooldown(float normalizedAccuracy)
    {
        float clampedMin = Mathf.Max(0f, minJumpCooldown);
        float clampedMax = Mathf.Max(clampedMin, maxJumpCooldown);
        return Mathf.Lerp(clampedMax, clampedMin, normalizedAccuracy);
    }

    //-----------api
    public void DisallowInput()
    {
        canMove = false;
    }
    public void AllowInput()
    {
        canMove = true;
    }
}