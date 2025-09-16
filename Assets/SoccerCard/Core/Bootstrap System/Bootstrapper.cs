using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using System;

public class Bootstrapper : MonoBehaviour
{
    private List<IBootItem> bootItems = new List<IBootItem>();

    private void Awake()
    {
        AddBootItemToList(typeof(AddressableLoader));
        AddBootItemToList(typeof(UIManager));
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

    private async UniTask StartBoot()
    {
        var ct = new CancellationToken();
        foreach (var item in bootItems)
        {
            Debug.Log($"Booting: {item.DisplayName}");
            await item.Boot(ct);
        }
    }
}



