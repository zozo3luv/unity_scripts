using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] string m_mainMenuName = "Main Menu";

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(m_mainMenuName);
    }
}
