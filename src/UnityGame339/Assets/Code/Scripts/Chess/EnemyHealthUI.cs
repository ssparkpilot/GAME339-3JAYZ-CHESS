using UnityEngine;

public class PieceHealthUI : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private RectTransform fillRect;

    private int maxHP;
    private float fullWidth = 80f;

    private void Awake()
    {
        if (health == null)
            health = GetComponentInParent<Health>();

        maxHP = health.CurrentHP;
    }

    private void OnEnable()
    {
        if (health != null)
        {
            health.OnHealthChanged += UpdateHealth;
            UpdateHealth(health.CurrentHP);
        }
    }

    private void OnDisable()
    {
        if (health != null)
            health.OnHealthChanged -= UpdateHealth;
    }

    private void UpdateHealth(int currentHP)
    {
        float percent = (float)currentHP / maxHP;

        // Update width
        if (fillRect != null)
        {
            Vector2 size = fillRect.sizeDelta;
            size.x = fullWidth * percent;
            fillRect.sizeDelta = size;
        }
    }
}