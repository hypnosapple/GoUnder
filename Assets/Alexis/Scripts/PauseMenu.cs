using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool GameIsPaused = false;

    public UIInteract inventorySystem;
    //public CommunicationInteract communicationSystem;
    public InventoryManager inventoryManager;
    public GameManager gameManager;

    public GameObject pauseMenu;
    public GameObject settingsControl;
    public GameObject settingsSound;
    public GameObject settingsDisplay;
    public GameObject viewControl;

    GameObject btnSettings;
    GameObject btnViewControl;
    GameObject btnMainMenu;
    GameObject btnSound;
    GameObject btnControl;
    GameObject btnDisplay;

    private void Start()
    {
        btnSettings = transform.Find("btn_settings").gameObject;
        btnViewControl = transform.Find("btn_viewControl").gameObject;
        btnMainMenu = transform.Find("btn_mainMenu").gameObject;
        btnSound = transform.Find("btn_sound").gameObject;
        btnControl = transform.Find("btn_control").gameObject;
        btnDisplay = transform.Find("btn_display").gameObject;
        btnSettings.SetActive(false);
        btnViewControl.SetActive(false);
        btnMainMenu.SetActive(false);
        btnSound.SetActive(false);
        btnControl.SetActive(false);
        btnDisplay.SetActive(false);
    }

    void Update()
    {
        if (gameManager.pauseMenuEnabled && inventoryManager.pauseMenuEnabled && Input.GetKeyDown(KeyCode.Escape))
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
        btnViewControl.SetActive(true);
        btnMainMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.visible = true;
        Debug.Log("Cursor is " + Cursor.visible);
    }

    public void openSettingsControl()
    {
        settingsControl.SetActive(true);
        settingsSound.SetActive(false);
        settingsDisplay.SetActive(false);
        viewControl.SetActive(false);
        btnSound.SetActive(true);
        btnControl.SetActive(true);
        btnDisplay.SetActive(true);
    }

    public void openSettingSound()
    {
        settingsControl.SetActive(false);
        settingsSound.SetActive(true);
        settingsDisplay.SetActive(false);
        viewControl.SetActive(false);
        btnSound.SetActive(true);
        btnControl.SetActive(true);
        btnDisplay.SetActive(true);
    }

    public void openDisplay()
    {
        settingsControl.SetActive(false);
        settingsSound.SetActive(false);
        settingsDisplay.SetActive(true);
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
        viewControl.SetActive(false);
        btnSettings.SetActive(false);
        btnViewControl.SetActive(false);
        btnMainMenu.SetActive(false);
        btnSound.SetActive(false);
        btnControl.SetActive(false);
        btnDisplay.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.visible = false;
        Debug.Log("Cursor is " + Cursor.visible);
    }

    public void openViewControl()
    {
        settingsControl.SetActive(false);
        settingsSound.SetActive(false);
        settingsDisplay.SetActive(false);
        viewControl.SetActive(true);
        btnSound.SetActive(false);
        btnControl.SetActive(false);
        btnDisplay.SetActive(false);
    }

    public void openMainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void GoBackToPause()
    {
        settingsControl.SetActive(false);
        settingsSound.SetActive(false);
        settingsDisplay.SetActive(false);
        viewControl.SetActive(false);
        btnSound.SetActive(false);
        btnControl.SetActive(false);
        btnDisplay.SetActive(false);
        btnSettings.SetActive(true);
        btnViewControl.SetActive(true);
        btnMainMenu.SetActive(true);
    }

}
