using UnityEngine;
using System.Collections.Generic;

public class CardDealer : MonoBehaviour
{
    [Header("Referencia al controlador de cartas")]
    public GameController gameController;// Referencia al GameController que contiene la lista de cartas

    [Header("Configuraci�n de jugadores")]
    public int playerCount = 4;// N�mero de jugadores
    public Transform[] playerHands;// Transforms donde se colocan las cartas visualmente para cada jugador

    private List<GameObject> instantiatedDeck = new List<GameObject>(); // Lista de cartas instanciadas en escena

    void Start()
    {
        CreateDeck();   // Instancia cada carta seg�n el prefab asociado
        ShuffleDeck();  // Mezcla las cartas
        DealCards();    // Las reparte entre los jugadores
    }

    // Crea visualmente el mazo de cartas a partir de los prefabs definidos en GameController
    void CreateDeck()
    {
        foreach (var cardInfo in gameController.allCards)
        {
            GameObject cardInstance = Instantiate(cardInfo.prefab); // Crea una copia del prefab
            cardInstance.name = cardInfo.cardName;                  // Asigna el nombre para f�cil identificaci�n
            instantiatedDeck.Add(cardInstance);                     // A�ade a la lista del mazo
        }
    }

    // Mezcla el mazo instanciado usando el algoritmo Fisher�Yates
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

    // Reparte las cartas de forma equitativa entre los jugadores
    void DealCards()
    {
        int currentPlayer = 0;

        foreach (GameObject card in instantiatedDeck)
        {
            // Asigna la carta visualmente al jugador actual
            card.transform.SetParent(playerHands[currentPlayer]);

            //Aqu� falta por a�adir a donde se traslada la carta y el a�adirla a la mano del jugador

            currentPlayer = (currentPlayer + 1) % playerCount; //Cambiamos entre los jugadores entre jugadores
        }
    }
}
