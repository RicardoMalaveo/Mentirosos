using UnityEngine;

public class Instructions : MonoBehaviour
{
    /*
    Tenemos el InstantiatedDeck que es una copia de DeckInfo
    -En DeckInfo se encuentran todos los tipos de carta que pueden haber en el juego (esto para el dev mode) as� si es necesario
    a�adir m�s cartas al juego se pueden hacer desde ah�. 
        -CardInfo: clase interna que describe una carta modelo (nombre, n�mero, palo, si es especial, prefab).
        -allCards: lista manualmente definida desde el editor. Esta lista no se instancia en la escena; solo sirve como referencia.

    -En InstantiatedDeck se encuentran todas las cartas que van a jugarse, �stas pueden est�r repetidas y al iniciarse la partida
    se instancia la lista de objetos, esto hace que aparezcan en el tablero todas las cartas que se jugar�n en la partida,
    acontinuaci�n se mezclan las cartas en la baraja y se epieza a hacer el reparto de �stas.
        -instantiatedDeck: mazo visual, objetos reales instanciados desde los prefabs en DeckInfo.allCards.
        -CreateDeck(): recorre DeckInfo.allCards, instancia un GameObject por cada tipo de carta (solo una vez por tipo, actualmente).
            Falta l�gica para crear m�ltiples copias si un juego lo requiere.

        -ShuffleDeck(): algoritmo Fisher-Yates sobre los objetos instanciados.
        -DealCards(): Recorre las cartas mezcladas y las asigna visualmente a las manos (playerHands[currentPlayer]) alternando jugador.
            -Falta: Posicionamiento espacial de las cartas (posici�n/rotaci�n). Registro l�gico en las playerHand del Player.

    -Tenemos tambi�n una lista de players para controlar las rondas, es una representaci�n l�gica (Interna del programa) no visual.
     
     
     
     */
}
