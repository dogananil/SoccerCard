using UnityEngine;

public class App : MonoBehaviour
{
    public static App Instance { get; private set; }
    public static AddressableLoader AddressableLoader { get; set; }
    public static UIManager UIManager { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
