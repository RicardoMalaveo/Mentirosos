using UnityEngine;
using System.Collections;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] private CardDealer cardDealer;
    [SerializeField] private PlayerController mainPlayer;
    public TextMeshProUGUI playingNumberText;
    public TextMeshProUGUI amountOfCardsOnTheTable;
    public TextMeshProUGUI mentiroso;
    public GameObject numPad;
    private void accusar()
    {
        cardDealer.GetGamePileToLiar(mainPlayer.controlledPlayer.playerID);
    }


    private void Update()
    {
        if(cardDealer.cardDeclared !=0)
        {
            playingNumberText.text = "Número de carta declarado: " + cardDealer.cardDeclared.ToString() + " x " + cardDealer.amountOfCardsPlayed.ToString();
            amountOfCardsOnTheTable.text = "Cantidad de cartas jugadas: " + cardDealer.totalAmountOfCardsInThePile.ToString(); ;
        }
        else
        {
            playingNumberText.text = "Número de carta declarado: No se ha declarado ninguna carta";
            amountOfCardsOnTheTable.text = "Cartas en juego: no hay cartas en juego";
        }


        if(cardDealer.CurrentPlayer==0)
        {
            numPad.SetActive(true);
        }
        else
        {
            numPad.SetActive(false);
        }
    }


    public IEnumerator Mentiroso()
    {
        mentiroso.gameObject.SetActive(true);
        yield return new WaitForSeconds(1F);
        mentiroso.gameObject.SetActive(false);
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
