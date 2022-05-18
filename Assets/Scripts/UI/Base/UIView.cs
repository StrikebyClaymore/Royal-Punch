using UnityEngine;

public abstract class UIView : MonoBehaviour
{
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
