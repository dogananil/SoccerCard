using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI ratingText;
    [SerializeField] private Image thumbnailImage;
    private bool isDraggingAvailable = false;

    public void Setup(PlayerCard card, Sprite thumbnail,bool isDragAvailable=false)
    {
        playerNameText.text = card.name;
        ratingText.text = card.rating.ToString();
        thumbnailImage.sprite = thumbnail;
        isDraggingAvailable = isDragAvailable;
    }

    public void OnBeginDrag(PointerEventData eventData) {  }
    public void OnDrag(PointerEventData eventData) { }
    public void OnEndDrag(PointerEventData eventData) { }
}
