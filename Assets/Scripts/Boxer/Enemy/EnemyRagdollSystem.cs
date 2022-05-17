public class EnemyRagdollSystem : RagdollSystem
{
    private void Start()
    {
        gameCamera = GameManager.Camera;
        Toggle(false);
    }
}