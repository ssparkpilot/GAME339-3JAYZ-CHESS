using UnityEngine;
using TMPro;

public class PieceHealthUI : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private TMP_Text healthText;

    private void Awake()
    {
        if (health == null)
            health = GetComponentInParent<Health>();

        if (healthText == null)
            healthText = GetComponentInChildren<TMP_Text>();
    }

    private void OnEnable()
    {
        if (health != null)
        {
            health.OnHealthChanged += UpdateHealth;
            UpdateHealth(health.CurrentHP); // initialize
        }
    }

    private void OnDisable()
    {
        if (health != null)
            health.OnHealthChanged -= UpdateHealth;
    }

    private void UpdateHealth(int hp)
    {
        if (healthText != null)
            healthText.text = "HP: " + hp;
    }
}