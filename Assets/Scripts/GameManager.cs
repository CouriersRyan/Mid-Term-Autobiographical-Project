using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    //TODO: Game Start and Game End States (Win/Lose)
    public static UnityAction m_GameStart;
    public static UnityAction m_ChoiceMade;
    public static UnityAction m_Choice0;
    public static UnityAction m_Choice1;
    public static UnityAction m_GameLose;
    public static UnityAction m_GameWin;

    [SerializeField] private GameObject[] toEnableList;
    [SerializeField] private GameObject[] startScreen;
    [SerializeField] private GameObject[] choiceScreen;
    
    [SerializeField] private GameObject[] objectsInAction;

    [SerializeField] public GameObject[] enemyList;
    public int enemyCount;

    private float startingXPos0 = -4;
    private float startingXPos1 = 4;
    

    private void Awake()
    {
        if (Instance == null) Instance = this;
        
        m_ChoiceMade += EnableObjectsOnStart;
        m_ChoiceMade += DeactivateChoiceScreen;
        m_GameStart += DeactivateStartScreen;
        m_GameStart += ActivateChoiceScreen;
        m_GameLose += GameEnd;
        m_GameWin += GameEnd;
    }

    public void GameStart()
    {
        m_GameStart();
    }

    public void ChoiceMade()
    {
        m_ChoiceMade();
    }

    public void Choice0()
    {
        toEnableList[0].transform.position = new Vector2(startingXPos0, toEnableList[0].transform.position.y);
        toEnableList[3].transform.position = new Vector2(startingXPos1, toEnableList[3].transform.position.y);
        m_Choice0();
    }

    public void Choice1()
    {
        toEnableList[0].transform.position = new Vector2(startingXPos1, toEnableList[0].transform.position.y);
        toEnableList[3].transform.position = new Vector2(startingXPos0, toEnableList[3].transform.position.y);
        m_Choice1();
    }

    private void EnableObjectsOnStart()
    {
        foreach (var t in toEnableList)
        {
            t.SetActive(true);
        }
    }

    private void DeactivateStartScreen()
    {
        foreach (var t in startScreen)
        {
            t.SetActive(false);
        }
    }

    private void ActivateChoiceScreen()
    {
        foreach (var t in choiceScreen)
        {
            t.SetActive(true);
        }
    }
    
    private void DeactivateChoiceScreen()
    {
        foreach (var t in choiceScreen)
        {
            t.SetActive(false);
        }
    }

    private void GameEnd()
    {
        foreach (var t in objectsInAction)
        {
            t.SetActive(false);
        }
        gameObject.GetComponent<ChoiceManager>().endScreenText.gameObject.SetActive(true);
        gameObject.GetComponent<ChoiceManager>().endScreenImage.gameObject.SetActive(true);
    }
}
