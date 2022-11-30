using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject InventoryCanvas;
    public GameObject player;
    public GameObject CrosshairCanvas;
    public CommunicationInteract CommunicationSystem;
    public UIInteract ComputerSystem;

    public SubtitleData_SO firstSub;
    

    public Camera mainCam;
    public GameObject CMStart;
    public GameObject CMStart2;
    public GameObject CMStart3;

    public GameObject blackPanel;
    private float fadeInT = 0f;
    private bool fadeIn = false;
    private float fadeOutT = 0f;
    private bool fadeOut = false;


    void Start()
    {
        player.GetComponent<CharacterController>().enabled = false;
        gameObject.GetComponent<SubtitleManager>().ShowSubtitle(firstSub);
        //EnableMove();
        StartFadeIn();
    }

    
    void Update()
    {
        InventoryVisibility();

        if (fadeIn)
        {
            if (fadeInT < 1f)
            {
                fadeInT += 0.3f * Time.deltaTime;
                blackPanel.GetComponent<Image>().color = Color.Lerp(new Color(blackPanel.GetComponent<Image>().color.r, blackPanel.GetComponent<Image>().color.g, blackPanel.GetComponent<Image>().color.b, 1f), new Color(blackPanel.GetComponent<Image>().color.r, blackPanel.GetComponent<Image>().color.g, blackPanel.GetComponent<Image>().color.b, 0f), fadeInT);

            }
            else
            {
                fadeIn = false;
            }
        }


        if (fadeOut)
        {
            if (fadeOutT < 1f)
            {
                fadeOutT += 0.3f * Time.deltaTime;
                blackPanel.GetComponent<Image>().color = Color.Lerp(new Color(blackPanel.GetComponent<Image>().color.r, blackPanel.GetComponent<Image>().color.g, blackPanel.GetComponent<Image>().color.b, 0f), new Color(blackPanel.GetComponent<Image>().color.r, blackPanel.GetComponent<Image>().color.g, blackPanel.GetComponent<Image>().color.b, 1f), fadeInT);

            }
            else
            {
                fadeOut = false;
            }
        }
    }


    public void InventoryVisibility()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (InventoryCanvas.activeInHierarchy)
            {
                if (CommunicationSystem.FocusOnScreen == false && ComputerSystem.FocusOnScreen == false)
                {
                    Cursor.visible = false;
                    InventoryCanvas.SetActive(false);
                    CrosshairCanvas.SetActive(true);
                }
                else
                {
                    Cursor.visible = true;
                    InventoryCanvas.SetActive(false);
                    CrosshairCanvas.SetActive(true);
                }
            }
            else
            {
                Cursor.visible = true;
                InventoryCanvas.SetActive(true);
                CrosshairCanvas.SetActive(false);
            }
        }

        if (InventoryCanvas.activeInHierarchy)
        {
            if (player.GetComponent<PlayerMovement>().enabled)
            {
                player.GetComponent<PlayerMovement>().enabled = false;
            }

            if (mainCam.GetComponent<Cinemachine.CinemachineBrain>().enabled)
            {
                mainCam.GetComponent<Cinemachine.CinemachineBrain>().enabled = false;
            }
        }
        else
        {
            if (!mainCam.GetComponent<Cinemachine.CinemachineBrain>().enabled)
            {
                mainCam.GetComponent<Cinemachine.CinemachineBrain>().enabled = true;
            }

            if (!player.GetComponent<PlayerMovement>().enabled)
            {
                player.GetComponent<PlayerMovement>().enabled = true;
            }
        }
    }

    public void EnableMove()
    {
        player.GetComponent<CharacterController>().enabled = true;
        StartCoroutine(SwitchCam());
    }

    IEnumerator SwitchCam()
    {
        CMStart.SetActive(false);
        yield return new WaitForSeconds(6f);
        CMStart2.SetActive(false);
        yield return new WaitForSeconds(5f);
        CMStart3.SetActive(false);
    }


    public void StartFadeIn()
    {
        if (blackPanel.GetComponent<Image>().color.a == 1)
        {
            fadeInT = 0f;

            fadeIn = true;
        }
    }



    public void StartFadeOut()
    {
        if (blackPanel.GetComponent<Image>().color.a == 0)
        {
            fadeOutT = 0f;

            fadeOut = true;
        }
    }
}
