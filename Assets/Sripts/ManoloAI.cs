using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using static DeckInfo;

public class ManoloAI : MonoBehaviour
{
    //Estas variables permiten calcular si se quiere tomar una decision o no, "1" es 100% ¡SI! y "0" es 100% ¡NO!
    private float manoloRisk = 0.5f; // a este numero se suma o se resta, este numero representa un rango de 100%, siempre empieza en 50% "neutral".

    [Header("Player Info")]
    [SerializeField] private int controlledPlayerID = 0;
    [SerializeField] private CardDealer cardDealer;

    [Header("Card Spacing")]
    [SerializeField] private float cardSpacingX = 1.5f;
    [SerializeField] private float cardSpacingZ = 0.001f;

    public Player controlledPlayer;
    void Awake()
    {
        InitializeController();
    }

    void Update()
    {
        //ManoloBehavior();
        ArrangeCards();
    }

    private void InitializeController()
    {
        controlledPlayer = cardDealer.CurrentGamePlayers.Find(player => player.playerID == controlledPlayerID);
        ArrangeCards();
    }

    //private void ManoloBehavior()
    //{
    //}

    public void PlayCard(Card card)
    {
        controlledPlayer.RemoveCard(card);
        ArrangeCards();
    }
    private void ArrangeCards()
    {
        for (int i = 0; i < controlledPlayer.playerHand.Count; i++)
        {
            float xPos = i * cardSpacingX;
            float ZPos = i * cardSpacingZ;
            controlledPlayer.playerHand[i].transform.localPosition = new Vector3(xPos, ZPos, 0);
        }
    }
    public void AddCardToHand(Card card)
    {
        controlledPlayer.AddCard(card);
        card.transform.SetParent(transform);
        ArrangeCards();
    }

    public void RemoveCardFromHand(Card card)
    {
        controlledPlayer.RemoveCard(card);
        ArrangeCards();
    }
}
