using UnityEngine;

public class Card : MonoBehaviour //Esta clase es un objeto en el juego, es la que añadimos como instancias en el juego
{
    public int cardNumber;
    public string cardSuits;
    public int cardValue;
    public bool cardIsSpecial;
    public bool isRaised = false;
    public bool isInPlay = false;
    public Vector3 initialLocalPosition;

    void Start()
    {
        initialLocalPosition = transform.localPosition;
    }
    public void SetInteractable(bool state)
    {
        GetComponent<Collider>().enabled = state;
    }

    public void GetCard(int number, string suits, bool isSpecial = false)
    {
        cardNumber = number;
        cardSuits = suits;
        cardIsSpecial = isSpecial;
    }
}
