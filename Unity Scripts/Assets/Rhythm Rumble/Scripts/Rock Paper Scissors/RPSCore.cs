using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RPSCore : MonoBehaviour
{
    public static RPSCore instance;
    private void Awake()
    {
        instance = this;
    }

    #region Core Game Logic
    bool m_idle = true;
    [HideInInspector] public UnityEvent StartRound;
    [HideInInspector] public UnityEvent EndRound;
    [HideInInspector] public UnityEvent TimeRunOut;
    [HideInInspector] public UnityEvent PlayerSelect;
    [HideInInspector] public UnityEvent PlayerWin;
    [HideInInspector] public UnityEvent CompWin;
    [HideInInspector] public UnityEvent Draw;

    //Death events
    [HideInInspector] public UnityEvent EndGameMaster;
    [HideInInspector] public UnityEvent PlayerDeath;
    [HideInInspector] public UnityEvent CompDeath;

    //Animation events
    [HideInInspector] public UnityEvent AnimatePlayerWin;
    [HideInInspector] public UnityEvent AnimateCompWin;

    //Implement on timeline, while starting comp animation
    public void OnStartRound()
    {
        m_result = 2; //Default result is 2 (Draw)
        SetCompHand();
        StartRound.Invoke();
        Debug.Log("Start Round!");
    }

    //Implement on Timing Ring animator, or activated by player button press
    public void OnEndRound(bool _timeRunOut = false)
    {
        if (m_idle) return;
        m_idle = true;

        //If this is triggered at the end of the timing ring, trigger computer win.
        if (_timeRunOut)
        {
            OnCompWin();
            OnAttackAnimation(true);
        }

        EndRound.Invoke();
        Debug.Log("End Round!");
    }

    //Implement on timing ring animator
    public void OnWaitPlayerInput()
    {
        m_idle = false;
    }

    //Implemented with button press code
    void OnPlayerHandSelect(Hand _hand)
    {
        if (m_idle) return;

        playerHand = _hand;
        m_result = CompareHands(playerHand, compHand);
        Debug.Log("Player chose: " + playerHand);

        switch (m_result)
        {
            case 0:
                OnPlayerWin();
                Debug.Log("Player wins!");
                break;

            case 1:
                OnCompWin();
                Debug.Log("Comp wins!");
                break;

            case 2:
                OnDraw();
                Debug.Log("Draw!");
                break;
        }

        PlayerSelect.Invoke();
        OnEndRound();
    }

    void OnPlayerWin()
    {
        PlayerWin.Invoke();
    }

    void OnCompWin()
    {
        CompWin.Invoke();
    }

    void OnDraw()
    {
        Draw.Invoke();
    }

    //Implemented 
    public void OnAttackAnimation(bool _timeRunOut = false)
    {
        if (_timeRunOut) AnimateCompWin.Invoke();
        else if (m_result == 0) AnimatePlayerWin.Invoke();
        else if (m_result == 1) AnimateCompWin.Invoke();
    }
    #endregion

    #region Rock Paper Scissors Rules
    public Hand playerHand { get; private set; }
    public Hand compHand { get; private set; }

    public enum Hand
    {
        Rock,
        Paper,
        Scissors
    }

    void SetCompHand()
    {
        //Randomly choose a hand
        Array _hands = Enum.GetValues(typeof(Hand));
        System.Random _random = new System.Random();
        compHand = (Hand) _hands.GetValue(_random.Next(_hands.Length));
        Debug.Log("Computer Hand: " + compHand);
    }

    #region Button Press
    public void OnRockPressed()
    {
        OnPlayerHandSelect(Hand.Rock);
    }

    public void OnScissorsPressed()
    {
        OnPlayerHandSelect(Hand.Scissors);
    }

    public void OnPaperPressed()
    {
        OnPlayerHandSelect(Hand.Paper);
    }
    #endregion

    int m_result = new int();
    int CompareHands(Hand _playerHand, Hand _compHand)
    {
        switch(_playerHand)
        {
            case Hand.Rock:
                if (_compHand == Hand.Scissors) return 0;
                if (_compHand == Hand.Paper) return 1;
                break;

            case Hand.Scissors:
                if (_compHand == Hand.Paper) return 0;
                if (_compHand == Hand.Rock) return 1;
                break;

            case Hand.Paper:
                if (_compHand == Hand.Rock) return 0;
                if (_compHand == Hand.Scissors) return 1;
                break;
        }

        //if they have same hands, return 2 as draw
        return 2;
    }
    #endregion
}
