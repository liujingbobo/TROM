using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public Button startGameButton;
    public Button settingButton;
    public Button exitButton;

    public void Start()
    {
        startGameButton.onClick.AddListener(OnStartGame);
        settingButton.onClick.AddListener(OnOpenSetting);
        exitButton.onClick.AddListener(OnExitGame);
    }

    public void OnStartGame()
    {
        SceneLoader.Singleton.LoadScene("CampScene");
    }

    public void OnOpenSetting()
    {
        
    }

    public void OnExitGame()
    {
        
    }
}
