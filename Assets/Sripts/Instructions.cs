using UnityEngine;

public class Instructions : MonoBehaviour
{
    /*
    Tenemos el InstantiatedDeck que es una copia de DeckInfo
    -En DeckInfo se encuentran todos los tipos de carta que pueden haber en el juego (esto para el dev mode) así si es necesario
    añadir más cartas al juego se pueden hacer desde ahí. 
        -CardInfo: clase interna que describe una carta modelo (nombre, número, palo, si es especial, prefab).
        -allCards: lista manualmente definida desde el editor. Esta lista no se instancia en la escena; solo sirve como referencia.

    -En InstantiatedDeck se encuentran todas las cartas que van a jugarse, éstas pueden estár repetidas y al iniciarse la partida
    se instancia la lista de objetos, esto hace que aparezcan en el tablero todas las cartas que se jugarán en la partida,
    acontinuación se mezclan las cartas en la baraja y se epieza a hacer el reparto de éstas.
        -instantiatedDeck: mazo visual, objetos reales instanciados desde los prefabs en DeckInfo.allCards.
        -CreateDeck(): recorre DeckInfo.allCards, instancia un GameObject por cada tipo de carta (solo una vez por tipo, actualmente).
            Falta lógica para crear múltiples copias si un juego lo requiere.

        -ShuffleDeck(): algoritmo Fisher-Yates sobre los objetos instanciados.
        -DealCards(): Recorre las cartas mezcladas y las asigna visualmente a las manos (playerHands[currentPlayer]) alternando jugador.
            -Falta: Posicionamiento espacial de las cartas (posición/rotación). Registro lógico en las playerHand del Player.

    -Tenemos también una lista de players para controlar las rondas, es una representación lógica (Interna del programa) no visual.
     
     
     
     */
}
