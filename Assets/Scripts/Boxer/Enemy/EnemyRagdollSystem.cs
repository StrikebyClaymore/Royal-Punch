public class EnemyRagdollSystem : RagdollSystem
{
    private void Start()
    {
        gameCamera = GameManager.Camera;
        Toggle(false);
    }
    
    public override void KnockOut(float force, bool standUp = false)
    {
        character.enabled = false;
        animation.StopPunch();
        Toggle(true);
        animation.Toggle(false);
        chestRb.AddForce(-transform.forward * force);
        if(standUp == false)
            StartCoroutine(GameManager.Player.Win());
    }
}