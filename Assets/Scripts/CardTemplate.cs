using UnityEngine;

[CreateAssetMenu(fileName = "New Card Template", menuName = "Card Template")]
public class CardTemplate : ScriptableObject
{
    public string cardName;
    public Sprite cardImage;
    public int cardValue;
    [TextArea(3, 10)]
    public string cardDescription;
    public CardTemplate() { }
    public void SetDataFromCardData(CardData cardData)
    {
        cardName = cardData.cardName;
        cardValue = cardData.cardValue;
        cardDescription = cardData.cardDescription;
    }
}
