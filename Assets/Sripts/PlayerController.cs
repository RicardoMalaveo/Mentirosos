using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Configuration")]
    [SerializeField] private int controlledPlayerID = 0;
    [SerializeField] private CardDealer cardDealer;

    [Header("Card Spacing")]
    [SerializeField] private float cardSpacingX = 1.5f;
    [SerializeField] private float cardSpacingZ = 0.001f;
    [SerializeField] private LayerMask cardLayer;

    [Header("Card Selection")]
    [SerializeField] private float raiseHeight = 0.000001f;
    [SerializeField] private float moveDuration = 0.2f;

    [Header("Pile")]
    public Transform mainPile;
    public Vector3 mainPilePosition;
    public Vector3 cardStackOffSet = new Vector3(0.1f, 0.1f, -0.05f);
    public float pileMoveDuration = 0.3f;

    public List<Card> cardsToPlay;

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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PlaySelectedCards();
        }
    }

    private void InitializeController()
    {
        controlledPlayer = cardDealer.CurrentGamePlayers.Find(player => player.playerID == controlledPlayerID);
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
    void PlaySelectedCards()
    {
        for (int i = cardsToPlay.Count - 1; i >= 0; i--)
        {
            Debug.Log(cardsToPlay.Count +" Cards to play");
            cardDealer.AddToCurrentGamePile(cardsToPlay[i]);
            controlledPlayer.playerHand.Remove(cardsToPlay[i]);
            cardsToPlay.RemoveAt(i);
            Debug.Log("Remove Cards from hand");
            Debug.Log(i);
        }
    }
    public void PlayCard(Card card)
    {
        controlledPlayer.RemoveCarta(card);
        ArrangeCards();
    }
    public void AddCardToHand(Card card)
    {
        controlledPlayer.AddCarta(card);
        card.transform.SetParent(transform);
        ArrangeCards();
    }

    public void RemoveCardFromHand(Card card)
    {
        controlledPlayer.RemoveCarta(card);
        ArrangeCards();
    }
    public void TogglePosition()
    {
        Vector3 targetPosition;
        Vector3 automaticTargetPosition;

        if ( selectedCard.isRaised)
        {
            cardsToPlay.Remove(selectedCard);
            targetPosition = selectedCard.initialLocalPosition;
            selectedCard.isRaised = false;
            StartCoroutine(MoveCard(targetPosition));
        } 
        else
        {
            if (cardsToPlay.Count >= 3)
            {
                autoSelectedCard = cardsToPlay[0];
                automaticTargetPosition = autoSelectedCard.initialLocalPosition;
                StartCoroutine(AutomaticMoveCard(automaticTargetPosition));
                autoSelectedCard.isRaised = false;

                cardsToPlay.Add(selectedCard);
                selectedCard.isRaised = true;
                targetPosition = selectedCard.initialLocalPosition + Vector3.back * raiseHeight;
                StartCoroutine(MoveCard(targetPosition));
            }
            else
            {
                cardsToPlay.Add(selectedCard);
                targetPosition = selectedCard.initialLocalPosition + Vector3.back * raiseHeight;
                selectedCard.isRaised = true;
                StartCoroutine(MoveCard(targetPosition));
            }
        }
    }
    private void ArrangeCards()
    {
        Dictionary<int, int> numberFrequency = controlledPlayer.playerHand.GroupBy(card => card.cardNumber).ToDictionary(group => group.Key, group => group.Count());
        controlledPlayer.playerHand = controlledPlayer.playerHand.OrderBy(card => numberFrequency[card.cardNumber]).ThenBy(card => card.cardNumber).ToList();
        for (int i = 0; i < controlledPlayer.playerHand.Count; i++)
        {
            float xPos = i * cardSpacingX;
            float ZPos = i * cardSpacingZ;
            controlledPlayer.playerHand[i].transform.localPosition = new Vector3(xPos, ZPos, 0);
            controlledPlayer.playerHand[i].SetInteractable(controlledPlayer.isHumanPlayer);
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
