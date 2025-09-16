using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class CardRepository
{
    private readonly Dictionary<string, PlayerCard> cards = new();
    private readonly Dictionary<string, Sprite> thumbnails = new();
    public List<PlayerCard> OpenedCards { get; set; } = new();

    public IReadOnlyDictionary<string, PlayerCard> Cards => cards;
    public IReadOnlyDictionary<string, Sprite> Thumbnails => thumbnails;

    public async UniTask InitializeAsync(List<PlayerCard> cardList)
    {
        cards.Clear();
        thumbnails.Clear();
        foreach (var card in cardList)
        {
            cards[card.id] = card;
            var sprite = await DownloadThumbnail(card.thumbnail);
            thumbnails[card.id] = sprite;
        }
    }

    private async UniTask<Sprite> DownloadThumbnail(string url)
    {
        using var www = new UnityEngine.Networking.UnityWebRequest(url);
        www.downloadHandler = new UnityEngine.Networking.DownloadHandlerTexture();
        await www.SendWebRequest();
        if (www.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
        {
            var texture = ((UnityEngine.Networking.DownloadHandlerTexture)www.downloadHandler).texture;
            return Sprite.Create(texture, new Rect(0,0,texture.width,texture.height), new Vector2(0.5f,0.5f));
        }
        return null;
    }

    public Sprite GetThumbnail(string cardId)
    {
        thumbnails.TryGetValue(cardId, out var sprite);
        return sprite;
    }

    public PlayerCard GetCard(string cardId)
    {
        cards.TryGetValue(cardId, out var card);
        return card;
    }
}
