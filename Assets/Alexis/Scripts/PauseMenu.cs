using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public bool GameIsPaused = false;

    public UIInteract inventorySystem;
    public CommunicationInteract communicationSystem;

    public GameObject pauseMenu;
    public GameObject settingsControl;
    public GameObject settingsSound;
    public GameObject settingsDisplay;
    public GameObject saveGame;
    public GameObject gameSaved;
    public GameObject gameCannot;
    public GameObject viewControl;

    GameObject btnSettings;
    GameObject btnSaveGame;
    GameObject btnViewControl;
    GameObject btnMainMenu;
    GameObject btnSound;
    GameObject btnControl;
    GameObject btnDisplay;

    private void Start()
    {
        btnSettings = transform.Find("btn_settings").gameObject;
        btnSaveGame = transform.Find("btn_saveGame").gameObject;
        btnViewControl = transform.Find("btn_viewControl").gameObject;
        btnMainMenu = transform.Find("btn_mainMenu").gameObject;
        btnSound = transform.Find("btn_sound").gameObject;
        btnControl = transform.Find("btn_control").gameObject;
        btnDisplay = transform.Find("btn_display").gameObject;
        btnSettings.SetActive(false);
        btnSaveGame.SetActive(false);
        btnViewControl.SetActive(false);
        btnMainMenu.SetActive(false);
        btnSound.SetActive(false);
        btnControl.SetActive(false);
        btnDisplay.SetActive(false);
    }

    void Update()
    {
        if (communicationSystem.pauseMenuEnable && inventorySystem.pauseMenuEnable && Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        btnSettings.SetActive(true);
        btnSaveGame.SetActive(true);
        btnViewControl.SetActive(true);
        btnMainMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void openSettingsControl()
    {
        pauseMenu.SetActive(false);
        settingsControl.SetActive(true);
        settingsSound.SetActive(false);
        settingsDisplay.SetActive(false);
        saveGame.SetActive(false);
        gameSaved.SetActive(false);
        gameCannot.SetActive(false);
        viewControl.SetActive(false);
        btnSound.SetActive(true);
        btnControl.SetActive(true);
        btnDisplay.SetActive(true);
    }

    public void openSettingSound()
    {
        pauseMenu.SetActive(false);
        settingsControl.SetActive(false);
        settingsSound.SetActive(true);
        settingsDisplay.SetActive(false);
        saveGame.SetActive(false);
        gameSaved.SetActive(false);
        gameCannot.SetActive(false);
        viewControl.SetActive(false);
        btnSound.SetActive(true);
        btnControl.SetActive(true);
        btnDisplay.SetActive(true);
    }

    public void openDisplay()
    {
        pauseMenu.SetActive(false);
        settingsControl.SetActive(false);
        settingsSound.SetActive(false);
        settingsDisplay.SetActive(true);
        saveGame.SetActive(false);
        gameSaved.SetActive(false);
        gameCannot.SetActive(false);
        viewControl.SetActive(false);
        btnSound.SetActive(true);
        btnControl.SetActive(true);
        btnDisplay.SetActive(true);
    }
    public void openSaveGame()
    {
        pauseMenu.SetActive(false);
        settingsControl.SetActive(false);
        settingsSound.SetActive(false);
        settingsDisplay.SetActive(true);
        saveGame.SetActive(true);
        gameSaved.SetActive(false);
        gameCannot.SetActive(false);
        viewControl.SetActive(false);
        btnSound.SetActive(false);
        btnControl.SetActive(false);
        btnDisplay.SetActive(false);
    }

    public void openSavedGame()
    {
        pauseMenu.SetActive(false);
        settingsControl.SetActive(false);
        settingsSound.SetActive(false);
        settingsDisplay.SetActive(false);
        saveGame.SetActive(false);
        gameSaved.SetActive(true);
        gameCannot.SetActive(false);
        viewControl.SetActive(false);
        btnSound.SetActive(true);
        btnControl.SetActive(true);
        btnDisplay.SetActive(true);
    }

    public void openCannotSave()
    {
        pauseMenu.SetActive(false);
        settingsControl.SetActive(false);
        settingsSound.SetActive(false);
        settingsDisplay.SetActive(false);
        saveGame.SetActive(false);
        gameSaved.SetActive(false);
        gameCannot.SetActive(true);
        viewControl.SetActive(false);
        btnSound.SetActive(true);
        btnControl.SetActive(true);
        btnDisplay.SetActive(true);
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        settingsControl.SetActive(false);
        settingsSound.SetActive(false);
        settingsDisplay.SetActive(false);
        saveGame.SetActive(false);
        gameSaved.SetActive(false);
        gameCannot.SetActive(false);
        viewControl.SetActive(false);
        btnSettings.SetActive(false);
        btnSaveGame.SetActive(false);
        btnViewControl.SetActive(false);
        btnMainMenu.SetActive(false);
        btnSound.SetActive(false);
        btnControl.SetActive(false);
        btnDisplay.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void openViewControl()
    {
        pauseMenu.SetActive(false);
        settingsControl.SetActive(false);
        settingsSound.SetActive(false);
        settingsDisplay.SetActive(false);
        saveGame.SetActive(false);
        gameSaved.SetActive(false);
        gameCannot.SetActive(false);
        viewControl.SetActive(true);
        btnSound.SetActive(false);
        btnControl.SetActive(false);
        btnDisplay.SetActive(false);
    }

    public void openMainMenu()
    {
        //???
    }

    public void GoBackToPause()
    {
        pauseMenu.SetActive(true);
        settingsControl.SetActive(false);
        settingsSound.SetActive(false);
        settingsDisplay.SetActive(false);
        saveGame.SetActive(false);
        gameSaved.SetActive(false);
        gameCannot.SetActive(false);
        viewControl.SetActive(false);
        btnSound.SetActive(false);
        btnControl.SetActive(false);
        btnDisplay.SetActive(false);
        btnSettings.SetActive(true);
        btnSaveGame.SetActive(true);
        btnViewControl.SetActive(true);
        btnMainMenu.SetActive(true);
    }

}
