using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterGameManager : MonoBehaviour
{
    public static MasterGameManager instance;

    [HideInInspector] public List<string> completedGameList = new List<string>();

    public AudioSource buttonSoundPlayer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void RecordCompletedGame(string _sceneName)
    {
        completedGameList.Add(_sceneName);
    }

    public bool CheckGameCompletion(string _sceneName)
    {
        return completedGameList.Contains(_sceneName);
    }
}
