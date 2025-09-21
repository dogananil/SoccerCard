using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardPackOpenningView : View
{
    [SerializeField] private Image cardPackImage;
    [SerializeField] private GridLayoutGroup cardListContainer;
    [SerializeField] private Button nextButton;
    private List<CardView> cardViews = new();
    private CardPackOpenAnimationConfig animationConfig;
    [SerializeField] private Animator openPackAnimator;
    [SerializeField] private Animator revealItemsAnimator;
    private List<PlayerCard> openedCards = new();

    private void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
    public override void Show()
    {
        base.Show();
        //cardPackImage.gameObject.SetActive(true);//OLD ANIMATION code
        //cardListContainer.gameObject.SetActive(false);//OLD ANIMATION code
        PlayRevealAnim();
        //LoadConfigAndPlayAnimation().Forget();//OLD ANIMATION
    }

    private void PlayRevealAnim()
    {
        LoadCards().Forget();
        openPackAnimator.SetTrigger("OpenPack");
        openPackAnimator.SetTrigger("RevealCard");
    }

    private async UniTask LoadConfigAndPlayAnimation()
    {
        var loader = ServiceLocator.Get<AddressableLoader>();
        animationConfig = await loader.LoadAsset<CardPackOpenAnimationConfig>("CardPackOpenAnimationConfig");
        await PlayFlipAnimation();
    }

    private async UniTask PlayFlipAnimation()
    {
        float duration = animationConfig.flipDuration;
        int flipCount = animationConfig.flipCount;
        for (int i = 0; i < flipCount; i++)
        {
            await FlipOnce(cardPackImage.transform, duration);
            duration *= animationConfig.flipSpeedup;
        }

        await Explode(cardPackImage.transform);
        cardPackImage.gameObject.SetActive(false);
        cardListContainer.gameObject.SetActive(true);
        await LoadCards();
    }

    private async UniTask FlipOnce(Transform target, float duration)
    {
        float elapsed = 0f;
        Vector3 start = target.localScale;
        Vector3 end = new Vector3(-start.x, start.y, start.z);
        while (elapsed < duration)
        {
            target.localScale = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            await UniTask.Yield();
        }
        target.localScale = end;
    }

    private async UniTask Explode(Transform target)
    {
        float duration = animationConfig.explodeDuration;
        float elapsed = 0f;
        Vector3 startScale = target.localScale;
        Vector3 endScale = startScale * animationConfig.explodeScale;
        Vector3 originalPos = target.localPosition;

        var image = target.GetComponent<Image>();
        Color startColor = image != null ? image.color : Color.white;
        Color explodeColor = animationConfig.explodeColor;
        explodeColor.a = 1f;

        float shakeMagnitude = animationConfig.shakeMagnitude;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            target.localScale = Vector3.Lerp(startScale, endScale, t);

            float shakeX = Random.Range(-1f, 1f) * shakeMagnitude * (1f - t);
            float shakeY = Random.Range(-1f, 1f) * shakeMagnitude * (1f - t);
            target.localPosition = originalPos + new Vector3(shakeX, shakeY, 0);

            if (image != null)
                image.color = Color.Lerp(startColor, explodeColor, t);

            elapsed += Time.deltaTime;
            await UniTask.Yield();
        }
        target.localScale = endScale;
        target.localPosition = originalPos;
        if (image != null)
            image.color = explodeColor;

        float fadeDuration = animationConfig.fadeDuration;
        elapsed = 0f;
        Color fadeColor = explodeColor;
        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            fadeColor.a = Mathf.Lerp(1f, 0f, t);
            if (image != null)
                image.color = fadeColor;
            elapsed += Time.deltaTime;
            await UniTask.Yield();
        }
        if (image != null)
            image.color = new Color(explodeColor.r, explodeColor.g, explodeColor.b, 0f);

        await UniTask.Delay(100);
        target.gameObject.SetActive(false);
    }

    private async UniTask LoadCards()
    {
        var loader = ServiceLocator.Get<AddressableLoader>();
        var cardViewPrefabGO = await loader.LoadAsset<GameObject>("CardView");
        var repository = ServiceLocator.Get<CardRepository>();
        foreach (Transform child in cardListContainer.transform)
            Destroy(child.gameObject);

        var allCards = new List<PlayerCard>(repository.Cards.Values);
        openedCards = allCards.OrderBy(x => Random.value).Take(5).ToList();
        repository.OpenedCards = openedCards;

        foreach (var card in openedCards)
        {
            GameObject cardViewObj = Instantiate(cardViewPrefabGO, cardListContainer.transform);
            var cardView = cardViewObj.GetComponent<CardView>();
            cardViews.Add(cardView);
            var thumbnail = repository.GetThumbnail(card.id);
            cardView.Setup(card, thumbnail);
        }

        nextButton.gameObject.SetActive(true);
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(OnNextClicked);
    }

    private void OnNextClicked()
    {
        nextButton.gameObject.SetActive(false);

        var uiManager = ServiceLocator.Get<UIManager>();
        uiManager.ShowViewAsync("SquadBuilderView").Forget();
        Hide();
    }
}
