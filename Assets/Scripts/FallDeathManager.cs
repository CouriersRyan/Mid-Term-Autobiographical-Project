using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Manages what happens when an rigidbody object falls off of the arena.
public class FallDeathManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        //If it is a player, return it back up to the arena and lose 1 life.
        if (other.CompareTag("Player"))
        {
            other.transform.position = new Vector2(0, 0);
            LivesTracker.Instance.ChangeLives(-1);
        }
        
        //If it is an enemy, have to be set to inactive and activate the next enemy.
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);
            GameManager.Instance.enemyCount++;
            if(GameManager.Instance.enemyCount < GameManager.Instance.enemyList.Length)
            {
                GameManager.Instance.enemyList[GameManager.Instance.enemyCount].SetActive(true);
            }
            else
            {
                //Win
                GameManager.m_GameWin();
            }
        }
    }
}
