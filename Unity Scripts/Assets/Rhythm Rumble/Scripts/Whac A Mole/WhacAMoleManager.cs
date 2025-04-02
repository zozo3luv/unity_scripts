using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class WhacAMoleManager : MonoBehaviour
{
    #region Game Modes
    enum GameModes
    {
        Classic,
        Rhythm
    }

    [SerializeField] GameModes m_mode = GameModes.Classic;
    #endregion

    #region variables
    [Header("Classic Mode Settings")]
    [SerializeField] Vector2 m_possibleTimeBetweenPops;
    [SerializeField] Vector2 m_possibleStayTime;

    [Header("Rhythm Mode Settings")]
    [SerializeField] float m_fixedStayTime;

    [Header("Backend settings")]
    [SerializeField] TextMeshProUGUI m_scoreText;
    [SerializeField] Button[] m_buttonList;
    [SerializeField] Animator[] m_crocdileAnimators;
    AssignedButton[] m_assignedButtons;
    AssignedButton m_chosenButton;
    #endregion

    #region Whac A Mole State Machine
    public enum WAM_States
    {
        Idle,
        Play,
        End
    }

    [HideInInspector] public WAM_States m_gameState = WAM_States.Idle;
    [HideInInspector] public UnityEvent WAM_Idle;
    [HideInInspector] public UnityEvent WAM_Play;
    [HideInInspector] public UnityEvent WAM_Win;
    [HideInInspector] public UnityEvent WAM_Lose;

    public void SetGameState(int _stateIndex)
    {
        m_gameState = (WAM_States) _stateIndex;

        switch(m_gameState)
        {
            case WAM_States.Idle:
                WAM_Idle.Invoke();
                break;

            case WAM_States.Play:
                WAM_Play.Invoke();
                break;

            case WAM_States.End:
                WAM_Idle.Invoke();
                OnGameEnd();
                break;
        }
    }

    #endregion

    #region Start and Update
    void Start()
    {
        m_assignedButtons = new AssignedButton[m_buttonList.Length];
        //Take the list of buttons and construct class objects for them
        for (int i = 0; i < m_buttonList.Length; i++)
        {
            m_assignedButtons[i] = new AssignedButton(m_buttonList[i], m_crocdileAnimators[i]);
        }

        if (m_mode == GameModes.Classic) StartCoroutine(ChooseButton());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && m_gameState == WAM_States.Play)
        {
            // Check if the click is over a UI element
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Miss();
            }
        }
    }
    #endregion

    #region Game State Logic (Classic)
    //Automatically cycle between moles, user set time etc.
    IEnumerator m_currentGame;
    IEnumerator ChooseButton()
    {
        m_gameState = WAM_States.Idle;
        m_chosenButton = null;
        float _waitTime = Random.Range(m_possibleTimeBetweenPops.x, m_possibleTimeBetweenPops.y);
        yield return new WaitForSeconds(_waitTime);

        //Choose a button
        int _buttonIndex = Random.Range(0, m_buttonList.Length);
        m_chosenButton = m_assignedButtons[_buttonIndex];

        //Wait for input
        m_currentGame = WaitForInput();

        m_chosenButton.OnButtonChosen(m_currentGame);

        StartCoroutine(m_currentGame);
    }

    IEnumerator WaitForInput()
    {
        m_gameState = WAM_States.Play;
        float _stayTime = Random.Range(m_possibleStayTime.x, m_possibleStayTime.y);
        yield return new WaitForSeconds(_stayTime);

        //Reset Button
        m_chosenButton.OnButtonDechosen();

        //Start a new round
        StartCoroutine(ChooseButton());
    }
    #endregion

    #region Game State Logic (Rhythm)
    //Manually set when the mole pops in the timeline, call these functions when needed

    public void MoleUp(int _index)
    {
        IEnumerator _dechosenCoroutine = MoleDown(m_assignedButtons[_index]);
        m_assignedButtons[_index].OnButtonChosen(_dechosenCoroutine);
        StartCoroutine(_dechosenCoroutine);
    }

    IEnumerator MoleDown(AssignedButton _button)
    {
        yield return new WaitForSeconds(m_fixedStayTime);
        _button.OnButtonDechosen();
    }

    #endregion

    #region Score System
    [HideInInspector] public UnityEvent WAM_Score_onScore;
    [HideInInspector] public UnityEvent WAM_Score_onMiss;

    [Header("Score System")]
    [SerializeField] int m_winScore = 5;
    int m_score = 0;
    public float GetWinScore()
    {
        return m_winScore;
    }

    public float GetCurrentScore()
    {
        return m_score;
    }

    void Score()
    {
        //Update score
        m_score++;
        m_scoreText.text = m_score.ToString();

        WAM_Score_onScore.Invoke();
    }

    void Miss()
    {
        WAM_Score_onMiss.Invoke();
    }

    void OnGameEnd()
    {
        //Determine Winning or Losing
        if (m_score >= m_winScore) WAM_Win.Invoke();
        else WAM_Lose.Invoke();
    }
    #endregion

    #region Button Press
    void OnButtonPressed(AssignedButton _pressedButton)
    {
        //Check if the correct button is pressed
        if(_pressedButton.isChosen == true && m_gameState == WAM_States.Play)
        {
            _pressedButton.assignedButton.GetComponent<HitSound>().PlayCrocdileHitSound();

            //Score system
            Score();

            //Set the status of the button
            StopCoroutine(_pressedButton.coroutineOnButton);
            _pressedButton.OnButtonDechosen(true);

            //Set game state (Classic Mode)
            if (m_mode == GameModes.Classic)
            {
                StopCoroutine(m_currentGame);
                StartCoroutine(ChooseButton());
            }
        }
        else if (m_gameState == WAM_States.Play)
        {
            Miss();
        }
    }

    public void OnButtonPressed(int _buttonIndex)
    {
        OnButtonPressed(m_assignedButtons[_buttonIndex]);
    }
    #endregion

    #region Button Class
    class AssignedButton
    {
        public Button assignedButton { get; private set;}
        public bool isChosen {get; private set;}
        public Animator assignedAnimator { get; private set;}

        public IEnumerator coroutineOnButton { get; private set; }
        ColorBlock m_defaultColorBlock;

        public AssignedButton(Button _button, Animator _assignedAnimator)
        {
            assignedButton = _button;
            isChosen = false;
            m_defaultColorBlock = _button.colors;
            assignedAnimator = _assignedAnimator;
        }

        #region Button State Managers
        public void OnButtonChosen(IEnumerator _dechosenCoroutine)
        {
            isChosen = true;
            coroutineOnButton = _dechosenCoroutine;

            //Change button color to white
            assignedButton.TryGetComponent(out Button _button);
            ColorBlock _chosenStatusColor = new ColorBlock();
            _chosenStatusColor.normalColor = Color.white;
            _button.colors = _chosenStatusColor;

            //Play crocdile animation
            assignedAnimator.SetTrigger("Up");
        }

        public void OnButtonDechosen(bool _hit = false)
        {
            isChosen = false;
            assignedButton.colors = m_defaultColorBlock;

            //Play crocdile animation
            assignedAnimator.SetTrigger("Down");
            if (_hit) assignedAnimator.SetTrigger("Hit");
            else assignedAnimator.SetTrigger("Unhit");
        }
        #endregion
    }
    #endregion
}
