using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

public class UIManager : MonoBehaviour, IBootItem
{
    public string DisplayName => "UIManager";
    public bool RequiresGameObjectInstance => true;
    private readonly Dictionary<string, View> views = new();
    public async UniTask Boot(CancellationToken ct)
    {
        ServiceLocator.Register(this);
        await UniTask.CompletedTask;
    }

    public async UniTask ShowViewAsync(string viewName)
    {
        if (views.ContainsKey(viewName))
        {
            views[viewName].Show();
            return;
        }
        var loader = ServiceLocator.Get<AddressableLoader>();
        var view = await loader.LoadAsset<View>(viewName);
        if (view != null)
        {
            views[viewName] = Instantiate(view, transform);
            views[viewName].Show();
        }
    }
    public void HideView(string viewName)
    {
        if (views.ContainsKey(viewName))
        {
            views[viewName].Hide();
        }
    }
}
