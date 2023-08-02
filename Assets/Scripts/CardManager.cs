using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class CardManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public CardTemplate[] allCardTemplates;
    public Transform canvasTransform;
    private List<CardTemplate> availableCardTemplates = new List<CardTemplate>();

    public Vector2 cardSpacing = new Vector2(100f, 0f);
    public int cardsPerRow = 4;

    private List<Vector2Int> availableGridPositions = new List<Vector2Int>();

    void Start()
    {
        // Initialize availableCardTemplates with allCardTemplates
        availableCardTemplates.AddRange(allCardTemplates);

        // Generate all possible grid positions
        for (int y = 0; y < Mathf.CeilToInt(allCardTemplates.Length / (float)cardsPerRow); y++)
        {
            for (int x = 0; x < cardsPerRow; x++)
            {
                availableGridPositions.Add(new Vector2Int(x, y));
            }
        }

        // Draw 7 cards at the start of the game
        for (int i = 0; i < 7; i++)
        {
            DrawCard();
        }
    }

    public void DrawCard()
    {
        if (availableCardTemplates.Count > 0 && availableGridPositions.Count > 0)
        {
            // Get a random index within the availableCardTemplates list
            int randomIndex = Random.Range(0, availableCardTemplates.Count);

            // Get the CardTemplate at the random index
            CardTemplate drawnCardTemplate = availableCardTemplates[randomIndex];

            // Get a random grid position from the availableGridPositions
            int randomGridIndex = Random.Range(0, availableGridPositions.Count);
            Vector2Int gridPosition = availableGridPositions[randomGridIndex];

            // Remove the used grid position from the availableGridPositions
            availableGridPositions.RemoveAt(randomGridIndex);

            // Instantiate a new card using the drawn CardTemplate and the generated grid position
            CreateCard(drawnCardTemplate, gridPosition);

            // Remove the drawn card from the available options
            availableCardTemplates.RemoveAt(randomIndex);
        }
    }
    private void CreateCard(CardTemplate cardTemplate, Vector2Int gridPosition)
    {
        GameObject newCard = Instantiate(cardPrefab, canvasTransform);
        Card cardComponent = newCard.GetComponent<Card>();
        cardComponent.cardTemplate = cardTemplate;

        SpriteRenderer spriteRenderer = newCard.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && cardTemplate.cardImage != null)
        {
            spriteRenderer.sprite = cardTemplate.cardImage;
        }

        newCard.transform.localPosition = new Vector3(gridPosition.x * cardSpacing.x, -gridPosition.y * cardSpacing.y, 0f);
    }




    public void SaveGameState()
    {
        // Create a list to hold card data
        List<CardData> cardDataList = new List<CardData>();

        // Iterate through all the drawn cards and collect their data
        foreach (Card drawnCard in canvasTransform.gameObject.GetComponentsInChildren<Card>())
        {
            CardData cardData = new CardData(drawnCard.cardTemplate, GetGridPosition(drawnCard.gameObject));
            cardDataList.Add(cardData);
        }

        // Serialize the list to JSON
        string json = JsonUtility.ToJson(new CardDataListWrapper(cardDataList));

        // Save the JSON data to PlayerPrefs
        PlayerPrefs.SetString("savedGameData", json);
        Debug.Log("Game saved.");
    }


    public void LoadGameState()
    {
        // Get the saved JSON data from PlayerPrefs
        string json = PlayerPrefs.GetString("savedGameData", string.Empty);

        // Deserialize the JSON data to get the list of card data
        CardDataListWrapper wrapper = JsonUtility.FromJson<CardDataListWrapper>(json);

        // Clear the canvas to remove any existing cards
        ClearDrawnCards();

        // Instantiate the cards based on the loaded data
        foreach (CardData cardData in wrapper.cardDataList)
        {
            CardTemplate template = allCardTemplates.FirstOrDefault(x => x.cardName == cardData.cardName);
            if (template != null)
            {
                template.SetDataFromCardData(cardData);
                CreateCard(template, cardData.gridPosition);
                availableCardTemplates.Remove(template); // Remove the template from the availableCardTemplates list
            }
            else
            {
                Debug.LogWarning("Card template not found for: " + cardData.cardName);
            }
        }
    }




    private Vector2Int GetGridPosition(GameObject cardObject)
    {
        int colIndex = Mathf.RoundToInt(cardObject.transform.localPosition.x / cardSpacing.x);
        int rowIndex = Mathf.RoundToInt(-cardObject.transform.localPosition.y / cardSpacing.y);
        return new Vector2Int(colIndex, rowIndex);
    }

    private void ClearDrawnCards()
    {
        foreach (Card drawnCard in canvasTransform.gameObject.GetComponentsInChildren<Card>())
        {
            Destroy(drawnCard.gameObject);
        }

        availableGridPositions.Clear();
        availableCardTemplates.Clear(); 
                                        // Regenerate all possible grid positions
        for (int y = 0; y < Mathf.CeilToInt(allCardTemplates.Length / (float)cardsPerRow); y++)
        {
            for (int x = 0; x < cardsPerRow; x++)
            {
                availableGridPositions.Add(new Vector2Int(x, y));
            }
        }

    }
   
}
