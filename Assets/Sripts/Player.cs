using System;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Player //Clase de Player
{
    [Header("All Player Info")]
    public int playerID;
    public List<Card> playerHand = new List<Card>();
    public bool isHumanPlayer = false;

    public void AddCarta(Card card)
    {
        playerHand.Add(card);
    }

    public void RemoveCarta(Card card)
    {
        playerHand.Remove(card);
    }
    public Player(int playerID)
    {
        this.playerID = playerID;
    }

    public virtual List<Card> JugarCartas(int quantity, int declaredNumber, bool lie)
    {
        //Lógica simple para devolver cartas
        return new List<Card>();
    }
}
