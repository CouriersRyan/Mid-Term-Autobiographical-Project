using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LivesTracker : MonoBehaviour
{
    public static LivesTracker Instance;
    
    [SerializeField] private TextMeshProUGUI t_lives;
    [SerializeField] private string text = "Lives: ";
    [SerializeField] private int lives = 3;
    
    
    void Awake()
    {
        if (Instance == null) Instance = this;
    }


    private void Start()
    {
        t_lives.text = text + lives;
    }


    public void ChangeLives(int delta)
    {
        lives += delta;
        t_lives.text = text + lives;
        if (lives <= 0) GameManager.m_GameLose();
    }
}
