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

        Debug.Log("HoverHealthUI Awake called");

        if (panel != null)
        {
            panel.SetActive(false);
        }
        else
        {
            Debug.Log("Panel is NULL in Awake");
        }

        if (healthText == null)
        {
            Debug.Log("HealthText is NULL in Awake");
        }
    }

    public void Show(Vector3 worldPosition, int health)
    {
        Debug.Log("Show() was called");

        if (panel == null)
        {
            Debug.Log("Panel is NULL in Show()");
            return;
        }

        if (healthText == null)
        {
            Debug.Log("HealthText is NULL in Show()");
            return;
        }

        panel.SetActive(true);
        healthText.text = "HP: " + health;

        // 🔴 TEMP TEST: force to center of screen
        panel.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        Debug.Log("Panel should now be visible in center");
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