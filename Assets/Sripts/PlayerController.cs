using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Configuration")]
    [SerializeField] private int controlledPlayerID = 0;
    [SerializeField] private CardDealer cardDealer;

    [Header("Interaction Settings")]
    [SerializeField] private float cardSpacingX = 1.5f;
    [SerializeField] private float cardSpacingZ = 0.001f;
    [SerializeField] private LayerMask cardLayer;

    [Header("Card Movement")]
    [SerializeField] private float raiseHeight = 0.000001f;
    [SerializeField] private float moveDuration = 0.2f;

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
                    //PlayCard(selectedCard);
                    TogglePosition();
                }
            }
        }
    }

    public void PlayCard(Card card)
    {
        controlledPlayer.RemoveCarta(card);
        cardDealer.SubmitPlay(card);
        ArrangeCards();
    }
    public void SelectCard()
    {

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

    //private void UpdateSelectedCards(Card card)
    //{
    //    if (selectedCard.isRaised)
    //    {
    //        if (cardsToPlay.Count >= 3)
    //        {
    //            Debug.Log("list is full");
    //            cardsToPlay.RemoveAt(0);
    //            selectedCard = cardsToPlay[2];
    //            TogglePosition();
    //            cardsToPlay.Add(card);
    //        }
    //        cardsToPlay.Add(card);
    //    }
    //    else
    //    {
    //        Debug.Log("removing card");
    //        cardsToPlay.Remove(card);
    //    }

    //    Debug.Log($"Current selected cards: {cardsToPlay.Count}");
    //}
    public void TogglePosition()
    {
        Vector3 targetPosition;

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

                targetPosition = cardsToPlay[0].initialLocalPosition + Vector3.back * raiseHeight;
                StartCoroutine(MoveCard(targetPosition));
                cardsToPlay[0].isRaised = false;
                cardsToPlay.Remove(cardsToPlay[0]);

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
        Debug.Log($"Current selected cards: {cardsToPlay.Count}");
        Debug.Log("toggling card");
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
        yield return new WaitForSeconds(0.1f);
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
}
