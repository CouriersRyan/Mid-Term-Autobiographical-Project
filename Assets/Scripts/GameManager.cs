using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    
    
    //TODO: Game Start and Game End States (Win/Lose)
    public static UnityAction m_GameStart;
    public static UnityAction m_ChoiceMade;

    [SerializeField] private GameObject[] toEnableList;
    [SerializeField] private GameObject[] startScreen;
    [SerializeField] private GameObject[] choiceScreen;

    [SerializeField] private GameObject[] enemyList;
    private int _enemyCount;

    private float startingXPos0 = -4;
    private float startingXPos1 = 4;

    private void Awake()
    {
        m_ChoiceMade += EnableObjectsOnStart;
        m_ChoiceMade += DeactivateChoiceScreen;
        m_GameStart += DeactivateStartScreen;
        m_GameStart += ActivateChoiceScreen;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = new Vector2(0, 0);
        }

        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            _enemyCount++;
            if(_enemyCount < enemyList.Length)
            {
                enemyList[_enemyCount].SetActive(true);
            }
            else
            {
                //Win
            }
        }
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
    }

    public void Choice1()
    {
        toEnableList[0].transform.position = new Vector2(startingXPos1, toEnableList[0].transform.position.y);
        toEnableList[3].transform.position = new Vector2(startingXPos0, toEnableList[3].transform.position.y);
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
}
