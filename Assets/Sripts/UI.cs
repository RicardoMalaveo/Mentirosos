using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UIElements;
using NUnit.Framework;

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
    public Cinematic cinematic;


    private void Update()
    {
        if(cardDealer.cardDeclared !=0)
        {
            playingNumberText.text = "Declared Card Number: " + cardDealer.cardDeclared.ToString() + " x " + cardDealer.amountOfCardsPlayed.ToString() + " Card";
            amountOfCardsOnTheTable.text = "Current Cards in Play: " + cardDealer.totalAmountOfCardsInThePile.ToString(); ;
        }
        else
        {
            playingNumberText.text = "Declared Card Number: No Declared";
            amountOfCardsOnTheTable.text = "Current Cards in Play: No Cards";
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
            turnos.text = "Your Turn!";
        }
        else if(cardDealer.CurrentPlayer ==1)
        {
            turnos.text = "First Manolo's Turn";
        }
        else if (cardDealer.CurrentPlayer == 2)
        {
            turnos.text = "Second Manolo's Turn";
        }

        if(cardDealer.aPlayerRanOutOfCards)
        {
            EndGame();
        }
    }


    public IEnumerator Mentiroso()
    {
        AudioManager.Instance.PlaySFX("Mentiroso");
        GetPlayersInvolveInAccusing();
        mentiroso.gameObject.SetActive(true);
        //Hay que ajustar la duración de la cinematica a la duración de la acusación
        //cinematic.ActivarCinematica();
        yield return new WaitForSeconds(1F); //Habrá que cambiarlo aquí pero manolo sigue jugando 
        mentiroso.gameObject.SetActive(false);
    }

    void GetPlayersInvolveInAccusing()
    {
        if(playerID ==0)
        {
            currentAcuser = "You";
        }
        else if (playerID ==1)
        {
            currentAcuser = "First Manolo";
        }
        else if (playerID == 2)
        {
            currentAcuser = "Second Manolo";
        }


        if(cardDealer.lastPlayer == 0)
        {
            players = "you";
        }
        else if(cardDealer.lastPlayer == 1)
        {
            players = "first Manolo";
        }
        else if (cardDealer.lastPlayer == 2)
        {
            players = "second Manolo";
        }


        if(playerID == 0)
        {
            quienAcusaAquien.text = currentAcuser + " acuse to " + players + " of";
        }
        else
        {
            quienAcusaAquien.text = currentAcuser + " acuses to " + players + " of";
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

    private bool gamePaused = false;
    public void TogglePause()
    {
        if (gamePaused == false)
        {
            Time.timeScale = 0f;
            gamePaused = true;
        }
        else
        {
            Time.timeScale = 1f;
            gamePaused = false;
        }
    }
}
