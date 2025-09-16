using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;

public class CardSystemController : IBootItem
{
    public string DisplayName => "CardSystemController";
    public bool RequiresGameObjectInstance => false;
    private static string CardsJsonUrl => Application.streamingAssetsPath + "/cards.json";

    public async UniTask Boot(CancellationToken ct)
    {
        var cardList = await CardService.FetchCardsAsync(CardsJsonUrl);
        if (cardList == null)
        {
            Debug.LogError("CardSystemController: Failed to fetch card data.");
            return;
        }
        var repository = new CardRepository();
        await repository.InitializeAsync(cardList);
        ServiceLocator.Register(repository);
    }
}
