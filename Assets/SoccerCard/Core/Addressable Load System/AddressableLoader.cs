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
        // Register itself to ServiceLocator
        ServiceLocator.Register(this);
        await UniTask.CompletedTask;
    }

    public async UniTask<T> LoadAsset<T>(string address) where T : Object
    {
        // If T is a Component, load as GameObject and get component (no instantiate)
        if (typeof(Component).IsAssignableFrom(typeof(T)) && typeof(T) != typeof(GameObject))
        {
            AsyncOperationHandle<GameObject> goHandle = Addressables.LoadAssetAsync<GameObject>(address);
            await goHandle.ToUniTask();
            if (goHandle.Status == AsyncOperationStatus.Succeeded && goHandle.Result != null)
            {
                var comp = goHandle.Result.GetComponent<T>();
                if (comp != null)
                    return comp;
                Debug.LogError($"Component of type {typeof(T).Name} not found on prefab '{address}'.");
                return null;
            }
            Debug.LogError($"Failed to load prefab '{address}' for component type {typeof(T).Name}.");
            return null;
        }
        // Otherwise, load as T (GameObject, ScriptableObject, etc.)
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
        await handle.ToUniTask();
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError($"Failed to load addressable asset at '{address}' as type {typeof(T).Name}: {handle.OperationException?.Message}");
            return null;
        }
        return handle.Result;
    }
}