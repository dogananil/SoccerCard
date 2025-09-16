using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using System.Threading;

public class Bootstrapper : MonoBehaviour
{
    private List<IBootItem> bootItems = new List<IBootItem>();

    private void Awake()
    {
        // Example: Add systems to boot in order
        bootItems.Add(new AddressableLoader());
        // Add more boot items here as needed
        StartBoot().Forget();
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



