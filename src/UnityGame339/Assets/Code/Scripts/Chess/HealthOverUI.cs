using UnityEngine;
using TMPro;

public class HoverHealthUI : MonoBehaviour
{
    public static HoverHealthUI main;

    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text healthText;

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
        panel.SetActive(true);
        healthText.text = "HP: " + health;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
        panel.transform.position = screenPos + new Vector3(0, 40, 0);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}