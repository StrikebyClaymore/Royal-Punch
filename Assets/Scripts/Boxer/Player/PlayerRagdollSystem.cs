public class PlayerRagdollSystem : RagdollSystem
{
    private void Start()
    {
        gameCamera = GameManager.Camera;
        Toggle(false);
    }
}