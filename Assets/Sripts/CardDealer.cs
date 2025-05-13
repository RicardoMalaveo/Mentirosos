using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using static DeckInfo;

public class CardDealer : MonoBehaviour
{
    [Header("Game State Info")]
    [SerializeField] private bool IsFirstTurn;

    [Header("Players")]
    [SerializeField] private Player playerInfo;
    [SerializeField] private int CurrentPlayer;
    [SerializeField] private int LastPlayer;

    [Header("Deck")]
    public DeckInfo deckInfo;
    public Transform mainPile;

    [Header("Game Pile")]
    [SerializeField] private Card LastCardInf;
    [SerializeField] private Card CurrentCardInf;
    [SerializeField] private int amountOfCardsPlayed;
    [SerializeField] private int totalAmountOfCardsInThePile;

    [SerializeField] private List<Card> CurrentGamePile = new List<Card>(); //Lista de cartas en la pila del juego actual.
    [SerializeField] private bool didLastPlayerLied;

    [Header("Player Configuration")]
    public int playerCount = 3;
    public Transform[] playerHands;

    [Header("Player List")]
    public List<Player> CurrentGamePlayers = new List<Player>();
    private List<GameObject> instantiatedDeck = new List<GameObject>(); //Lista de cartas instanciadas en escena, estará vacia hasta que se cree el deck
    [SerializeField] private List<Card> DiscardedGamePile = new List<Card>(); //Lista de cartas descartadas definitivamente en el juego.


    void Start()
    {
        IsFirstTurn = true;
        CreateAndShuffleDeck();
        DealCards();
    }
    //Crea visualmente el mazo de cartas a partir de los prefabs definidos
    //Mezcla el mazo instanciado usando el algoritmo Fisher–Yates
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

    //Reparte las cartas de forma equitativa entre los jugadores
    void DealCards()
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
    public void AddCardsToCurrentGamePile(Card cardsToPlay, Player player)
    {
        CurrentGamePile.Add(cardsToPlay);
        cardsToPlay.transform.SetParent(mainPile);
        cardsToPlay.transform.localPosition = Vector3.zero;
        cardsToPlay.transform.localRotation = Quaternion.identity;
        CurrentCardInf = CurrentGamePile.Last();
        LastCardInf = CurrentGamePile.Last();
    }

    public void PlayerTurnControl()
    {
        //se decide de quien es el turno
        if(IsFirstTurn)
        {
            CurrentPlayer = 0;
            IsFirstTurn = false;
        }
        else if(CurrentPlayer >= CurrentGamePlayers.Count -1)
        {
            CurrentPlayer = 0;
            LastPlayer += 1;
        }
        else
        {
            CurrentPlayer++;
            LastPlayer = CurrentPlayer - 1;
        }
    }

    void GetCurrentGamePile() //añade elementos a la pila descartada y la pila de valores reales
    {
        //aqui se toman dos valores, la cantidad de cartas descartadas declaradas por el ultimo jugador y el valor de las cartas
        //aqui se toman dos valores mas, estos son los valores actuales de las cartas, el palo y el numero actual de cada carta

        //hay dos listas declaradas previamente piladeclarada y la piladevaloresreales
    }

    void ResolveAcusation()
    {
        //aqui se comparan los valores de la pila declarada y la piladevaloresreales

        //encualquier caso la piladeclarada se resetea ".Clear(); y se devuelve un true or false a la bool de acusation.
    }

    void AddToDiscardedGamePile()
    {
        //aqui se agregan elementos a la pila de cartas descartadas finalmente.
        //cada turno verifica si el jugador que esta de turno tiene grupos de 4 cartas para descartar automaticamente.
    }
}
