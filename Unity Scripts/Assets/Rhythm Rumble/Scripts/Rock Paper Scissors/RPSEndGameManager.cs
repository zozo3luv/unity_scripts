using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class RPSEndGameManager : MonoBehaviour
{
    [SerializeField] PlayableDirector m_directorPlayerWin;
    [SerializeField] PlayableDirector m_directorCompWin;

    void Start()
    {
        RPSCore.instance.EndGameMaster.AddListener(delegate { StartCoroutine(SlowDownAnimation()); });
        RPSCore.instance.CompDeath.AddListener(delegate { Invoke("PlayPlayerWinTimeline", 0.8f); }) ;
        RPSCore.instance.PlayerDeath.AddListener(PlayCompWinTimeline);
    }

    #region Timeline Controls
    void PlayPlayerWinTimeline()
    {
        m_directorPlayerWin.Play();
    }

    void PlayCompWinTimeline()
    {
        m_directorCompWin.Play();
    }
    #endregion

    #region Slow Down Animation and Timeline
    [Header("Slow Down Animation and Timeline")]
    [SerializeField] PlayableDirector m_directorCore;
    [SerializeField] Animator m_playerAnimator;
    [SerializeField] Animator m_compAnimator;

    //Smooth damp settings
    [Space]
    [SerializeField] float m_slowSmoothTime = 0.5f;
    float m_currentPlaySpeed = 1;
    float m_slowSmoothV;

    IEnumerator SlowDownAnimation()
    {
        while (true)
        {
            m_currentPlaySpeed = Mathf.SmoothDamp(m_currentPlaySpeed, 0, 
                ref m_slowSmoothV, m_slowSmoothTime);

            //If playback speed reaches 0, stop animations and timeline.
            if (m_currentPlaySpeed <= 0)
            {
                m_directorCore.Stop();
                m_playerAnimator.StopPlayback();
                m_compAnimator.StopPlayback();
                break;
            }

            //Set timeline and animation speed
            m_directorCore.playableGraph.GetRootPlayable(0).SetSpeed(m_currentPlaySpeed);
            m_playerAnimator.speed = m_currentPlaySpeed;
            m_compAnimator.speed = m_currentPlaySpeed;

            yield return null;
        }
    }
    #endregion
}
