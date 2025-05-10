using UnityEngine;
using System.Collections.Generic;
using static DeckInfo;

public class CardDealer : MonoBehaviour
{
    [Header("Deck Info Database Reference")]
    public DeckInfo deckInfo;

    [Header("Player Configuration")]
    public int playerCount = 3;
    public Transform[] playerHands;//Transforms donde se colocan las cartas visualmente para cada jugador

    private List<Player> CurrrentGamePlayers = new List<Player>(); //lista total de juegos actuales
    private List<GameObject> instantiatedDeck = new List<GameObject>(); //Lista de cartas instanciadas en escena, estará vacia hasta que se cree el deck
    private List<CardInfo> CurrentGamePile = new List<CardInfo>(); //Lista de cartas en la pila del juego actual.
    private List<CardInfo> DiscardedGamePile = new List<CardInfo>(); //Lista de cartas descartadas definitivamente en el juego.

    private int currentDeclaredNumber;

    void Start()
    {
        CreateCurrentPlayers();
        CreateDeck();
        ShuffleDeck();
        DealCards();
    }
    //Crea multiples player dependiendo de la cantidad de jugadores estipulados en playercount
    void CreateCurrentPlayers()
    {
        for(int i = 0; i <= playerCount; i++)
        {
            CurrrentGamePlayers.Add(new Player(i));
        }

    }
    //Crea visualmente el mazo de cartas a partir de los prefabs definidos en GameController
    void CreateDeck()
    {
        //Si en un futuro queremos añadir 2 cartas identicas por el motivo que fuera podemos hacerlo a través de esta función
        foreach (var cardInfo in deckInfo.allCards)
        {
            GameObject cardInstance = Instantiate(cardInfo.prefab); 
            cardInstance.name = cardInfo.cardName;                  
            instantiatedDeck.Add(cardInstance);                     
        }
    }

    //Mezcla el mazo instanciado usando el algoritmo Fisher–Yates
    void ShuffleDeck()
    {
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
        int currentPlayer = 0;

        foreach (GameObject card in instantiatedDeck)
        {
            card.transform.SetParent(playerHands[currentPlayer]);

            //Aquí falta por añadir a donde se traslada la carta y el añadirla a la mano del jugador

            currentPlayer = (currentPlayer + 1) % playerCount; //Cambiamos entre los jugadores
        }
    }
    void CurrentPlay()
    {
        //se decide de quien es el turno
        //se decide si se va a acusar o a jugar(primer turno la corrutina no se ejecuta, solo se puede jugar)
        //en caso de acusar se ejecuta resolver acusacion
        //finalmente se juega, seleccionas las cartas a jugar y en caso de ser el primer jugador de la ronda, decide si mentir o no y decides el numero de la carta 
        //junto a la cantidad, en caso de no ser el primer jugador de la ronda, solo se puede acusar y mentir o no, seleccionando las cartas en mano.

        //se ejecuta currentgamepile
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
