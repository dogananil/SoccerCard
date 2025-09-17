using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : View
{
    [SerializeField] Button openPackButton;

    private void Awake()
    {
        openPackButton.onClick.AddListener(OnOpenPackClicked);
    }

    private void OnOpenPackClicked()
    {
        var uiManager = ServiceLocator.Get<UIManager>();
        uiManager.ShowViewAsync("CardPackOpenningView").Forget();
        Hide();
    }
}
