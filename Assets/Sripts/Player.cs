﻿using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.GPUSort;
[System.Serializable]
public class Player //Clase de Player
{
    [Header("All Player Info")]
    public int playerID;
    public List<Card> playerHand = new List<Card>();
    public bool isHumanPlayer;

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
    public bool CanPlayCard(Card card)
    {
        return true;
    }

    public void ClearHand()
    {
        playerHand.Clear();
    }
}
