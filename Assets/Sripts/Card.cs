using UnityEngine;

public class Card : MonoBehaviour
{
    public int cardNumber; // 1 al 12
    public string cardSuits; // Oros, Bastos, Copas y Espadas (Lo vamos a cambiar)
    public bool CardIsSpecial; // Para saber si es una carta especial, vamos a introducir en estas los ases tambi�n
    public Card(int cardNumber, string cardSuits, bool CardIsSpecial = false)
    {
        this.cardNumber = cardNumber;
        this.cardSuits = cardSuits;
        this.cardSuits = cardSuits;
    }
}
