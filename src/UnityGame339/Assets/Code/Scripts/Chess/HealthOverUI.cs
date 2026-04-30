using UnityEngine;
using TMPro;

public class HoverHealthUI : MonoBehaviour
{
    public static HoverHealthUI main;

    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Vector3 offset = new Vector3(0, 15, 0);

    private void Awake()
    {
        main = this;

        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    public void Show(Vector3 worldPosition, int health)
    {
        if (panel == null || healthText == null) return;

        panel.SetActive(true);
        healthText.text = "HP: " + health;

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        panel.transform.position = screenPosition + offset;
    }

    public void Hide()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }
}