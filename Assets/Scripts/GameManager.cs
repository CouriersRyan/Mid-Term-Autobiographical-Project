using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyList;
    
    //TODO: Game Start and Game End States (Win/Lose)
    
    private int enemyCount;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = new Vector2(0, 0);
        }

        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            enemyCount++;
            if(enemyCount < enemyList.Length)
            {
                enemyList[enemyCount].SetActive(true);
            }
            else
            {
                //Win
            }
        }
    }
}
