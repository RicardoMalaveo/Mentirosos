using System.Collections.Generic;
using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [System.Serializable]
    public class CardInfo
    {
        public string cardName;           // Nombre único de la carta (por ejemplo: "7 de Copas")
        public int number;                // Número de la carta (1 al 12, etc.)
        public string suit;               // Palo (Copas, Espadas, etc.)
        public GameObject prefab;         // Prefab asociado a esta carta
        public bool isSpecial = false;    // Marca si esta carta es especial
    }

    [Header("Lista completa de cartas del juego")]
    public List<CardInfo> allCards = new List<CardInfo>(); // Aquí defines manualmente todas las cartas desde el Inspector
}