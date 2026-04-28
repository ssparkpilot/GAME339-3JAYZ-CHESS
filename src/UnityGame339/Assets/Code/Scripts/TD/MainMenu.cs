using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnClickLoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void OnClickQuit()
    {
        Application.Quit();
        //Show this message in the editor so we know this method is being called
        Debug.Log("Main Menu Quit.");
    }
}
