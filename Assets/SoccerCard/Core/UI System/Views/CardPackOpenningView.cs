using Cysharp.Threading.Tasks;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPackOpenningView : View
{
    [SerializeField]private VerticalLayoutGroup cardListContainer;
    [SerializeField] private Transform parent;
    private List<CardView> cardViews = new();

    public override void Show()
    {
        base.Show();
        cardListContainer.gameObject.SetActive(true);
        LoadCards().Forget();
    }
    private async UniTask LoadCards()
    {
        // Load cards into the cardListContainer
        var loader = ServiceLocator.Get<AddressableLoader>();
        var cardViewPrefabGO = await loader.LoadAsset<GameObject>("CardView");
        var repository = ServiceLocator.Get<CardRepository>();
        foreach (Transform child in cardListContainer.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var card in repository.Cards.Values)
        {
            GameObject cardViewObj = Instantiate(cardViewPrefabGO, cardListContainer.gameObject.transform);
            cardViews.Add(cardViewObj.GetComponent<CardView>());
            var cardView = cardViewObj.GetComponent<CardView>();
            var thumbnail = repository.GetThumbnail(card.id);
            cardView.Setup(card, thumbnail);
        }
    }
}
