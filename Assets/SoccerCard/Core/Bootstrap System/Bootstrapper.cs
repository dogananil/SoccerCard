using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Reflection;

public class Bootstrapper : MonoBehaviour
{
    private List<IBootItem> bootItems = new List<IBootItem>();

    private void Awake()
    {
        AddBootItemToList(typeof(AddressableLoader));
        AddBootItemToList(typeof(UIManager));
        AddBootItemToList(typeof(CardSystemController));
        StartBoot().Forget();
    }

    private void AddBootItemToList(Type bootItemType)
    {
        // Create IBootItem instance
        var dummy = Activator.CreateInstance(bootItemType) as IBootItem;
        if (dummy == null)
            return;
        if (dummy.RequiresGameObjectInstance)
        {
            var go = new GameObject(bootItemType.Name);
            var comp = go.AddComponent(bootItemType) as IBootItem;
            if (comp != null)
                bootItems.Add(comp);
        }
        else
        {
            bootItems.Add(dummy);
        }
    }

    private void RegisterSystem(IBootItem item)
    {
        var type = item.GetType();
        var method = typeof(ServiceLocator).GetMethod("Register");
        var generic = method.MakeGenericMethod(type);
        generic.Invoke(null, new object[] { item });
    }

    private async UniTask StartBoot()
    {
        var ct = new CancellationToken();
        foreach (var item in bootItems)
        {
            Debug.Log($"Booting: {item.DisplayName}");
            await item.Boot(ct);
            RegisterSystem(item);
        }
        var uiManager = ServiceLocator.Get<UIManager>();
        uiManager.ShowViewAsync("MainMenuView").Forget();
    }
}



