﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadScene(string theSceneName)
    {
        SceneManager.LoadScene(theSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
