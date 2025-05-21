using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using static DeckInfo;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;


public class ManoloAI : MonoBehaviour
{
    [SerializeField] float manoloRisk = 0.5f;//Esta variables permiten calcular si se quiere tomar una decision o no, "1" es 100% ¡SI! y "0" es 100% ¡NO!
                                             //A este numero se le suma o se le resta, este numero representa un rango de 0 a 100%, siempre empieza en 50% "neutral".

    [Header("Player Info")]
    [SerializeField] private float deckValue = 0;
    [SerializeField] private int controlledPlayerID;
    [SerializeField] private CardDealer cardDealer;

    [SerializeField] private bool myTurn = false;
    [SerializeField] private bool checkedCardsToDiscard = false;
    [SerializeField] private bool deckCalculated = false;
    [SerializeField] private float declaredCardRisk;


    [Header("Card Spacing")]
    [SerializeField] private float cardSpacingX = 1.5f;
    [SerializeField] private float cardSpacingZ = 0.001f;

    [SerializeField] private float timer;
    [SerializeField] List<Card> cardsToPlay;
    [SerializeField] List<Card> cardsValue;


    public Player controlledPlayer;
    void Awake()
    {
        InitializeController();
    }
    private void InitializeController()
    {
        controlledPlayer = cardDealer.CurrentGamePlayers.Find(player => player.playerID == controlledPlayerID);
        ArrangeCards();
    }


    void Update()
    {
        if (cardDealer.CurrentPlayer == controlledPlayerID && !myTurn)
        {
            DiscardCards();

            Debug.Log("turn started");
        }


        if (checkedCardsToDiscard)
        {
            myTurn = true;
        }

        if(!deckCalculated && myTurn)
        {
            ManoloRiskCalculator();
        }

        if (cardDealer.CurrentPlayer == controlledPlayerID && myTurn) 
        {

            timer += Time.deltaTime;
            startTurn();
        }

        if(timer > 10F && myTurn)
        {
            finishTurn();
            Debug.Log("done");
        }
    }


    private void ManoloRiskCalculator()
    {
        if(!cardDealer.IsFirstTurn)
        {
            for (int i = 0; i < controlledPlayer.playerHand.Count; i++)
            {
                if(cardDealer.cardDeclared == controlledPlayer.playerHand[i].cardNumber)
                {
                    cardsValue.Add(controlledPlayer.playerHand[i]);
                }
            }
            declaredCardRisk = cardsValue.Count;
            manoloRisk += declaredCardRisk / 6;
            cardsValue.Clear();
            Debug.Log("second play");
        }
        else
        {
            for (int i = 0; i < controlledPlayer.playerHand.Count; i++)
            {
                for (int x = 0; x < controlledPlayer.playerHand.Count; x++)
                {
                    if (controlledPlayer.playerHand[i].cardNumber == controlledPlayer.playerHand[x].cardNumber)
                    {
                        cardsValue.Add(controlledPlayer.playerHand[x]);
                    }
                }

                if (cardsValue.Count > 1)
                {
                    for (int y = 0; y < cardsValue.Count; y++)
                    {
                        cardsValue[y].cardValue = cardsValue.Count;
                    }
                }

                deckValue += controlledPlayer.playerHand[i].cardValue;
                cardsValue.Clear();
            }

            manoloRisk -= deckValue / 100;
            Debug.Log("first play");
        }

        ManoloBehavior(manoloRisk);
        deckCalculated = true;
    }

    public static bool ManoloBehavior(float manoloRisk)
    {
        float clampedProb = Mathf.Clamp01(manoloRisk);
        return Random.Range(0f, 1f) < clampedProb;
    }

    private void startTurn()
    {

        if (timer > 2F)
        {
            Debug.Log("choosing cards to play");
            Debug.Log(ManoloBehavior(manoloRisk));
            Debug.Log(manoloRisk);
        }

    }




    void ChoosingAndPlayingCards()
    {
        if (ManoloBehavior(manoloRisk))
        {
           int randomNum = Mathf.Min(Random.Range(1, controlledPlayer.playerHand.Count), controlledPlayer.playerHand.Count);
            for (int i = 0; i < controlledPlayer.playerHand.Count; i++)
            {
                if (controlledPlayer.playerHand[randomNum].cardNumber == controlledPlayer.playerHand[i].cardNumber)
                {
                    cardDealer.AddCardsToCurrentGamePile(controlledPlayer.playerHand[i]);
                    controlledPlayer.RemoveCard(controlledPlayer.playerHand[i]);
                    controlledPlayer.playerHand.RemoveAt(i);
                }
            }
        }
        else
        {

        }
    }

