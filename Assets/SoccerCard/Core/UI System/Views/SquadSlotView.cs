using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SquadSlotView : MonoBehaviour
{
    public bool IsEmpty { get; private set; } = true;
    private CardView assignedCard;
    [SerializeField] private Image highlightImage;
    private UIManager uiManager;
    private SquadBuilderView squadBuilderView;
    private void Awake()
    {
        uiManager= ServiceLocator.Get<UIManager>();
        uiManager.GetView("SquadBuilderView", out var view);
        squadBuilderView = view as SquadBuilderView;
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (!IsEmpty)
            return;

        var cardView = eventData.pointerDrag?.GetComponent<CardView>();
        if (cardView != null)
        {
            AssignCard(cardView);
        }
        SetHighlight(false);
    }

    public void OnPointerEnter()
    {
        if (IsEmpty)
            SetHighlight(true);
    }

    public void OnPointerExit()
    {
        SetHighlight(false);
    }

    public void AssignCard(CardView cardView)
    {
        assignedCard = cardView;
        IsEmpty = false;
        cardView.transform.SetParent(transform);
        cardView.transform.localPosition = Vector3.zero;
        cardView.transform.localScale = Vector3.one;
        SetHighlight(false);
        
        squadBuilderView.OnSlotChanged();
    }

    public void RemoveCard()
    {
        assignedCard = null;
        IsEmpty = true;
        SetHighlight(false);
        squadBuilderView.OnSlotChanged();
    }

    private void SetHighlight(bool active)
    {
        if (highlightImage != null)
            highlightImage.enabled = active;
    }
}
