using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI ratingText;
    [SerializeField] private Image thumbnailImage;

    public void Setup(PlayerCard card, Sprite thumbnail)
    {
        playerNameText.text = card.name;
        ratingText.text = card.rating.ToString();
        thumbnailImage.sprite = thumbnail;
    }
}
