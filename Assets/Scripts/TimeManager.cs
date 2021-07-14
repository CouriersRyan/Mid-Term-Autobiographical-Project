using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI t_timer;
    [SerializeField] private float timer = 100;
    
    private bool isTimed = true;

    private void Start()
    {
        GameManager.m_GameLose += DeactivateTimer;
        GameManager.m_GameWin += PauseTimer;
    }

    // Update is called once per frame
    private void Update()
    {
        if (t_timer.gameObject.activeSelf && isTimed)
        {
            timer -= Time.deltaTime;
            t_timer.text = Mathf.Floor(timer).ToString();

            if (timer <= 0) GameManager.m_GameLose();
        }
    }

    public void DeactivateTimer()
    {
        t_timer.gameObject.SetActive(false);
    }
    
    public void PauseTimer()
    {
        isTimed = false;
    }
}
