using UnityEngine;
using TMPro;

public class HoverHealthUI : MonoBehaviour
{
    public static HoverHealthUI main;

    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text healthText;

    private RectTransform panelRect;
    private Health currentTarget;

    private void Awake()
    {
        main = this;

        if (panel != null)
        {
            panelRect = panel.GetComponent<RectTransform>();
            panel.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        if (currentTarget == null)
        {
            if (panel != null && panel.activeSelf)
                panel.SetActive(false);
            return;
        }

        UpdatePosition();
    }

    public void Show(Health target)
    {
        if (panel == null || healthText == null || target == null)
            return;

        if (currentTarget != null && currentTarget != target)
        {
            Unsubscribe(currentTarget);
        }

        currentTarget = target;

        Subscribe(currentTarget);

        panel.SetActive(true);
        UpdateHealthText(currentTarget.CurrentHP);
        UpdatePosition();

        Debug.Log("panel showed for " + target.name);
    }

    public void Hide(Health target)
    {
        if (target == null)
            return;

        // Only hide if the target exiting is the one currently displayed
        if (currentTarget != target)
            return;

        Unsubscribe(currentTarget);
        currentTarget = null;

        if (panel != null)
        {
            panel.SetActive(false);
            Debug.Log("panel hidden");
        }
    }

    public void ForceHide()
    {
        if (currentTarget != null)
        {
            Unsubscribe(currentTarget);
            currentTarget = null;
        }

        if (panel != null)
        {
            panel.SetActive(false);
            Debug.Log("panel force hidden");
        }
    }

    private void UpdatePosition()
    {
        if (currentTarget == null || panel == null)
            return;

        SpriteRenderer sr = currentTarget.GetComponent<SpriteRenderer>();
        Vector3 worldPosition = currentTarget.transform.position;

        if (sr != null)
            worldPosition = sr.bounds.center;

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        panel.transform.position = screenPosition;
    }

    private void UpdateHealthText(int hp)
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + hp;
        }

        Debug.Log("HP: " + hp);
    }

    private void HandleTargetDied()
    {
        ForceHide();
        Debug.Log("target has died");
    }

    private void Subscribe(Health target)
    {
        if (target == null) return;

        target.OnHealthChanged -= UpdateHealthText;
        target.OnDied -= HandleTargetDied;

        target.OnHealthChanged += UpdateHealthText;
        target.OnDied += HandleTargetDied;
    }

    private void Unsubscribe(Health target)
    {
        if (target == null) return;

        target.OnHealthChanged -= UpdateHealthText;
        target.OnDied -= HandleTargetDied;
    }
}