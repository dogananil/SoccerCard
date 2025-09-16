using UnityEngine;

public class View : MonoBehaviour
{
    public string ViewName;

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
