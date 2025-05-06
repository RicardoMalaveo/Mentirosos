using UnityEngine;
using System.Collections.Generic;

public class CardDealer : MonoBehaviour
{
    [Header("Deck Info Database Reference")]
    public DeckInfo deckInfo;

    [Header("Player Configuration")]
    public int playerCount = 4;
    public Transform[] playerHands;//Transforms donde se colocan las cartas visualmente para cada jugador

    private List<GameObject> instantiatedDeck = new List<GameObject>(); //Lista de cartas instanciadas en escena, estará vacia hasta que se cree el deck

    void Start()
    {
        CreateDeck();
        ShuffleDeck();
        DealCards();
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
}
