using Cysharp.Threading.Tasks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MatchResultView : View
{
    [SerializeField] private Button playAgainButton;

    [SerializeField] private TextMeshProUGUI LocalSumTxt;
    [SerializeField] private TextMeshProUGUI VisitorSumTxt;
    [SerializeField] private TextMeshProUGUI ResultTxt;

    [SerializeField] private GridLayoutGroup currentCardsContainer;
    private List<CardView> currentCardViews = new();
    private int localSum;
    private int visitorSum;

    public override void Show()
    {
        base.Show();
        playAgainButton.onClick.RemoveAllListeners();
        playAgainButton.onClick.AddListener(OnPlayAgainClicked); 
        CalculateResult().Forget();
    }

    private async UniTask CalculateResult()
    {
        await LoadSquad();
        await SetupResults();
    }

    private async UniTask SetupResults()
    {
        localSum = await AnimateLocalSum();
        visitorSum = await AnimateVisitorSum();
        ShowResult();
    }

    private void ShowResult()
    {
        if (localSum > visitorSum)
            ResultTxt.text = "<color=green>Victory!</color>";
        else if (localSum < visitorSum)
            ResultTxt.text = "<color=red>Lose!</color>";
        else
            ResultTxt.text = "<color=yellow>Draw!</color>";
    }

    private async UniTask LoadSquad()
    {
        var cardRepository = ServiceLocator.Get<CardRepository>();
        var loader = ServiceLocator.Get<AddressableLoader>();
        var cardViewPrefab = await loader.LoadAsset<CardView>("CardView");
        foreach (Transform child in currentCardsContainer.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var card in cardRepository.SelectedSquadCards)
        {
            var cardView = Instantiate(cardViewPrefab, currentCardsContainer.transform);
            var thumbnail = cardRepository.GetThumbnail(card.id);
            cardView.Setup(card, thumbnail, false);
            currentCardViews.Add(cardView);
        }
    }
    private void OnPlayAgainClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private async UniTask<int> AnimateLocalSum()
    {
        var cardRepository = ServiceLocator.Get<CardRepository>();
        int sum = 0;
        LocalSumTxt.text = "0";
        foreach (CardView card in currentCardViews)
        {
            int rating = card.GetPlayerCard().rating;

            await AnimateScale(card.transform, 1.2f, 0.15f);

            int targetSum = sum + rating;
            await AnimateSumText(sum, targetSum, LocalSumTxt, card.transform);
            sum = targetSum;
        }

        return sum;
    }

    private async UniTask<int> AnimateVisitorSum()
    {
        int minValue = 250;
        int maxValue = 300;
        float duration = 1.5f; 
        float elapsed = 0f;
        float delay = 0.03f; 
        int finalValue = UnityEngine.Random.Range(minValue, maxValue + 1);

        while (elapsed < duration)
        {
            int value = UnityEngine.Random.Range(minValue, maxValue + 1);
            VisitorSumTxt.text = value.ToString();
            float t = elapsed / duration;
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            delay = Mathf.Lerp(0.03f, 0.15f, t); 
            elapsed += delay;
        }

        int current = int.Parse(VisitorSumTxt.text);
        float settleDuration = 0.4f;
        float settleElapsed = 0f;
        while (settleElapsed < settleDuration)
        {
            float t = settleElapsed / settleDuration;
            int value = Mathf.RoundToInt(Mathf.Lerp(current, finalValue, t));
            VisitorSumTxt.text = value.ToString();
            float scale = Mathf.Lerp(1f, 1.3f, Mathf.Sin(t * Mathf.PI));
            VisitorSumTxt.transform.localScale = Vector3.one * scale;
            settleElapsed += Time.deltaTime;
            await UniTask.Yield();
        }
        VisitorSumTxt.text = finalValue.ToString();
        VisitorSumTxt.transform.localScale = Vector3.one;

        return finalValue;
    }

    private async UniTask AnimateSumText(int from, int to, TextMeshProUGUI sumText, Transform cardTransform)
    {
        float duration = 0.25f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            int value = Mathf.RoundToInt(Mathf.Lerp(from, to, t));
            sumText.text = value.ToString();
            
            float scale = Mathf.Lerp(1f, 1.3f, Mathf.Sin(t * Mathf.PI));
            sumText.transform.localScale = Vector3.one * scale;
            elapsed += Time.deltaTime;
            await UniTask.Yield();
        }
        sumText.text = to.ToString();
        sumText.transform.localScale = Vector3.one;
    }

    private async UniTask AnimateScale(Transform target, float scaleAmount, float duration)
    {
        float elapsed = 0f;
        Vector3 originalScale = target.localScale;
        Vector3 targetScale = originalScale * scaleAmount;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            target.localScale = Vector3.Lerp(originalScale, targetScale, Mathf.Sin(t * Mathf.PI));
            elapsed += Time.deltaTime;
            await UniTask.Yield();
        }
        target.localScale = originalScale;
    }
}