    void ArrangeCards()
    {
        for (int i = 0; i < controlledPlayer.playerHand.Count; i++)
        {
            float xPos = i * cardSpacingX;
            float ZPos = i * cardSpacingZ;
            controlledPlayer.playerHand[i].transform.localPosition = new Vector3(xPos, ZPos, 0);
        }
    }

    void DiscardCards()
    {
        for (int i = 0; i < controlledPlayer.playerHand.Count; i++)
        {
            for (int x = 0; x < controlledPlayer.playerHand.Count; x++)
            {
                if (controlledPlayer.playerHand[i].cardNumber == controlledPlayer.playerHand[x].cardNumber)
                {
                    cardsToPlay.Add(controlledPlayer.playerHand[x]);
                }
            }

            if (cardsToPlay.Count > 3)
            {
                cardDealer.AddToDiscardedGamePile(cardsToPlay);
            }
            cardsToPlay.Clear();
        }
        cardsToPlay.Clear();
        Debug.Log("done checking for cards to discard");
        checkedCardsToDiscard = true;
    }

    void finishTurn()
    {
        cardDealer.PlayerTurnControl();
        timer = 0;
        myTurn = false;
        checkedCardsToDiscard = false;
    }

    //private void ManolosTurn()
    //{
    //    int randomNumb;
    //    if (cardDealer.IsFirstTurn)
    //    {
    //        if (ManoloBehavior(manoloRisk))
    //        {
    //            randomNumb = Mathf.Min(Random.Range(1, 16), controlledPlayer.playerHand.Count);
    //            for (int i = 0; i < controlledPlayer.playerHand.Count; i++)
    //            {
    //                if (controlledPlayer.playerHand[randomNumb].cardNumber == controlledPlayer.playerHand[i].cardNumber)
    //                {
    //                    cardDealer.AddCardsToCurrentGamePile(controlledPlayer.playerHand[i]);
    //                    controlledPlayer.RemoveCard(controlledPlayer.playerHand[i]);
    //                    controlledPlayer.playerHand.RemoveAt(i);
    //                }
    //            }

    //        }

    //    }
    //    else
    //    {
    //        if (ManoloBehavior(manoloRisk))
    //        {
    //            if (ManoloBehavior(manoloRisk))
    //            {
    //                if (cardDealer.cardDeclared == 0)
    //                {
    //                    for (int i = 0; i < controlledPlayer.playerHand.Count; i++)
    //                    {
    //                        if (cardDealer.actualPlayedCard.cardNumber == controlledPlayer.playerHand[i].cardNumber)
    //                        {
    //                            cardDealer.AddCardsToCurrentGamePile(controlledPlayer.playerHand[i]);
    //                            controlledPlayer.RemoveCard(controlledPlayer.playerHand[i]);
    //                            controlledPlayer.playerHand.RemoveAt(i);
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    for (int i = 0; i < controlledPlayer.playerHand.Count; i++)
    //                    {
    //                        if (cardDealer.cardDeclared == controlledPlayer.playerHand[i].cardNumber)
    //                        {
    //                            cardDealer.AddCardsToCurrentGamePile(controlledPlayer.playerHand[i]);
    //                            controlledPlayer.RemoveCard(controlledPlayer.playerHand[i]);
    //                            controlledPlayer.playerHand.RemoveAt(i);
    //                        }
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                randomNumb = Mathf.Min(Random.Range(1, 4), controlledPlayer.playerHand.Count);
    //                for (int i = 0; i < randomNumb; i++)
    //                {
    //                    cardDealer.AddCardsToCurrentGamePile(controlledPlayer.playerHand[i]);
    //                    controlledPlayer.RemoveCard(controlledPlayer.playerHand[i]);
    //                    controlledPlayer.playerHand.RemoveAt(i);
    //                }
    //            }
    //        }
    //        else
    //        {
    //            cardDealer.GetGamePileToLiar(controlledPlayer.playerID);
    //        }
    //    }

    //    ArrangeCards();
    //    cardDealer.PlayerTurnControl();
    //}
}
