using System;
using System.Collections.Generic;

public class Player //Clase de Player
{
    public int playerID;
    public List<Card> playerHand = new List<Card>();
    public bool isHumanPlayer = false;

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
