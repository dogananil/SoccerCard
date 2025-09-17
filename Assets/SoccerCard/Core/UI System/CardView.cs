using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI ratingText;
    [SerializeField] private Image thumbnailImage;
    private bool isDraggingAvailable = false;

    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private Vector3 originalPosition;

    private SquadSlotView currentSlot;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void Setup(PlayerCard card, Sprite thumbnail, bool isDragAvailable = false)
    {
        playerNameText.text = card.name;
        ratingText.text = card.rating.ToString();
        thumbnailImage.sprite = thumbnail;
        isDraggingAvailable = isDragAvailable;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDraggingAvailable) return;
        if (currentSlot != null)
        {
            currentSlot.RemoveCard();
            currentSlot = null;
        }
        var uIManager = ServiceLocator.Get<UIManager>();
        uIManager.GetView("SquadBuilderView", out var view);

        originalParent = (view as SquadBuilderView).CardPackParent;
        originalPosition = transform.localPosition;
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(view.transform); 
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggingAvailable) return;
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggingAvailable) return;
        canvasGroup.blocksRaycasts = true;


        var slot = eventData.pointerEnter?.GetComponent<SquadSlotView>();
        if (slot != null && slot.IsEmpty)
        {
            transform.SetParent(slot.transform);
            transform.localPosition = Vector3.zero;
            slot.AssignCard(this);
            currentSlot = slot;
        }
        else
        {
            transform.SetParent(originalParent);
            transform.localPosition = originalPosition;
        }
    }
}
