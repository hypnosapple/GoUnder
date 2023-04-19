using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.Video;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Cutscene Controll")]
    public bool enableOldCutscenes;
    public bool enableNewCutscenes;
    public bool startFromSecondFloor;

    [Header("Player")]
    public GameObject player;

    [Header("Canvas")]
    public GameObject InventoryCanvas;
    
    public GameObject CrosshairCanvas;

    [Header("Systems")]
    public CommunicationInteract CommunicationSystem;
    public UIInteract ComputerSystem;

    [Header("Subtitle Data")]
    public SubtitleData_SO firstSub;

    [Header("Camera")]
    public Camera mainCam;
    [SerializeField] private CinemachineVirtualCamera vCam;
    public GameObject CMStart;
    public GameObject CMStart2;
    public GameObject CMStart3;

    public GameObject CMUnderStart1;
    public GameObject CMUnderStart2;

    [Header("Video")]
    public GameObject videoScreen;
    public VideoPlayer cutsceneVideoPlayer;
    public VideoPlayer TV1VideoPlayer;

    [Header("Fade in/out Panel")]
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


    [Header("Underworld Level")]
    public List<ItemData_SO> ItemsCollectedFromAbove;

    private float blackT = 0f;
    public bool GroundLevelEnd;
    public GameObject blackCube;
    private bool GroundLevelFadedOut = false;


    [Header("Camera Noise")]
    private CinemachineBasicMultiChannelPerlin noise;
    
    public NoiseSettings screenShakeNoise;

    [Header("Audio")]
    public AudioSource playerAudio;
    public AudioSource tunnelTone;
    public AudioSource ambience;
    public AudioSource BGM;
    public AudioClip GroundEndPanic;
    public AudioClip UnderworldWake;
    public AudioClip OpeningTinnitus;
    public AudioClip AfterOpening1;
    public AudioClip AfterOpening2;
    public AudioSource wrongChoiceAlert;

    [Header("Inventory System")]
    public bool inventoryEnabled;
    public GameObject AtlasItemList;
    public GameObject FileItemList;
    public GameObject UseableItemList;

    public GameObject AtlasExpandButton;
    public GameObject FileExpandButton;
    public GameObject UseableExpandButton;

    
    [Header("Level 3 to 2")]
    public GameObject tunnel3to2;
    public GameObject glass;
    public GameObject cam1;

    [Header("Pause Menu")]
    public bool pauseMenuEnabled = true;

    [Header("Renderer Data")]
    public ForwardRendererData forwardRendererData;
    List<ScriptableRendererFeature> features;

    [Header("Fullscren Materials")]
    public Material Edge1;
    public Material Edge2;

    [Header("Door Metrics")]
    public int itemDraggedWrong;
    public int currentRoomIndex = 1;

    [Header("Inventory Metrics")]
    public int inventoryOpened;

    void Start()
    {
        Instance = this;

        SceneName = SceneManager.GetActiveScene().name;
        noise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        features = forwardRendererData.rendererFeatures;

        // Opening cutscenes
        if (startFromSecondFloor)
        {
            whitePanel.SetActive(false);
            blackPanel.SetActive(false);
            CMStart.SetActive(false);
            CMStart2.SetActive(false);
            CMStart3.SetActive(false);

            inventoryEnabled = true;
            player.transform.position = new Vector3(26, 40.4483452f, -34);
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
                PlayerMovement.Instance.moveDisabled = true;
                inventoryEnabled = false;
                pauseMenuEnabled = false;
                gameObject.GetComponent<SubtitleManager>().ShowSubtitle(firstSub);
            }
            else if (SceneName == "UnderScene")
            {
                for (int i = 0; i < ItemsCollectedFromAbove.Count; i++)
                {
                    InventoryCanvas.GetComponentInParent<InventoryManager>().AddItemWithoutReminder(ItemsCollectedFromAbove[i]);
                    //player.GetComponent<CharacterController>().enabled = false;
                    PlayerMovement.Instance.moveDisabled = true;
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
            LockPlayerCam();
            PlayerMovement.Instance.moveDisabled = true;
            inventoryEnabled = false;
            pauseMenuEnabled = false;
            CrosshairCanvas.SetActive(false);
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
                    pauseMenuEnabled = true;
                    if (!mainCam.GetComponent<Cinemachine.CinemachineBrain>().enabled)
                    {
                        mainCam.GetComponent<Cinemachine.CinemachineBrain>().enabled = true;
                    }

                    if (PlayerMovement.Instance.moveDisabled)
                    {
                        PlayerMovement.Instance.moveDisabled = false;
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
                inventoryOpened += 1;
                Cursor.visible = true;
                pauseMenuEnabled = false;
                InventoryCanvas.SetActive(true);
                CrosshairCanvas.SetActive(false);

                if (!PlayerMovement.Instance.moveDisabled)
                {
                    PlayerMovement.Instance.moveDisabled = true;
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
        yield return new WaitForSeconds(1f);
        playerAudio.clip = AfterOpening1;
        playerAudio.Play();
        yield return new WaitForSeconds(5f);
        CMStart2.SetActive(false);
        yield return new WaitForSeconds(5f);
        CMStart3.SetActive(false);
        playerAudio.clip = AfterOpening2;
        playerAudio.Play();
        yield return new WaitForSeconds(3.5f);

        UnlockPlayerCam();
        PlayerMovement.Instance.moveDisabled = false;
        inventoryEnabled = true;
        pauseMenuEnabled = true;
        CrosshairCanvas.SetActive(true);
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

    public void ControlRendererFeature(int i, bool active)
    {
        if (active)
        {
            features[i].SetActive(true);
        }
        else
        {
            features[i].SetActive(false);
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
        PlayerMovement.Instance.moveDisabled = false;
        inventoryEnabled = true;

    }

    IEnumerator TransitionToUnderworldCoroutine()
    {
        yield return new WaitForSeconds(3f);
        //player.GetComponent<CharacterController>().enabled = false;
        PlayerMovement.Instance.moveDisabled = true;
        inventoryEnabled = false;
        PlayerMovement.Instance.cam6DShakeOn = true;

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
        yield return new WaitForSeconds(10f);
        StartFadeInWhite();
        TV1VideoPlayer.Play();
        yield return new WaitForSeconds(2f);
        ambience.Play();
        BGM.Play();
        yield return new WaitForSeconds(3f);

        EnableOpeningMove();
        
    }


    public void PlayVideo1()
    {
        blackPanel.SetActive(true);
        blackPanel.GetComponent<Image>().color = new Color(0, 0, 0, 1);

        tunnelTone.Stop();
        ambience.Stop();
        BGM.Stop();

        //player.GetComponent<CharacterController>().enabled = false;
        PlayerMovement.Instance.moveDisabled = true;
        inventoryEnabled = false;
        pauseMenuEnabled = false;

        
        
        glass.SetActive(true);
        player.transform.position = new Vector3(26, 40.4483452f, -34);
        tunnel3to2.SetActive(false);

        videoScreen.SetActive(true);
        cutsceneVideoPlayer.Play();
        StartCoroutine(AfterVideo1());
    }


    IEnumerator AfterVideo1()
    {
        
        yield return new WaitForSeconds(29f);
        Tinylytics.AnalyticsManager.LogThirdFloorPlaytime();
        player.transform.position = new Vector3(26, 40.4483452f, -34);
        videoScreen.SetActive(false);
        StartFadeIn();
        ambience.Play();
        BGM.Play();
        yield return new WaitForSeconds(4f);

        PlayerMovement.Instance.moveDisabled = false;
        inventoryEnabled = true;
        pauseMenuEnabled = true;

    }

    public void CloseCam1()
    {
        cam1.SetActive(false);
    }

    public void LockPlayerCam()
    {
        vCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 0;
        vCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 0;
    }

    public void UnlockPlayerCam()
    {
        vCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 300;
        vCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 300;
    }

    private void OnApplicationQuit()
    {
        Tinylytics.AnalyticsManager.LogCustomMetric("Total Collected Items", InventoryManager.Instance.itemCollected.ToString());
        Tinylytics.AnalyticsManager.LogSessionPlaytime();
        Tinylytics.AnalyticsManager.LogCustomMetric("Inventory Opened Times", inventoryOpened.ToString());
    }
}
