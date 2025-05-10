using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.GPUSort;
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

    void start()
    {
        foreach (Card card in playerHand)
        {
            Debug.Log($" | Number: {playerHand[playerID].cardNumber} | Suit: {playerHand[playerID].cardSuits}");
        }
    }
}
