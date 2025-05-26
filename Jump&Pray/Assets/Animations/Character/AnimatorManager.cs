using UnityEngine;


public class AnimatorManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;


    private void Start()
    {
        EventManager.Instance.OnPlayerJumped += AnimateJump;
    }

    private void Update()
    {
        UpdateParameters();
    }

    private void UpdateParameters()
    {
        Vector3 horizontalVelocity = playerController.GetVelocity();
        horizontalVelocity.y = 0;
        animator.SetFloat("horizontalVelocity", horizontalVelocity.magnitude);
    }

    private void AnimateJump()
    {
        animator.SetTrigger("jumped");
    }
}
