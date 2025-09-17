using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SquadSlotView : MonoBehaviour
{
    public bool IsEmpty { get; private set; } = true;
    private CardView assignedCard;
    [SerializeField] private Image highlightImage;

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
        Debug.LogError($"Pointer Entered Slot {IsEmpty}");
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
    }

    public void RemoveCard()
    {
        assignedCard = null;
        IsEmpty = true;
        SetHighlight(false);
    }

    private void SetHighlight(bool active)
    {
        if (highlightImage != null)
            highlightImage.enabled = active;
    }
}
