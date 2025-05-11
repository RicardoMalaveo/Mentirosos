using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using static DeckInfo;

public class ManoloAI : MonoBehaviour
{
    //Estas variables permiten calcular si se quiere tomar una decision o no, "1" es 100% ¡SI! y "0" es 100% ¡NO!
    private float manoloRisk = 0.5f; // a este numero se suma o se resta, este numero representa un rango de 100%, siempre empieza en 50% "neutral".

    [Header("Player Info")]
    [SerializeField] private int controlledPlayerID = 0;
    [SerializeField] private CardDealer cardDealer;

    [Header("Card Spacing")]
    [SerializeField] private float cardSpacingX = 1.5f;
    [SerializeField] private float cardSpacingZ = 0.001f;

    public Player controlledPlayer;
    //List<Card> playableCards;
    void Awake()
    {
        InitializeController();
    }

    void Update()
    {
        ManoloBehavior();
        ArrangeCards();
    }

    private void InitializeController()
    {
        controlledPlayer = cardDealer.CurrentGamePlayers.Find(player => player.playerID == controlledPlayerID);
        ArrangeCards();
    }

    private void ManoloBehavior()
    {
        if (cardDealer.IsAITurn(controlledPlayerID))
        {
            //List<Card> playableCards = GetPlayableCards();
            if (controlledPlayer.playerHand.Count > 0)
            {
                Card aiChoice = controlledPlayer.playerHand[Random.Range(0, controlledPlayer.playerHand.Count)];
                PlayCard(aiChoice);
            }
            else
            {
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
        OrganizeCards();
        for (int i = 0; i < controlledPlayer.playerHand.Count; i++)
        {
            float xPos = i * cardSpacingX;
            float ZPos = i * cardSpacingZ;
            controlledPlayer.playerHand[i].transform.localPosition = new Vector3(xPos, ZPos, 0);
        }
    }

    //private List<Card> GetPlayableCards()
    //{
    //    List<Card> playableCards = new List<Card>();
    //    int currentNumber = cardDealer.GetCurrentDeclaredNumber();

    //    foreach (Card card in controlledPlayer.playerHand)
    //    {
    //        if (card.cardNumber == currentNumber + 1 || card.cardNumber == currentNumber)
    //        {
    //            playableCards.Add(card);
    //        }
    //    }
    //    return playableCards;
    //}
    void OrganizeCards()
    {
        // Calculate frequency of each card number
        Dictionary<int, int> numberFrequency = controlledPlayer.playerHand
            .GroupBy(card => card.cardNumber)
            .ToDictionary(group => group.Key, group => group.Count());

        // Sort the list by frequency (descending) and then by cardNumber (ascending)
        controlledPlayer.playerHand = controlledPlayer.playerHand
            .OrderByDescending(card => numberFrequency[card.cardNumber])
            .ThenBy(card => card.cardNumber)
            .ToList();
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
