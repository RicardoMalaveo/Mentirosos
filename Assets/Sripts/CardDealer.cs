using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using static DeckInfo;
using System.ComponentModel;
using System.Collections;

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
    [SerializeField] public bool aPlayerRanOutOfCards;
    [SerializeField] public bool someoneGotAccused = false;
    [SerializeField] public bool gamePileEmpty = true;
    [SerializeField] public bool LastPlayerCanBeAccused = false;
    [SerializeField] public int roundsWithAPlayerWithNoCards;

    [Header("Deck")]
    public DeckInfo deckInfo;
    public Transform mainPile;
    public Transform discardedPile;


    [Header("Game Pile")]
    [SerializeField] public Card actualPlayedCard;
    [SerializeField] public int cardDeclared;
    [SerializeField] private Card CurrentCard;
    public int amountOfCardsPlayed;
    public int totalAmountOfCardsInThePile;
    [SerializeField] public List<Card> CurrentGamePile = new List<Card>(); //Lista de cartas en la pila del juego actual.

    [Header("Player Configuration")]
    public int playerCount = 3;
    public Transform[] playerHands;

    [Header("Player List")]
    [SerializeField] List<ManoloAI> manoloScript;
    public List<Player> CurrentGamePlayers = new List<Player>();
    private List<GameObject> instantiatedDeck = new List<GameObject>(); //Lista de cartas instanciadas en escena, estar� vacia hasta que se cree el deck
    [SerializeField] private List<Card> DiscardedGamePile = new List<Card>(); //Lista de cartas descartadas definitivamente en el juego.

    [Header("UI")]
    [SerializeField] private UI uiComponet;


    void Start()
    {
        Time.timeScale = 1F;
        IsFirstTurn = true;
        CreateAndShuffleDeck();
        DealCards();

        for (int i = 0; i < manoloScript.Count; i++)
        {
            manoloScript[i].ArrangeCards();
        }
    }




    public void AddCardsToCurrentGamePile(Card cardsToPlay) //envia cartas a la pila de juego
    {
        CurrentGamePile.Add(cardsToPlay);

        CardAnimation animation = cardsToPlay.GetComponent<CardAnimation>();
        if (animation != null)
        {
            animation.AnimateCard(cardsToPlay.transform, mainPile, 0.75f, new Vector3(0f, 0f, 180F));
            cardsToPlay.transform.SetParent(mainPile);
        }
        else
        {
            cardsToPlay.transform.localRotation = Quaternion.identity;
            cardsToPlay.transform.SetParent(mainPile);
            cardsToPlay.transform.localPosition = Vector3.zero;

        }

        actualPlayedCard = CurrentGamePile.First();

        if (cardDeclared == 0)
        {
            cardDeclared = actualPlayedCard.cardNumber;
        }

        for (int i = 0; i < manoloScript.Count; i++)
        {
            manoloScript[i].ArrangeCards();
        }
    }




    public void PlayerTurnControl()        //se decide de quien es el turno
    {
        if (IsFirstTurn)
        {
            if (CurrentPlayer >= CurrentGamePlayers.Count - 1)
            {
                IsFirstTurn = false;
                CurrentPlayer = 0;
            }
            else
            {
                IsFirstTurn = false;
                CurrentPlayer += 1;
            }
        }
        else if (CurrentPlayer >= CurrentGamePlayers.Count - 1)
        {
            CurrentPlayer = 0;
            lastPlayer = CurrentGamePlayers.Count - 1;
        }
        else
        {
            if(CurrentPlayer == lastPlayer)
            {
                CurrentPlayer++;
            }
            else
            {
                CurrentPlayer++;
                lastPlayer = CurrentPlayer - 1;
            }
        }

        GetCurrentGamePileAmounts();
        LiarChecker();
    }


    public void GetGamePileToLiar(int playerId) //se ejecuta cuando alguien acusa, envia las cartas dependiendo de quien haya acusado o si ha mentido
    {
        LastPlayerCanBeAccused = false;
        uiComponet.playerID = playerId;

        if (playerId != lastPlayer)
        {
            uiComponet.StartCoroutine(uiComponet.Mentiroso());
            someoneGotAccused = true;

            for (int i = 0; i < manoloScript.Count; i++)
            {
                manoloScript[i].finishTurn();
                manoloScript[i].listOfCardsPlayed.Clear();
            }

            if (didLastPlayerLied)
            {
                for (int i = 0; i < CurrentGamePile.Count; i++)
                {
                    CardAnimation animation = CurrentGamePile[i].GetComponent<CardAnimation>();
                    Vector3 parentEuler = playerHands[lastPlayer].rotation.eulerAngles;
                    animation.AnimateCard(CurrentGamePile[i].transform, playerHands[lastPlayer], 0.75f, parentEuler);
                    CurrentGamePlayers[lastPlayer].playerHand.Add(CurrentGamePile[i]);
                    CurrentGamePile[i].transform.SetParent(playerHands[lastPlayer]);


                    //CurrentGamePile[i].transform.localPosition = Vector3.zero;
                    //CurrentGamePile[i].transform.localRotation = Quaternion.identity;
                    //CurrentGamePile[i].isRaised = false;
                    //CurrentGamePlayers[lastPlayer].playerHand.Add(CurrentGamePile[i]);
                }

                CurrentGamePile.Clear();
                ResetTable(playerId);
                StartCoroutine(ArrangeCardsForPlayer(lastPlayer));

                CurrentPlayer = lastPlayer;
            }
            else
            {
                for (int i = 0; i < CurrentGamePile.Count; i++)
                {
                    CardAnimation animation = CurrentGamePile[i].GetComponent<CardAnimation>();
                    Vector3 parentEuler = playerHands[playerId].rotation.eulerAngles;
                    animation.AnimateCard(CurrentGamePile[i].transform, playerHands[playerId], 0.75f, parentEuler);
                    CurrentGamePlayers[playerId].playerHand.Add(CurrentGamePile[i]);
                    CurrentGamePile[i].transform.SetParent(playerHands[playerId]);
                    //CurrentGamePile[i].transform.SetParent(playerHands[playerId]);
                    //CurrentGamePile[i].transform.localPosition = Vector3.zero;
                    //CurrentGamePile[i].transform.localRotation = Quaternion.identity;
                    //CurrentGamePile[i].isRaised = false;
                    //CurrentGamePlayers[playerId].playerHand.Add(CurrentGamePile[i]);
                }
                CurrentGamePile.Clear();
                ResetTable(playerId);
                StartCoroutine(ArrangeCardsForPlayer(playerId));

                lastPlayer = CurrentPlayer;
                CurrentPlayer = playerId;
            }

            if (lastPlayer == 0 || playerId == 0)
            {
                playerController.ArrangeCards();
            }

            if(!playerController.cardsInHand && CurrentPlayer!=0)
            {
                    playerController.NoCardsLeft = true;
            }
        }
    }

    public IEnumerator ArrangeCardsForPlayer(int playerID)
    {
        yield return new WaitForSeconds(0.76F);
        if (playerID ==0)
        {
            playerController.ArrangeCards();
            playerController.DiscardCards();
        }
        else
        {
            manoloScript[playerID-1].ArrangeCards();
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
                    CardAnimation animation = DiscardedGamePile[i].GetComponent<CardAnimation>();
                    animation.AnimateCard(playerHands[CurrentPlayer].transform, discardedPile, 0.75f, new Vector3(0F, 0F, 0F));
                    DiscardedGamePile[i].transform.SetParent(discardedPile);
                    //DiscardedGamePile[i].transform.localPosition = Vector3.zero;
                    //DiscardedGamePile[i].transform.localRotation = Quaternion.identity;
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
        didLastPlayerLied = false;
        IsFirstTurn = true;
        someoneGotAccused = false;
        gamePileEmpty = true;
        if (CurrentPlayer == 0)
        {
            playerController.ArrangeCards();
        }
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






    public void GetCurrentGamePileAmounts() //indica el total de cartas en la pila en juego
    {
        if (gamePileEmpty)
        {
            totalAmountOfCardsInThePile = CurrentGamePile.Count;
            amountOfCardsPlayed = totalAmountOfCardsInThePile;
            gamePileEmpty = false;
        }
        else
        {
            amountOfCardsPlayed = CurrentGamePile.Count - totalAmountOfCardsInThePile;
            totalAmountOfCardsInThePile = CurrentGamePile.Count;
        }
    }

    public void LiarChecker() //indica si el ultimo jugador a mentido
    {
        for (int i = totalAmountOfCardsInThePile - amountOfCardsPlayed; i < CurrentGamePile.Count; i++)
        {
            if(cardDeclared == CurrentGamePile[i].cardNumber)
            {
                didLastPlayerLied = false;
                Debug.Log("the last player is telling the truth");
            }
            else
            {
                didLastPlayerLied = true;
                i = CurrentGamePile.Count-1;
                Debug.Log("Last player is lying");
            }

            CurrentCard = CurrentGamePile[i];
            Debug.Log(cardDeclared + " comparada con " + CurrentCard.cardNumber);
        }
    }
}
