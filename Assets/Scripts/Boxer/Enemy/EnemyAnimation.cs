public class EnemyAnimation : BaseAnimation
{
    private const string Super = "Super_";

    public void StartSuper(int attackNumber)
    {
        animator.Play(Super+attackNumber);
        //animator.SetTrigger(Super+attackNumber);   
    }
}