using UnityEngine;
using TMPro;

public class HoverHealthUI : MonoBehaviour
{
    public static HoverHealthUI main;

    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text healthText;

    private RectTransform panelRect;

    private void Awake()
    {
        main = this;

        if (panel != null)
        {
            panelRect = panel.GetComponent<RectTransform>();
            panel.SetActive(false);
        }
    }

    public void Show(Vector3 worldPosition, int health)
    {
        Debug.Log("Show() was called");

        if (panel == null || healthText == null || panelRect == null)
        {
            Debug.Log("Hover UI references missing");
            return;
        }

        panel.SetActive(true);
        healthText.text = "HP: " + health;

        // TEMP TEST: center relative to the UI canvas
        panelRect.anchoredPosition = Vector2.zero;

        Debug.Log("Panel should be visible in UI center");
    }

    public void Hide()
    {
        Debug.Log("Hide() called");

        if (panel != null)
        {
            panel.SetActive(false);
        }
    }
}