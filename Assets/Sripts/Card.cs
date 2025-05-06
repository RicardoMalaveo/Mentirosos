using UnityEngine;

public class Card : MonoBehaviour //Esta clase es un objeto en el juego, es la que añadimos como instancias en el juego
{
    public int cardNumber;
    public string cardSuits;
    public bool cardIsSpecial;
    public Card(int cardNumber, string cardSuits, bool cardIsSpecial = false)
    {
        this.cardNumber = cardNumber;
        this.cardSuits = cardSuits;
        this.cardIsSpecial = cardIsSpecial;
    }
}
