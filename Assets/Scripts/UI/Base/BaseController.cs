using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    [HideInInspector]
    public RootMenu root;

    public virtual void Activate()
    {
        gameObject.SetActive(true);
    }

    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
    }
}

public abstract class BaseController<T> : BaseController where T : UIView
{
    [SerializeField] protected T ui;

    public override void Activate()
    {
        base.Activate();
        ui.Show();
    }
}
