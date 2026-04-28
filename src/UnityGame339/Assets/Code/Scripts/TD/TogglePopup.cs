using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TogglePopup : MonoBehaviour
{

private bool isPanelActive = false;
        public GameObject panel;    // secs

    public void OnToggle()
    {
        isPanelActive = !isPanelActive;
        panel.SetActive(isPanelActive);
    }

    public void ReverseToggle()
    {
        isPanelActive = !isPanelActive;
    }

    
}
