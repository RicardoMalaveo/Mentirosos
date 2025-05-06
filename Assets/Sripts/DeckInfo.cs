using System.Collections.Generic;
using System;
using UnityEngine;

public class DeckInfo : MonoBehaviour
{
    [System.Serializable]
    public class CardInfo
    {
        public string cardName;
        public int number;
        public string suit; 
        public GameObject prefab;
        public bool isSpecial = false;
    }

    [Header("All Cards Info Database")]
    public List<CardInfo> allCards = new List<CardInfo>(); //Aquí defino manualmente todas las cartas desde el Inspector
}