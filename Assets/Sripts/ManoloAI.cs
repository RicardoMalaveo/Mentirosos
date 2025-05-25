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
    [SerializeField] private bool cardsPlayed = false;
    [SerializeField] private bool hasCardsWithTheDeclaredNumber;
    [SerializeField] private bool lastPlayerMightBeLying= false;
    [SerializeField] private float declaredCardRisk;


    [Header("Card Spacing")]
    [SerializeField] private float cardSpacingX = 1.5f;
    [SerializeField] private float cardSpacingZ = 0.001f;

    [SerializeField] private float timer;
    [SerializeField] List<Card> cardsToPlay;
    [SerializeField] List<Card> cardsValue;
    [SerializeField] public List<Card> listOfCardsPlayed;

    public Player controlledPlayer;


    void Awake()
    {
        InitializeController();
        ArrangeCards();
    }

    private void InitializeController()
    {
        controlledPlayer = cardDealer.CurrentGamePlayers.Find(player => player.playerID == controlledPlayerID);
    }


    void Update()
    {
        if (cardDealer.CurrentPlayer == controlledPlayerID && !myTurn)
        {
            DiscardCards();

            Debug.Log("turn started");
        }


        if (checkedCardsToDiscard && !cardDealer.someoneGotAccused)
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

        if(timer > 4F && myTurn)
        {
            finishTurn();
            Debug.Log("done");
        }
    }


    private void ManoloRiskCalculator() //ejecuta varias funciones, dependiendo del caso el valor de manolo risk sera mas o menos alto.
    {
        CalculateCardsOnHandValue();
        DeckCheckerForDeclaredCardNumber();
        CalculateIfLastPlayerIsLying();



        if (!cardDealer.IsFirstTurn)
        {
            manoloRisk += declaredCardRisk / 6;
            cardsValue.Clear();
        }
        else
        {
            manoloRisk -= deckValue / 200;
            cardsValue.Clear();
        }

        ManoloBehavior(manoloRisk);
        deckCalculated = true;
    }

    public static bool ManoloBehavior(float manoloRisk)
    {
        float clampedProb = Mathf.Clamp01(manoloRisk);
        return Random.Range(0f, 1f) < clampedProb;
    }

    private void startTurn() //crea un pequeño retraso que permite que el jugador principal acuse al jugador anterior.
    {
        if (timer > 3F && !cardsPlayed)
        {
            ChoosingAndPlayingCards();
            cardsPlayed = true;
        }
    }




    void ChoosingAndPlayingCards() //basado en multiples variables, manolo decidira como jugar.
    {
        int randomAmount;
        int randomCardNumber;
        int randomPosicion;

        if (cardDealer.IsFirstTurn) //solo entra aqui si es el primer turno.
        {
            if (ManoloBehavior(manoloRisk)) //en este caso el riesgo se calcula evaluando el valor del las cartas en mano, si el valor es bajo, es mas probable que diga la verdad
            {
                randomPosicion = Random.Range(0, controlledPlayer.playerHand.Count);
                for (int i = 0; i < controlledPlayer.playerHand.Count; i++)
                {
                    if (controlledPlayer.playerHand[randomPosicion].cardNumber == controlledPlayer.playerHand[i].cardNumber)
                    {
                        cardDealer.AddCardsToCurrentGamePile(controlledPlayer.playerHand[i]);
                        listOfCardsPlayed.Add(controlledPlayer.playerHand[i]);
                        controlledPlayer.playerHand.RemoveAt(i);
                    }
                }
            }
            else //si el valor de las cartas en mano es alto, es mas probably que mienta, seleccione una cantidad de cartas al azar y posiciones al azar.
            {
                randomAmount = Random.Range(1, 5);
                
                randomCardNumber = Random.Range(1, 13);
                for (int i = 0; i < randomAmount; i++)
                {
                    randomPosicion = Random.Range(1, controlledPlayer.playerHand.Count);
                    cardDealer.AddCardsToCurrentGamePile(controlledPlayer.playerHand[randomPosicion]);
                    listOfCardsPlayed.Add(controlledPlayer.playerHand[randomPosicion]);
                    controlledPlayer.playerHand.RemoveAt(randomPosicion);
                }

                cardDealer.cardDeclared = randomCardNumber;
            }
        }
        else
        {
            if (ManoloBehavior(manoloRisk) && hasCardsWithTheDeclaredNumber)
            {
                for (int i = 0; i < controlledPlayer.playerHand.Count; i++)
                {
                    if (cardDealer.cardDeclared == controlledPlayer.playerHand[i].cardNumber)
                    {
                        cardDealer.AddCardsToCurrentGamePile(controlledPlayer.playerHand[i]);
                        listOfCardsPlayed.Add(controlledPlayer.playerHand[i]);
                        controlledPlayer.playerHand.RemoveAt(i);
                    }
                }
            }
            else
            {
                randomAmount = Random.Range(1, 5);

                if(controlledPlayer.playerHand.Count <1)
                {
                    cardDealer.AddCardsToCurrentGamePile(controlledPlayer.playerHand[0]);
                    listOfCardsPlayed.Add(controlledPlayer.playerHand[0]);
                    controlledPlayer.playerHand.RemoveAt(0);
                }
                else
                {
                    for (int i = 0; i < randomAmount; i++)
                    {
                        randomPosicion = Random.Range(0, controlledPlayer.playerHand.Count);
                        cardDealer.AddCardsToCurrentGamePile(controlledPlayer.playerHand[randomPosicion]);
                        listOfCardsPlayed.Add(controlledPlayer.playerHand[randomPosicion]);
                        controlledPlayer.playerHand.RemoveAt(randomPosicion);
                    }
                }
            }
        }
    }


    void CalculateCardsOnHandValue() //da un valor a las cartas en mano dependiendo de cuantas veces se repitan.
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
    }

    void DeckCheckerForDeclaredCardNumber() //indica si manolo tiene cartas con el mismo numero, ayuda a tomar una decision en cuanto a que carta jugar.
    {
        for (int y = 0; y < controlledPlayer.playerHand.Count; y++) 
        {
            if (controlledPlayer.playerHand[y].cardNumber == cardDealer.cardDeclared)
            {
                hasCardsWithTheDeclaredNumber = true;
                y = controlledPlayer.playerHand.Count;
            }
            else
            {
                hasCardsWithTheDeclaredNumber = false;
            }
        }
    }

   void CalculateIfLastPlayerIsLying() //devuelve un valor basado en cuantas cartas posee con el mismo numero que la carta declarada.
    {
        for (int x = 0; x < controlledPlayer.playerHand.Count; x++)
        {
            if (cardDealer.cardDeclared == controlledPlayer.playerHand[x].cardNumber)
            {
                cardsValue.Add(controlledPlayer.playerHand[x]);
            }
        }
        declaredCardRisk = cardsValue.Count;
    }

    void ArrangeCards() //posiciona las cartas en la mano del jugador con una separacion.
    {
        for (int i = 0; i < controlledPlayer.playerHand.Count; i++)
        {
            float xPos = i * cardSpacingX;
            float ZPos = i * cardSpacingZ;
            controlledPlayer.playerHand[i].transform.localPosition = new Vector3(xPos, ZPos, 0);
        }
    }

    void DiscardCards() //descarta cartas cuando tiene 4 con el mismo numero.
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
                for (int y = 0; y < cardsToPlay.Count; y++)
                {
                    controlledPlayer.playerHand.Remove(cardsToPlay[y]);
                }
                Debug.Log("discarding 4 cards");
            }
            cardsToPlay.Clear();
        }

        cardsToPlay.Clear();

        checkedCardsToDiscard = true;
    }

    public void finishTurn() //termina el turno
    {
        if(!cardDealer.someoneGotAccused)
        {
            cardDealer.PlayerTurnControl();
        }

        declaredCardRisk = 0;
        timer = 0;
        deckValue = 0;
        myTurn = false;
        hasCardsWithTheDeclaredNumber = false;
        deckCalculated = false;
        checkedCardsToDiscard = false;
        cardsPlayed = false;
    }
}
