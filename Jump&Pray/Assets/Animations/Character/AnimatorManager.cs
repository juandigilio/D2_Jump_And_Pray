using UnityEngine;


public class AnimatorManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;


    private void OnEnable()
    {
        EventManager.Instance.OnPlayerJumped += AnimateJump;
        EventManager.Instance.OnPlayerDied += AnimateDeath;
        EventManager.Instance.OnPlayerLanded += AnimateLand;
        EventManager.Instance.OnPlayerRolled += AnimateRoll;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnPlayerJumped -= AnimateJump;
        EventManager.Instance.OnPlayerDied -= AnimateDeath;
        EventManager.Instance.OnPlayerLanded -= AnimateLand;
        EventManager.Instance.OnPlayerRolled -= AnimateRoll;
    }

    private void Update()
    {
        UpdateParameters();
    }

    private void UpdateParameters()
    {
        Vector3 velocity = playerController.GetVelocity();

        animator.SetFloat("verticalVelocity", velocity.y);

        velocity.y = 0;
        animator.SetFloat("horizontalVelocity", velocity.magnitude);

        animator.SetBool("isGrounded", playerController.IsGrounded());
    }

    private void AnimateJump()
    {
        animator.SetTrigger("jumped");
    }

    private void AnimateDeath()
    {
        animator.SetTrigger("hasDied");
    }

    private void AnimateLand()
    {
        animator.SetTrigger("landed");
    }

    private void AnimateRoll()
    {
        animator.SetTrigger("rolled");
    }
}
