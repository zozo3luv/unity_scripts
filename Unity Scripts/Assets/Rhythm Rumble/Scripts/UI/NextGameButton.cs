using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextGameButton : MonoBehaviour
{
    public void GoToNextScene(string _nextSceneName)
    {
        SceneManager.LoadScene(_nextSceneName);
    }
}
