using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    
    [SerializeField] private Animator animator;
    [SerializeField] private string triggerName = "Entry";

    public void SetAnimation(string animName)
    {
        animator.SetTrigger(animName);
    }
}
