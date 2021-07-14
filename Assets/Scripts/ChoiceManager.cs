using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceManager : MonoBehaviour
{
    [SerializeField] private string choice0Text = "SOFT-BOILED";
    [SerializeField] private string choice1Text = "SUNNY-SIDE UP";

    [SerializeField] private RawImage choice0Image;
    [SerializeField] private RawImage choice1Image;

    [SerializeField] private string winText = " REIGNS SUPREME!";
    [SerializeField] private string loseText = " HAS BEEN DEFEATED!";

    [SerializeField] public RawImage inGameImage;
    
    [SerializeField] public TextMeshProUGUI endScreenText;
    [SerializeField] public RawImage endScreenImage;
    

    private bool selectedChoice; // false = 0 & true = 1
    
    // Start is called before the first frame update
    void Start()
    {
        GameManager.m_Choice0 += Choice0;
        GameManager.m_Choice1 += Choice1;
        GameManager.m_GameLose += EndScreenLose;
        GameManager.m_GameWin += EndScreenWin;
    }
    

    void Choice0()
    {
        selectedChoice = false;
        inGameImage.texture = choice0Image.texture;
        endScreenImage.texture = choice0Image.texture;
    }

    void Choice1()
    {
        selectedChoice = true;
        inGameImage.texture = choice1Image.texture;
        endScreenImage.texture = choice1Image.texture;
    }

    void EndScreenLose()
    {
        endScreenText.text = (selectedChoice ? choice1Text : choice0Text) + loseText;
    }

    void EndScreenWin()
    {
        endScreenText.text = (selectedChoice ? choice1Text : choice0Text) + winText;
    }
}
