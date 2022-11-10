using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject settingsControl;
    public GameObject settingsSound;
    public GameObject saveGame;
    public GameObject continuePanel;
    public GameObject viewControl;
    public GameObject mainMenu;

    GameObject btnSettings;
    GameObject btnSaveGame;
    GameObject btnContinue;
    GameObject btnViewControl;
    GameObject btnMainMenu;
    GameObject btnSound;
    GameObject btnControl;

    private void Start()
    {
        btnSettings = transform.Find("btn_settings").gameObject;
        btnSaveGame = transform.Find("btn_saveGame").gameObject;
        btnContinue = transform.Find("btn_continue").gameObject;
        btnViewControl = transform.Find("btn_viewControl").gameObject;
        btnMainMenu = transform.Find("btn_mainMenu").gameObject;
        btnSound = transform.Find("btn_sound").gameObject;
        btnControl = transform.Find("btn_control").gameObject;
        pauseMenu.SetActive(false);
        btnSettings.SetActive(false);
        btnSaveGame.SetActive(false);
        btnContinue.SetActive(false);
        btnViewControl.SetActive(false);
        btnMainMenu.SetActive(false);
        btnSound.SetActive(false);
        btnControl.SetActive(false);
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        btnSettings.SetActive(true);
        btnSaveGame.SetActive(true);
        btnContinue.SetActive(true);
        btnViewControl.SetActive(true);
        btnMainMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void openSettingsControl()
    {
        pauseMenu.SetActive(false);
        settingsControl.SetActive(true);
        settingsSound.SetActive(false);
        saveGame.SetActive(false);
        continuePanel.SetActive(false);
        viewControl.SetActive(false);
        mainMenu.SetActive(false);
        btnSound.SetActive(true);
        btnControl.SetActive(true);
    }

    public void openSettingSound()
    {
        pauseMenu.SetActive(false);
        settingsControl.SetActive(false);
        settingsSound.SetActive(true);
        saveGame.SetActive(false);
        continuePanel.SetActive(false);
        viewControl.SetActive(false);
        mainMenu.SetActive(false);
        btnSound.SetActive(true);
        btnControl.SetActive(true);
    }

    public void openSaveGame()
    {
        pauseMenu.SetActive(false);
        settingsControl.SetActive(false);
        settingsSound.SetActive(false);
        saveGame.SetActive(true);
        continuePanel.SetActive(false);
        viewControl.SetActive(false);
        mainMenu.SetActive(false);
        btnSound.SetActive(false);
        btnControl.SetActive(false);
    }


    public void Resume()
    {
        pauseMenu.SetActive(false);
        continuePanel.SetActive(false);
        settingsControl.SetActive(false);
        settingsSound.SetActive(false);
        saveGame.SetActive(false);
        viewControl.SetActive(false);
        mainMenu.SetActive(false);
        btnSound.SetActive(false);
        btnControl.SetActive(false);
        Time.timeScale = 1f;
    }

    public void openViewControl()
    {
        pauseMenu.SetActive(false);
        settingsControl.SetActive(false);
        settingsSound.SetActive(false);
        saveGame.SetActive(false);
        continuePanel.SetActive(false);
        viewControl.SetActive(true);
        mainMenu.SetActive(false);
        btnSound.SetActive(false);
        btnControl.SetActive(false);
    }

    public void openMainMenu()
    {
        //???
    }

    public void saveGameYes()
    {
        Debug.Log("button_yes");
    }

    public void saveGameNo()
    {
        Debug.Log("button_no");
    }
}
