using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class WAMEndGameManager : MonoBehaviour
{
    PlayableDirector m_director;
    [SerializeField] WhacAMoleManager m_gm;

    void Start()
    {
        m_director = GetComponent<PlayableDirector>();
        m_gm.WAM_Win.AddListener(PlayEndGameTimeline);
    }

    void PlayEndGameTimeline()
    {
        m_director.Play();
    }
}
