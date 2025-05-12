using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.GPUSort;
[System.Serializable]
public class Player
{
    [Header("All Player Info")]
    public int playerID;
    public List<Card> playerHand = new List<Card>();
    public bool isHumanPlayer;

    public void AddCard(Card card)
    {
        playerHand.Add(card);
    }

    public void RemoveCard(Card card)
    {
        playerHand.Remove(card);
    }
    public Player(int playerID)
    {
        this.playerID = playerID;
    }
    public bool CanPlayCard(Card card)
    {
        return true;
    }

    public void ClearHand()
    {
        playerHand.Clear();
    }
}
