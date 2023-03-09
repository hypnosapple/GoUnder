using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public bool enableOldCutscenes;
    public bool enableNewCutscenes;
    public bool startFromSecondFloor;

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

    public GameObject CMUnderStart1;
    public GameObject CMUnderStart2;

    public GameObject videoScreen;
    public VideoPlayer videoPlayer;

    public GameObject blackPanel;
    public GameObject whitePanel;

    private float fadeInT = 0f;
    private bool fadeIn = false;
    private float fadeOutT = 0f;
    private bool fadeOut = false;

    private float WfadeInT = 0f;
    private bool WfadeIn = false;
    private float WfadeOutT = 0f;
    private bool WfadeOut = false;

    public string SceneName;

    public List<ItemData_SO> ItemsCollectedFromAbove;

    private float blackT = 0f;
    public bool GroundLevelEnd;
    public GameObject blackCube;
    private bool GroundLevelFadedOut = false;


    [SerializeField] private CinemachineVirtualCamera vCam;
    private CinemachineBasicMultiChannelPerlin noise;

    public NoiseSettings screenShakeNoise;

    public AudioSource playerAudio;
    public AudioClip GroundEndPanic;
    public AudioClip UnderworldWake;
    public AudioClip OpeningTinnitus;

    public GameObject AtlasItemList;
    public GameObject FileItemList;
    public GameObject UseableItemList;

    public GameObject AtlasExpandButton;
    public GameObject FileExpandButton;
    public GameObject UseableExpandButton;

    public bool inventoryEnabled;

    public GameObject tunnel3to2;
    public GameObject glass;

    public GameObject cam1;

    void Start()
    {
        SceneName = SceneManager.GetActiveScene().name;
        noise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        // Opening cutscenes
        if (startFromSecondFloor)
        {
            whitePanel.SetActive(false);
            blackPanel.SetActive(false);
            CMStart.SetActive(false);
            CMStart2.SetActive(false);
            CMStart3.SetActive(false);

            inventoryEnabled = true;
            player.transform.position = new Vector3(91, 40.4483452f, -34);
        }

        else if (enableOldCutscenes)
        {
            blackPanel.SetActive(true);
            whitePanel.SetActive(false);

            if (SceneName == "MainScene")
            {
                CMStart.SetActive(true);
                CMStart2.SetActive(true);
                CMStart3.SetActive(true);

                //player.GetComponent<CharacterController>().enabled = false;
                player.GetComponent<PlayerMovement>().moveDisabled = true;
                inventoryEnabled = false;
                gameObject.GetComponent<SubtitleManager>().ShowSubtitle(firstSub);
            }
            else if (SceneName == "UnderScene")
            {
                for (int i = 0; i < ItemsCollectedFromAbove.Count; i++)
                {
                    InventoryCanvas.GetComponentInParent<InventoryManager>().AddItemWithoutReminder(ItemsCollectedFromAbove[i]);
                    //player.GetComponent<CharacterController>().enabled = false;
                    player.GetComponent<PlayerMovement>().moveDisabled = true;
                    inventoryEnabled = false;
                    StartCoroutine(UnderWorldOpening());
                }

            }
        }
        else if (enableNewCutscenes)
        {
            whitePanel.SetActive(true);
            blackPanel.SetActive(false);
            CMStart.SetActive(true);
            CMStart2.SetActive(true);
            CMStart3.SetActive(true);

            //player.GetComponent<CharacterController>().enabled = false;
            player.GetComponent<PlayerMovement>().moveDisabled = true;
            inventoryEnabled = false;
            StartCoroutine(NewOpening());
        }
        else
        {
            whitePanel.SetActive(false);
            blackPanel.SetActive(false);
            CMStart.SetActive(false);
            CMStart2.SetActive(false);
            CMStart3.SetActive(false);

            inventoryEnabled = true;
        }
        
        
        
        //EnableMove();
        //StartFadeIn();
    }

    
    void Update()
    {
        InventoryVisibility();
        TransitionToUnderworldInProgress();

        BlackFadeProcess();
        WhiteFadeProcess();

        CheckButtonAvailablity();
    }


    public void CheckButtonAvailablity()
    {
        if (AtlasItemList.GetComponent<AtlasContainerUI>().AtlasItems.Count == 0)
        {
            AtlasExpandButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            AtlasExpandButton.GetComponent<Button>().interactable = true;
        }

        if (FileItemList.GetComponent<FileContainerUI>().FileItems.Count == 0)
        {
            FileExpandButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            FileExpandButton.GetComponent<Button>().interactable = true;
        }

        if (UseableItemList.GetComponent<UseableContainerUI>().UseableItems.Count == 0)
        {
            UseableExpandButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            UseableExpandButton.GetComponent<Button>().interactable = true;
        }
    }


    public void InventoryVisibility()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && inventoryEnabled)
        {
            if (InventoryCanvas.activeInHierarchy)
            {
                if (CommunicationSystem.FocusOnScreen == false && ComputerSystem.FocusOnScreen == false)
                {
                    Cursor.visible = false;
                    if (!mainCam.GetComponent<Cinemachine.CinemachineBrain>().enabled)
                    {
                        mainCam.GetComponent<Cinemachine.CinemachineBrain>().enabled = true;
                    }

                    if (player.GetComponent<PlayerMovement>().moveDisabled)
                    {
                        player.GetComponent<PlayerMovement>().moveDisabled = false;
                    }

                    InventoryCanvas.SetActive(false);
                    CrosshairCanvas.SetActive(true);


                }
                else
                {
                    Cursor.visible = true;
                    InventoryCanvas.SetActive(false);
                    //CrosshairCanvas.SetActive(true);
                }
            }
            else
            {
                Cursor.visible = true;
                InventoryCanvas.SetActive(true);
                CrosshairCanvas.SetActive(false);

                if (!player.GetComponent<PlayerMovement>().moveDisabled)
                {
                    player.GetComponent<PlayerMovement>().moveDisabled = true;
                }

                if (mainCam.GetComponent<Cinemachine.CinemachineBrain>().enabled)
                {
                    mainCam.GetComponent<Cinemachine.CinemachineBrain>().enabled = false;
                }
            }
        }

        
    }

    public void EnableOpeningMove()
    {
        
        StartCoroutine(SwitchOpeningCam());
    }

    IEnumerator SwitchOpeningCam()
    {
        CMStart.SetActive(false);
        yield return new WaitForSeconds(6f);
        CMStart2.SetActive(false);
        yield return new WaitForSeconds(5f);
        CMStart3.SetActive(false);
        yield return new WaitForSeconds(3.5f);

        //player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<PlayerMovement>().moveDisabled = false;
        inventoryEnabled = true;
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


    public void StartFadeInWhite()
    {
        if (whitePanel.GetComponent<Image>().color.a == 1)
        {
            WfadeInT = 0f;

            WfadeIn = true;
        }
    }


    public void StartFadeOutWhite()
    {
        if (whitePanel.GetComponent<Image>().color.a == 0)
        {
            WfadeOutT = 0f;

            WfadeOut = true;
        }
    }


    public void BlackFadeProcess()
    {
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
                blackPanel.GetComponent<Image>().color = Color.Lerp(new Color(blackPanel.GetComponent<Image>().color.r, blackPanel.GetComponent<Image>().color.g, blackPanel.GetComponent<Image>().color.b, 0f), new Color(blackPanel.GetComponent<Image>().color.r, blackPanel.GetComponent<Image>().color.g, blackPanel.GetComponent<Image>().color.b, 1f), fadeOutT);

            }
            else
            {
                fadeOut = false;

            }
        }
    }


    public void WhiteFadeProcess()
    {
        if (WfadeIn)
        {
            if (WfadeInT < 1f)
            {
                WfadeInT += 0.3f * Time.deltaTime;
                whitePanel.GetComponent<Image>().color = Color.Lerp(new Color(whitePanel.GetComponent<Image>().color.r, whitePanel.GetComponent<Image>().color.g, whitePanel.GetComponent<Image>().color.b, 1f), new Color(whitePanel.GetComponent<Image>().color.r, whitePanel.GetComponent<Image>().color.g, whitePanel.GetComponent<Image>().color.b, 0f), WfadeInT);

            }
            else
            {
                WfadeIn = false;
            }
        }


        if (WfadeOut)
        {
            if (WfadeOutT < 1f)
            {
                WfadeOutT += 0.3f * Time.deltaTime;
                whitePanel.GetComponent<Image>().color = Color.Lerp(new Color(whitePanel.GetComponent<Image>().color.r, whitePanel.GetComponent<Image>().color.g, whitePanel.GetComponent<Image>().color.b, 0f), new Color(whitePanel.GetComponent<Image>().color.r, whitePanel.GetComponent<Image>().color.g, whitePanel.GetComponent<Image>().color.b, 1f), WfadeOutT);

            }
            else
            {
                WfadeOut = false;

            }
        }
    }


    public void TransitionToUnderworld()
    {
        StartCoroutine(TransitionToUnderworldCoroutine());
    }

    public void TransitionToUnderworldInProgress()
    {
        if (GroundLevelEnd)
        {
            if (blackT < 1f)
            {
                blackCube.transform.position = Vector3.Lerp(new Vector3(0, -160, 0), new Vector3(0, 0, 0), blackT);
                blackT += 0.007f * Time.deltaTime;
            }

            if (blackT > 0.1f && !GroundLevelFadedOut)
            {
                StartFadeOut();
                GroundLevelFadedOut = true;
            }

            if (blackT > 0.15f)
            {
                SceneManager.LoadScene("UnderScene");
            }
        }
    }

    IEnumerator UnderWorldOpening()
    {
        yield return new WaitForSeconds(2f);
        StartFadeIn();
        playerAudio.clip = UnderworldWake;
        playerAudio.Play();

        yield return new WaitForSeconds(5f);
        CMUnderStart1.SetActive(false);
        yield return new WaitForSeconds(4f);
        CMUnderStart2.SetActive(false);
        yield return new WaitForSeconds(4f);
        //player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<PlayerMovement>().moveDisabled = false;
        inventoryEnabled = true;

    }

    IEnumerator TransitionToUnderworldCoroutine()
    {
        yield return new WaitForSeconds(3f);
        //player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<PlayerMovement>().moveDisabled = true;
        inventoryEnabled = false;
        player.GetComponent<PlayerMovement>().cam6DShakeOn = true;

        noise.m_NoiseProfile = screenShakeNoise;
        noise.m_AmplitudeGain = 6f;
        noise.m_FrequencyGain = 2.5f;

        blackCube.SetActive(true);
        GroundLevelEnd = true;
        playerAudio.clip = GroundEndPanic;
        playerAudio.Play();
    }

    IEnumerator NewOpening()
    {
        playerAudio.clip = OpeningTinnitus;
        playerAudio.Play();
        yield return new WaitForSeconds(3f);
        StartFadeInWhite();
        yield return new WaitForSeconds(5f);
        EnableOpeningMove();
        
    }


    public void PlayVideo1()
    {
        blackPanel.SetActive(true);
        blackPanel.GetComponent<Image>().color = new Color(0, 0, 0, 1);

        //player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<PlayerMovement>().moveDisabled = true;
        inventoryEnabled = false;

        
        tunnel3to2.SetActive(false);
        glass.SetActive(true);
        player.transform.position = new Vector3(26, 40.4483452f, -34);

        videoScreen.SetActive(true);
        videoPlayer.Play();
        StartCoroutine(AfterVideo1());
    }


    IEnumerator AfterVideo1()
    {
        
        yield return new WaitForSeconds(29f);
        player.transform.position = new Vector3(26, 40.4483452f, -34);
        videoScreen.SetActive(false);
        StartFadeIn();
        yield return new WaitForSeconds(4f);

        player.GetComponent<PlayerMovement>().moveDisabled = false;
        inventoryEnabled = true;
        
    }

    public void CloseCam1()
    {
        cam1.SetActive(false);
    }


}
