using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using static DeckInfo;

public class CardDealer : MonoBehaviour
{
    [Header("Game State Info")]
    [SerializeField] public bool IsFirstTurn; //indica si es el primer turno

    [Header("Players")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Player playerInfo;
    [SerializeField] public int CurrentPlayer;
    [SerializeField] public int lastPlayer;
    [SerializeField] private bool didLastPlayerLied;

    [Header("Deck")]
    public DeckInfo deckInfo;
    public Transform mainPile;
    public Transform discardedPile;


    [Header("Game Pile")]
    [SerializeField] public Card actualPlayedCard;
    [SerializeField] public int cardDeclared;
    [SerializeField] private Card CurrentCard;
    [SerializeField] private int amountOfCardsPlayed;
    [SerializeField] private int totalAmountOfCardsInThePile;
    [SerializeField] private List<Card> CurrentGamePile = new List<Card>(); //Lista de cartas en la pila del juego actual.

    [Header("Player Configuration")]
    public int playerCount = 3;
    public Transform[] playerHands;

    [Header("Player List")]
    public List<Player> CurrentGamePlayers = new List<Player>();
    private List<GameObject> instantiatedDeck = new List<GameObject>(); //Lista de cartas instanciadas en escena, estar� vacia hasta que se cree el deck
    [SerializeField] private List<Card> DiscardedGamePile = new List<Card>(); //Lista de cartas descartadas definitivamente en el juego.




    void Start()
    {
        IsFirstTurn = true;
        CreateAndShuffleDeck();
        DealCards();
    }




    public void AddCardsToCurrentGamePile(Card cardsToPlay) //envia cartas a la pila de juego
    {
        CurrentGamePile.Add(cardsToPlay);
        cardsToPlay.transform.SetParent(mainPile);
        cardsToPlay.transform.localPosition = Vector3.zero;
        cardsToPlay.transform.localRotation = Quaternion.identity;
        actualPlayedCard = CurrentGamePile.First();

        if(cardDeclared == 0 && IsFirstTurn)
        {
            cardDeclared = actualPlayedCard.cardNumber;
        }
    }





    public void PlayerTurnControl()        //se decide de quien es el turno
    {
        if (IsFirstTurn)
        {
            CurrentPlayer = 1;
            IsFirstTurn = false;
        }
        else if (CurrentPlayer >= CurrentGamePlayers.Count - 1)
        {
            CurrentPlayer = 0;
            lastPlayer = 2;
        }
        else
        {
            CurrentPlayer++;
            lastPlayer = CurrentPlayer - 1;
        }
        GetCurrentGamePileAmounts();
    }



    public void GetGamePileToLiar(int playerId) //se ejecuta cuando alguien acusa, envia las cartas dependiendo de quien haya acusado o si ha mentido
    {
        if (playerId != lastPlayer)
        {
            if (didLastPlayerLied)
            {
                CurrentGamePlayers[lastPlayer].playerHand.AddRange(CurrentGamePile);
                for (int i = 0; i < CurrentGamePile.Count; i++)
                {
                    CurrentGamePile[i].transform.SetParent(playerHands[lastPlayer]);
                    CurrentGamePile[i].transform.localPosition = Vector3.zero;
                    CurrentGamePile[i].transform.localRotation = Quaternion.identity;
                    CurrentGamePile[i].isRaised = false;
                }
            }
            else
            {
                CurrentGamePlayers[playerId].playerHand.AddRange(CurrentGamePile);
                for (int i = 0; i < CurrentGamePile.Count; i++)
                {
                    CurrentGamePile[i].transform.SetParent(playerHands[playerId]);
                    CurrentGamePile[i].transform.localPosition = Vector3.zero;
                    CurrentGamePile[i].transform.localRotation = Quaternion.identity;
                    CurrentGamePile[i].isRaised = false;
                }
            }
            if (lastPlayer == 0 || playerId == 0)
            {
                playerController.ArrangeCards();
            }

            CurrentGamePile.Clear();
            ResetTable(playerId);
        }
    }





    public void AddToDiscardedGamePile(List<Card> discardedCards) //envia 4 cartas seleccionadas con el mismo numero a la pila descartada.
    {
        if (discardedCards.Count > 3)
        {
            bool validDiscardedGroup = false;
            for (int i = 0; i < discardedCards.Count; i++)
            {
                if (discardedCards[0].cardNumber == discardedCards[i].cardNumber)
                {
                    validDiscardedGroup = true;
                }
                else
                {
                    validDiscardedGroup = false;
                    Debug.Log("Not all cards are the same");
                }
            }

            if (validDiscardedGroup)
            {
                DiscardedGamePile.AddRange(discardedCards);
                for (int i = 0; i < DiscardedGamePile.Count; i++)
                {
                    DiscardedGamePile[i].transform.SetParent(discardedPile);
                    DiscardedGamePile[i].transform.localPosition = Vector3.zero;
                    DiscardedGamePile[i].transform.localRotation = Quaternion.identity;
                }

                for (int x = playerController.cardsToPlay.Count - 1; x >= 0; x--)
                {
                    CurrentGamePlayers[CurrentPlayer].RemoveCard(playerController.cardsToPlay[x]);
                }
                Debug.Log("cards added to the discarded pile");
            }
        }
        else
        {
            Debug.Log("Not enough cards in hand");
        }
        playerController.ArrangeCards();
    }



    void ResetTable(int playerId) // cambia los valores de la ronda para que el juego pueda iniciar nuevamente luego de que alguien haya sido acusado.
    {
        actualPlayedCard = null;
        cardDeclared = 0;
        CurrentCard = null;
        amountOfCardsPlayed = 0;
        totalAmountOfCardsInThePile = 0;

        if (didLastPlayerLied)
        {
            CurrentPlayer = lastPlayer;
        }
        else
        {
            CurrentPlayer = playerId;
        }
        didLastPlayerLied = false;
        IsFirstTurn = true;
    }


    //Crea visualmente el mazo de cartas a partir de los prefabs definidos
    //Mezcla el mazo instanciado usando el algoritmo Fisher�Yates
    void CreateAndShuffleDeck()
    {
        foreach (var cardInfo in deckInfo.allCards)
        {
            GameObject cardInstance = Instantiate(cardInfo.prefab);
            cardInstance.name = cardInfo.cardName;
            instantiatedDeck.Add(cardInstance);
        }

        for (int i = 0; i < instantiatedDeck.Count; i++)
        {
            int rand = Random.Range(i, instantiatedDeck.Count);
            GameObject temp = instantiatedDeck[i];
            instantiatedDeck[i] = instantiatedDeck[rand];
            instantiatedDeck[rand] = temp;
        }
    }





    void DealCards()  //Reparte las cartas de forma equitativa entre los jugadores
    {
        for (int i = 0; i < instantiatedDeck.Count; i++)
        {
            int currentPlayer = i % playerCount;
            GameObject card = instantiatedDeck[i];
            card.transform.SetParent(playerHands[currentPlayer]);
            Card cardComponent = card.GetComponent<Card>();
            card.transform.localPosition = Vector3.zero;
            card.transform.localRotation = Quaternion.identity;
            CurrentGamePlayers[currentPlayer].AddCard(cardComponent);
        }
    }






    void GetCurrentGamePileAmounts() //indica el total de cartas en la pila en juego, indica cuantas cartas jugo el ultimo jugador, indica si el ultimo jugador a mentido
    {
        if (IsFirstTurn)
        {
            totalAmountOfCardsInThePile = CurrentGamePile.Count;
            amountOfCardsPlayed = totalAmountOfCardsInThePile;
        }
        else
        {
            amountOfCardsPlayed = CurrentGamePile.Count - totalAmountOfCardsInThePile;
            totalAmountOfCardsInThePile = CurrentGamePile.Count;
        }

        for (int i = totalAmountOfCardsInThePile - amountOfCardsPlayed; i < CurrentGamePile.Count; i++)
        {
            CurrentCard = CurrentGamePile[i];
            if (actualPlayedCard.cardNumber != CurrentCard.cardNumber || cardDeclared != 0)
            {
                if (cardDeclared != CurrentCard.cardNumber)
                {
                    didLastPlayerLied = true;
                    i = CurrentGamePile.Count - 1;
                }
                else
                {
                    didLastPlayerLied = false;
                }
            }
            else
            {
                didLastPlayerLied = false;
            }
            Debug.Log(CurrentCard);
        }
    }
}
