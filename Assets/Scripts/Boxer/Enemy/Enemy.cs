public class Enemy : Boxer
{
    protected override void Awake()
    {
        base.Awake();
        GameManager.Enemy = this;
    }

    private void Start()
    {
        base.ConnectActions();
    }
}