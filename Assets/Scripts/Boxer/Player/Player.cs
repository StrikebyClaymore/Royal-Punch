public class Player : Boxer
{
    protected internal PlayerAnimation animationSystem;

    protected override void Awake()
    {
        base.Awake();
        animationSystem = GetComponent<PlayerAnimation>();
        GameManager.Player = this;
    }

    private void Start()
    {
        base.ConnectActions();
    }
}