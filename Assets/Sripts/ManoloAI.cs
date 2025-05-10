using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using static DeckInfo;

public class ManoloAI
{
    private List<Card> playerHand;
    private int currentDeclaredNumber;
    private string currentDeclaredSuit;
    private List<CardInfo> discardPile;
    private CardDealer cardDealer;

    //Estas variables permiten calcular si se quiere tomar una decision o no, "1" es 100% ¡SI! y "0" es 100% ¡NO!
    private float ManoloRisk = 0.5f; // a este numero se suma o se resta, este numero representa un rango de 100%, siempre empieza en 50% "neutral".
}
