using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    [SerializeField] private CardDealer cardDealer;
    [SerializeField] private PlayerController mainPlayer;
    public TextMeshProUGUI playingNumberText;
    public TextMeshProUGUI amountOfCardsOnTheTable;
    public TextMeshProUGUI mentiroso;
    public TextMeshProUGUI turnos;
    public TextMeshProUGUI quienAcusaAquien;
    public string players;
    public int playerID;
    public int playerIDWithNoCards;
    public string currentAcuser;
    public GameObject numPad;
    public GameObject endGamePanel;
    public GameObject gameUI;
    public GameObject defeat;
    public GameObject victory;
    private void accusar()
    {
        cardDealer.GetGamePileToLiar(mainPlayer.controlledPlayer.playerID);
    }


    private void Update()
    {
        if(cardDealer.cardDeclared !=0)
        {
            playingNumberText.text = "Número de carta declarada: " + cardDealer.cardDeclared.ToString() + " x " + cardDealer.amountOfCardsPlayed.ToString() + "Cartas";
            amountOfCardsOnTheTable.text = "Cartas en juego: " + cardDealer.totalAmountOfCardsInThePile.ToString(); ;
        }
        else
        {
            playingNumberText.text = "Número de carta declarada: No se ha declarado ninguna carta";
            amountOfCardsOnTheTable.text = "Cartas en juego: no hay cartas en juego";
        }


        if(cardDealer.CurrentPlayer==0 && cardDealer.IsFirstTurn)
        {
            numPad.SetActive(true);
        }
        else
        {
            numPad.SetActive(false);
        }

        if(cardDealer.CurrentPlayer==0)
        {
            turnos.text = "!Es tu turno!";
        }
        else if(cardDealer.CurrentPlayer ==1)
        {
            turnos.text = "!Es el turno del Manolo!";
        }
        else if (cardDealer.CurrentPlayer == 2)
        {
            turnos.text = "!Es el turno del otro Manolo!";
        }

        if(cardDealer.aPlayerRanOutOfCards)
        {
            EndGame();
        }
    }


    public IEnumerator Mentiroso()
    {
        GetPlayersInvolveInAccusing();
        mentiroso.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5F);
        mentiroso.gameObject.SetActive(false);
    }

    void GetPlayersInvolveInAccusing()
    {
        if(playerID ==0)
        {
            currentAcuser = "Tu";
        }
        else if (playerID ==1)
        {
            currentAcuser = "Manolo";
        }
        else if (playerID == 2)
        {
            currentAcuser = "otro Manolo";
        }


        if(cardDealer.lastPlayer == 0)
        {
            players = "Ti";
        }
        else if(cardDealer.lastPlayer == 1)
        {
            players = "Manolo";
        }
        else if (cardDealer.lastPlayer == 2)
        {
            players = "otro Manolo";
        }


        if(playerID == 0)
        {
            quienAcusaAquien.text = currentAcuser + " acusas al " + players + " de";
        }
        else
        {
            quienAcusaAquien.text = currentAcuser + " acusa al " + players + " de";
        }
     }
     


    void EndGame()
    {
        if(playerIDWithNoCards ==0) //el jugador ha ganado
        {
            endGamePanel.SetActive(true);
            gameUI.SetActive(false);
            victory.SetActive(true);
            Time.timeScale = 0;
        }
        else //manolo ha ganado
        {
            endGamePanel.SetActive(true);
            gameUI.SetActive(false);
            defeat.SetActive(true);
            Time.timeScale = 0;
        }
    }




























    public void DeclaredNumber1()
    {
        cardDealer.cardDeclared = 1;
    }
    public void DeclaredNumber2()
    {
        cardDealer.cardDeclared = 2;
    }
    public void DeclaredNumber3()
    {
        cardDealer.cardDeclared = 3;
    }
    public void DeclaredNumber4()
    {
        cardDealer.cardDeclared = 4;
    }
    public void DeclaredNumber5()
    {
        cardDealer.cardDeclared = 5;
    }
    public void DeclaredNumber6()
    {
        cardDealer.cardDeclared = 6;
    }
    public void DeclaredNumber7()
    {
        cardDealer.cardDeclared = 7;
    }
    public void DeclaredNumber8()
    {
        cardDealer.cardDeclared = 8;
    }
    public void DeclaredNumber9()
    {
        cardDealer.cardDeclared = 9;
    }
    public void DeclaredNumber10()
    {
        cardDealer.cardDeclared = 10;
    }
    public void DeclaredNumber11()
    {
        cardDealer.cardDeclared = 11;
    }
    public void DeclaredNumber12()
    {
        cardDealer.cardDeclared = 12;
    }
}
