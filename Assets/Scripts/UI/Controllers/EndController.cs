public class EndController : BaseController<EndView>
{
    private void Start()
    {
        ConnectActions();
    }

    public override void Activate()
    {
        base.Activate();
        ui.UpdateCoins(GameManager.AddCoins());
    }

    private void Claim()
    {
        GameManager.EndBattle();
    }
    
    private void ConnectActions()
    {
        ui.OnClaimPressed += Claim;
    }
}