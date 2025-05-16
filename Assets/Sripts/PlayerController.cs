using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Configuration")]
    [SerializeField] private CardDealer cardDealer;

    [Header("Card Spacing")]
    [SerializeField] private float cardSpacingY = 0.05f;
    [SerializeField] private Vector3 direction = new Vector3(0, 0, 0);
    [SerializeField] private LayerMask cardLayer;

    [Header("Card Selection")]
    [SerializeField] private float cardRaise = 0.015f;
    [SerializeField] private float moveDuration = 0.1f;

    [Header("Pile")]
    public Transform mainPile;
    public Vector3 mainPilePosition;
    public Vector3 cardStackOffSet = new Vector3(0.1f, 0.1f, -0.05f);
    public float pileMoveDuration = 0.3f;

    [Header("Hand")]
    [SerializeField] public List<Card> cardsToPlay;

    public Player controlledPlayer;
    public Card selectedCard;
    public Card autoSelectedCard;
    public Camera mainCamera;




    void Awake()
    {
        mainCamera = Camera.main;
    }


    public void Start()
    {
        InitializeController();
    }


    void Update()
    {
        PlayerInput();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaySelectedCards();
            cardDealer.PlayerTurnControl();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            DiscardSelectCards();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            cardDealer.GetGamePileToLiar(controlledPlayer.playerID);
        }
            
    }



    private void InitializeController()
    {
        controlledPlayer = cardDealer.CurrentGamePlayers.Find(player => player.playerID == controlledPlayer.playerID);
        ArrangeCards();
    }




    public void PlayerInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, cardLayer))
            {
                selectedCard = hit.collider.GetComponent<Card>();
                if (selectedCard != null && controlledPlayer.playerHand.Contains(selectedCard))
                {
                    TogglePosition();
                }
            }
        }
    }


    public void AddCardToHand(Card card)
    {
        controlledPlayer.AddCard(card);
        card.transform.SetParent(transform);
        ArrangeCards();
    }





    void DiscardSelectCards()
    {
        cardDealer.AddToDiscardedGamePile(cardsToPlay);
        for (int i = cardsToPlay.Count - 1; i >= 0; i--)
        {
            cardsToPlay[i].isRaised = false;
            cardsToPlay.RemoveAt(i);
        }
    }


    void PlaySelectedCards()
    {
        for (int i = cardsToPlay.Count - 1; i >= 0; i--)
        {
            cardDealer.AddCardsToCurrentGamePile(cardsToPlay[i]);
            controlledPlayer.RemoveCard(cardsToPlay[i]);
            cardsToPlay.RemoveAt(i);
        }
        ArrangeCards();
    }



    public void TogglePosition()
    {
        Vector3 targetPosition;
        Vector3 automaticTargetPosition;

        if (selectedCard.isRaised)
        {
            cardsToPlay.Remove(selectedCard);
            targetPosition = selectedCard.initialLocalPosition;
            selectedCard.isRaised = false;
            StartCoroutine(MoveCard(targetPosition));
        }
        else
        {
            if (cardsToPlay.Count >= 4)
            {
                autoSelectedCard = cardsToPlay[0];
                automaticTargetPosition = autoSelectedCard.initialLocalPosition;
                StartCoroutine(AutomaticMoveCard(automaticTargetPosition));
                autoSelectedCard.isRaised = false;

                cardsToPlay.Add(selectedCard);
                selectedCard.isRaised = true;
                targetPosition = selectedCard.initialLocalPosition + Vector3.back * cardRaise;
                StartCoroutine(MoveCard(targetPosition));
            }
            else
            {
                cardsToPlay.Add(selectedCard);
                targetPosition = selectedCard.initialLocalPosition + Vector3.back * cardRaise;
                selectedCard.isRaised = true;
                StartCoroutine(MoveCard(targetPosition));
            }
        }
    }





    public void ArrangeCards()
    {
        Dictionary<int, int> numberFrequency = controlledPlayer.playerHand.GroupBy(card => card.cardNumber).ToDictionary(group => group.Key, group => group.Count());
        controlledPlayer.playerHand = controlledPlayer.playerHand.OrderBy(card => numberFrequency[card.cardNumber]).ThenBy(card => card.cardNumber).ToList();

        float totalSpread = (controlledPlayer.playerHand.Count - 1) * cardSpacingY;
        float startOffset = -totalSpread / 2f;

        for (int i = 0; i < controlledPlayer.playerHand.Count; i++)
        {
            if (controlledPlayer.playerHand[i] == null) continue;

            float positionOffset = startOffset + i * cardSpacingY;
            Vector3 newPosition = cardDealer.playerHands[controlledPlayer.playerID].localPosition + direction * positionOffset;
            controlledPlayer.playerHand[i].transform.position = newPosition;
            controlledPlayer.playerHand[i].transform.localRotation = Quaternion.identity;
            controlledPlayer.playerHand[i].SetInteractable(controlledPlayer.isHumanPlayer);
            controlledPlayer.playerHand[i].isRaised = false;
            controlledPlayer.playerHand[i].UpdateLocalPosition();
            cardsToPlay.Clear();
        }
    }




    private IEnumerator MoveCard(Vector3 targetPosition)
    {
        float timeElapsed = 0f;
        Vector3 startPosition = selectedCard.transform.localPosition;
        while (timeElapsed < moveDuration)
        {
            selectedCard.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, timeElapsed / moveDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        selectedCard.transform.localPosition = targetPosition;
    }





    private IEnumerator AutomaticMoveCard(Vector3 automaticTargetPosition)
    {
        float timeElapsed = 0f;
        Vector3 startPosition = autoSelectedCard.transform.localPosition;
        while (timeElapsed < moveDuration)
        {
            autoSelectedCard.transform.localPosition = Vector3.Lerp(startPosition, automaticTargetPosition, timeElapsed / moveDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        cardsToPlay.Remove(autoSelectedCard);
        autoSelectedCard.transform.localPosition = automaticTargetPosition;
    }
}
