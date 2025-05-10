using System.Collections.Generic;
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

    public Player controlledPlayer;
    public Camera mainCamera;
    void Awake()
    {
        mainCamera = Camera.main;
        InitializeController();
        ArrangeCards();
    }

    void Update()
    {
        PlayerInput();
        ArrangeCards();
    }

    private void InitializeController()
    {
        controlledPlayer = cardDealer.CurrentGamePlayers.Find(p => p.playerID == controlledPlayerID);
        ArrangeCards();
    }

    private void PlayerInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, cardLayer))
            {
                Card selectedCard = hit.collider.GetComponent<Card>();
                if (selectedCard != null && controlledPlayer.playerHand.Contains(selectedCard))
                {
                    PlayCard(selectedCard);
                    ArrangeCards();
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

    private void ArrangeCards()
    {
        for (int i = 0; i < controlledPlayer.playerHand.Count; i++)
        {
            float xPos = i * cardSpacingX;
            float ZPos = i * cardSpacingZ;
            controlledPlayer.playerHand[i].transform.localPosition = new Vector3(xPos, ZPos, 0);
            controlledPlayer.playerHand[i].SetInteractable(controlledPlayer.isHumanPlayer);
        }
    }

    private List<Card> GetPlayableCards()
    {
        List<Card> playable = new List<Card>();
        int currentNumber = cardDealer.GetCurrentDeclaredNumber();
        string currentSuit = cardDealer.GetCurrentDeclaredSuit();

        foreach (Card card in controlledPlayer.playerHand)
        {
            if (card.cardNumber == currentNumber || card.cardSuits == currentSuit)
            {
                playable.Add(card);
            }
        }
        return playable;
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
}
