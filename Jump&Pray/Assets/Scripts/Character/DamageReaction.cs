using UnityEngine;

public class DamageReaction : MonoBehaviour
{
    [SerializeField] private Renderer[] renderersToBlink;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float knockbackUpward = 2f;
    [SerializeField] private float invulnerabilityTime = 1f;
    [SerializeField] private float blinkSpeed = 0.1f;

    private Rigidbody rb;
    private bool isInvulnerable = false;

    private void Start()
    {
        rb = GameManager.Instance.GetPlayerController()?.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Player missing Rigidbody!");
        }

        EventManager.Instance.OnPlayerKicked += ReactToDamage;
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnPlayerKicked -= ReactToDamage;
    }

    private void ReactToDamage()
    {
        if (isInvulnerable) return;

        isInvulnerable = true;

        Vector3 knockbackDir = -transform.forward;
        knockbackDir.y = 0;
        knockbackDir.Normalize();
        knockbackDir += Vector3.up * knockbackUpward;

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(knockbackDir * knockbackForce, ForceMode.Impulse);

        StartCoroutine(InvulnerabilityCoroutine());
    }

    private System.Collections.IEnumerator InvulnerabilityCoroutine()
    {
        float timer = 0f;
        bool visible = true;

        while (timer < invulnerabilityTime)
        {
            visible = !visible;

            foreach (Renderer render in renderersToBlink)
            {
                render.enabled = visible;
            }

            yield return new WaitForSeconds(blinkSpeed);
            timer += blinkSpeed;
        }

        foreach (Renderer render in renderersToBlink)
        {
            render.enabled = true;
        }

        isInvulnerable = false;
    }
}
