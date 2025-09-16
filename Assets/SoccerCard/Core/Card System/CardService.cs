using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;

public static class CardService
{
    public static async UniTask<List<PlayerCard>> FetchCardsAsync(string url)
    {
        using var request = UnityWebRequest.Get(url);
        await request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Failed to fetch cards: {request.error}");
            return null;
        }
        var json = request.downloadHandler.text;
        var wrapper = JsonUtility.FromJson<PlayerCardListWrapper>(WrapJson(json));
        return wrapper.cards;
    }

    // Helper to wrap array JSON for Unity's JsonUtility
    private static string WrapJson(string json)
    {
        return $"{{\"cards\":{json}}}";
    }

    [System.Serializable]
    private class PlayerCardListWrapper
    {
        public List<PlayerCard> cards;
    }
}
