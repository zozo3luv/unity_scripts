using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameButton : MonoBehaviour
{
    public void OpenScene(string _sceneName)
    {
        MasterGameManager.instance?.buttonSoundPlayer.Stop();
        MasterGameManager.instance?.buttonSoundPlayer.Play();

        SceneManager.LoadScene(_sceneName);
    }
}
