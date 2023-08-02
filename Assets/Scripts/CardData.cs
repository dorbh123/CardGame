using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardData
{
    public string cardName;
    public int cardValue;
    public string cardDescription;
    public Vector2Int gridPosition; // New property

    public CardData(CardTemplate cardTemplate, Vector2Int gridPosition)
    {
        cardName = cardTemplate.cardName;
        cardValue = cardTemplate.cardValue;
        cardDescription = cardTemplate.cardDescription;
        this.gridPosition = gridPosition; // Save the grid position
    }

    // Rest of the class remains the same...
}



[System.Serializable]
public class CardDataListWrapper
{
    public List<CardData> cardDataList;

    public CardDataListWrapper(List<CardData> cardDataList)
    {
        this.cardDataList = cardDataList;
    }

    // Default constructor required for deserialization
    public CardDataListWrapper() { }
}
