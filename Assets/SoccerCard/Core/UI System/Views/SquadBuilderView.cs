using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquadBuilderView : View
{
    [SerializeField] private GridLayoutGroup openedCardsContainer;
    [SerializeField] private GridLayoutGroup squadSlotsContainer;
    public Transform SlotParent=> squadSlotsContainer.transform;
    public Transform CardPackParent=> openedCardsContainer.transform;
    [SerializeField] private int squadSlotCount = 3;
    private List<SquadSlotView> squadSlots = new();

    public override void Show()
    {
        base.Show();
        LoadOpenedCards().Forget();
        ShowSquadSlots();
    }

    private async UniTask ShowSquadSlots()
    {
        foreach (Transform child in squadSlotsContainer.transform)
            Destroy(child.gameObject);
        var cardRepository = ServiceLocator.Get<CardRepository>();
        var loader = ServiceLocator.Get<AddressableLoader>();
        var squadSlotPrefab = await loader.LoadAsset<SquadSlotView>("SquadSlotView");
        squadSlots.Clear();
        for (int i = 0; i < squadSlotCount; i++)
        {
            var slotObj = Instantiate(squadSlotPrefab, squadSlotsContainer.transform);
            squadSlots.Add(slotObj);
        }
    }

    private async UniTask LoadOpenedCards()
    {
        var cardRepository = ServiceLocator.Get<CardRepository>();
        var loader = ServiceLocator.Get<AddressableLoader>();
        var cardViewPrefab = await loader.LoadAsset<CardView>("CardView");
        foreach (Transform child in openedCardsContainer.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var card in cardRepository.OpenedCards)
        {
            var cardView = Instantiate(cardViewPrefab, openedCardsContainer.transform);
            var thumbnail = cardRepository.GetThumbnail(card.id);
            cardView.Setup(card, thumbnail,true);
        }
    }
}
