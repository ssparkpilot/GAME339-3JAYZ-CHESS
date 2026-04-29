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
    if (panel == null || healthText == null) return;

    panel.SetActive(true);
    healthText.text = "HP: " + health;

    Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
    panel.transform.position = screenPosition + new Vector3(0, 0, 0);
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