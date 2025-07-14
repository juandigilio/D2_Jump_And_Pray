using UnityEngine;

public class DamageReaction : MonoBehaviour
{
    [SerializeField] private Renderer[] renderersToBlink;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float knockbackUpward = 2f;
    [SerializeField] private float invulnerabilityTime = 1f;
    [SerializeField] private float blinkSpeed = 0.1f;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float colorBlendIntensity = 0.7f;

    private Rigidbody rb;
    private bool isInvulnerable = false;
    private Material[] originalMaterials;
    private Color[] originalColors;

    private void Start()
    {
        rb = GameManager.Instance.GetPlayerController()?.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Player missing Rigidbody!");
        }

        StoreMaterials();

        EventManager.Instance.OnPlayerKicked += ReactToDamage;
    }

    private void StoreMaterials()
    {
        originalMaterials = new Material[renderersToBlink.Length];
        originalColors = new Color[renderersToBlink.Length];

        for (int i = 0; i < renderersToBlink.Length; i++)
        {
            if (renderersToBlink[i] != null)
            {
                originalMaterials[i] = renderersToBlink[i].material;
                originalColors[i] = originalMaterials[i].color;
            }
        }
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

            for (int i = 0; i < renderersToBlink.Length; i++)
            {
                if (renderersToBlink[i] != null)
                {
                    renderersToBlink[i].enabled = visible;
                    
                    if (visible)
                    {
                        float blendFactor = Mathf.Sin((timer / blinkSpeed) * Mathf.PI) * 0.5f + 0.5f;
                        
                        Color blendedColor = Color.Lerp(originalColors[i], damageColor, blendFactor * colorBlendIntensity);
                        renderersToBlink[i].material.color = blendedColor;
                    }
                }
            }

            yield return new WaitForSeconds(blinkSpeed);
            timer += blinkSpeed;
        }

        for (int i = 0; i < renderersToBlink.Length; i++)
        {
            if (renderersToBlink[i] != null)
            {
                renderersToBlink[i].enabled = true;
                renderersToBlink[i].material.color = originalColors[i];
            }
        }

        isInvulnerable = false;
    }
}
