using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ScreenManager : MonoBehaviour
{

    public GameObject MainMenu;
    public GameObject SettingsMenu;
    public GameObject DeathMenu;
    public PipeTracker pipeTracker;
    public AudioSource swoosh;

    void Start()
    {
        swoosh.Stop();
    }

    public void SwitchMenus(string location)
    {
        switch (location)
        {
            case "Main":
                MainMenu.SetActive(true);
                SettingsMenu.SetActive(false);
                DeathMenu.SetActive(false);
                Time.timeScale = 1;
                break;

            case "Setting":
                if (SettingsMenu.activeSelf)
                {
                    SettingsMenu.SetActive(false);
                }
                else
                {
                    SettingsMenu.SetActive(true);
                }
                break;

            case "Play":
                MainMenu.SetActive(false);
                SettingsMenu.SetActive(false);
                DeathMenu.SetActive(false);
                Time.timeScale = 1;
                pipeTracker.isDead = false;
                pipeTracker.isGameActive = true;
                pipeTracker.birdTracker.canMove = true;
                pipeTracker.Restart();
                swoosh.Play();
                break;

            case "Exit":
                Application.Quit();
                Debug.Log("Closed Application");
                break;

            case "Death":
                MainMenu.SetActive(true);
                SettingsMenu.SetActive(false);
                DeathMenu.SetActive(true);
                pipeTracker.isGameActive = false;
                break;

        }
    }
}