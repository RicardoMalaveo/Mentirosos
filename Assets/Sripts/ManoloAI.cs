using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using static DeckInfo;

public class ManoloAI : MonoBehaviour
{
    //Estas variables permiten calcular si se quiere tomar una decision o no, "1" es 100% ¡SI! y "0" es 100% ¡NO!
    private float manoloRisk = 0.5f; // a este numero se suma o se resta, este numero representa un rango de 100%, siempre empieza en 50% "neutral".

    [Header("Player Info")]
    [SerializeField] private int controlledPlayerID;
    [SerializeField] private CardDealer cardDealer;

    [Header("Card Spacing")]
    [SerializeField] private float cardSpacingX = 1.5f;
    [SerializeField] private float cardSpacingZ = 0.001f;

    public Player controlledPlayer;
    void Awake()
    {
        InitializeController();
    }

    void Start()
    {
        ArrangeCards();
    }

    void Update()
    {
        if (cardDealer.CurrentPlayer == controlledPlayerID)
        {
            ManoloRiskCalculator();
            ManolosTurn();
        }

    }

    private void InitializeController()
    {
        controlledPlayer = cardDealer.CurrentGamePlayers.Find(player => player.playerID == controlledPlayerID);
        ArrangeCards();
    }

    private void ManoloRiskCalculator()
    {
        ManoloBehavior(manoloRisk);
        Debug.Log(ManoloBehavior(manoloRisk));
    }

    public static bool ManoloBehavior(float manoloRisk)
    {
        float clampedProb = Mathf.Clamp01(manoloRisk);
        return Random.Range(0f, 1f) < clampedProb;
    }

    private void ManolosTurn()
    {
        int randomNumb;
        if (cardDealer.IsFirstTurn)
        {
            if (ManoloBehavior(manoloRisk))
            {
                randomNumb = Mathf.Min(Random.Range(1, 16), controlledPlayer.playerHand.Count);
                for (int i = 0; i < controlledPlayer.playerHand.Count; i++)
                {
                    if (controlledPlayer.playerHand[randomNumb].cardNumber == controlledPlayer.playerHand[i].cardNumber)
                    {
                        cardDealer.AddCardsToCurrentGamePile(controlledPlayer.playerHand[i]);
                        controlledPlayer.RemoveCard(controlledPlayer.playerHand[i]);
                        controlledPlayer.playerHand.RemoveAt(i);
                    }
                }

            }

        }
        else
        {
            if (ManoloBehavior(manoloRisk))
            {
                if (ManoloBehavior(manoloRisk))
                {
                    if (cardDealer.cardDeclared == 0)
                    {
                        for (int i = 0; i < controlledPlayer.playerHand.Count; i++)
                        {
                            if (cardDealer.actualPlayedCard.cardNumber == controlledPlayer.playerHand[i].cardNumber)
                            {
                                cardDealer.AddCardsToCurrentGamePile(controlledPlayer.playerHand[i]);
                                controlledPlayer.RemoveCard(controlledPlayer.playerHand[i]);
                                controlledPlayer.playerHand.RemoveAt(i);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < controlledPlayer.playerHand.Count; i++)
                        {
                            if (cardDealer.cardDeclared == controlledPlayer.playerHand[i].cardNumber)
                            {
                                cardDealer.AddCardsToCurrentGamePile(controlledPlayer.playerHand[i]);
                                controlledPlayer.RemoveCard(controlledPlayer.playerHand[i]);
                                controlledPlayer.playerHand.RemoveAt(i);
                            }
                        }
                    }
                }
                else
                {
                    randomNumb = Mathf.Min(Random.Range(1, 4), controlledPlayer.playerHand.Count);
                    for (int i = 0; i < randomNumb; i++)
                    {
                        cardDealer.AddCardsToCurrentGamePile(controlledPlayer.playerHand[i]);
                        controlledPlayer.RemoveCard(controlledPlayer.playerHand[i]);
                        controlledPlayer.playerHand.RemoveAt(i);
                    }
                }
            }
            else
            {
                cardDealer.GetGamePileToLiar(controlledPlayer.playerID);
            }
        }

        ArrangeCards();
        cardDealer.PlayerTurnControl();
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
}
