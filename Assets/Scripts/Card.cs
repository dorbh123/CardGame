using UnityEngine;

public class Card : MonoBehaviour
{
    public CardTemplate cardTemplate;

    // You can access the properties of the CardTemplate like this:
    private void Start()
    {
        Debug.Log("Card Name: " + cardTemplate.cardName);
        Debug.Log("Card Value: " + cardTemplate.cardValue);
        Debug.Log("Card Description: " + cardTemplate.cardDescription);
        // You can also access the image using cardTemplate.cardImage
    }
}

