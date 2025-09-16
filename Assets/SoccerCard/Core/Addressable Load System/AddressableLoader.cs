using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableLoader : IBootItem
{
    public string DisplayName => "AddressableLoader";
    public bool RequiresGameObjectInstance => false;
    public async UniTask Boot(CancellationToken ct)
    {
        App.AddressableLoader = this;
        await UniTask.CompletedTask;
    }
    // Loads any addressable asset by address and type, with error handling
    public async UniTask<T> LoadAsset<T>(string address) where T : Object
    {
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
        await handle.ToUniTask();
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError($"Failed to load addressable asset at '{address}': {handle.OperationException?.Message}");
            return null;
        }
        return handle.Result;
    }
}