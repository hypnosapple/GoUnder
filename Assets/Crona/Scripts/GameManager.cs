using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.Video;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Cutscene Control")]
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
    public GameObject openingVideoScreen;
    public VideoPlayer openingVideoPlayer;
    public GameObject endVideoScreen;
    public VideoPlayer endVideoPlayer;
    public VideoPlayer TV1VideoPlayer;
    public VideoPlayer Lv1Projector;
    public VideoPlayer F2TV;

    [Header("Fade in/out Panel")]
    public GameObject blackPanel;
    public GameObject whitePanel;
    public GameObject controlsPanel;
    public GameObject upperBlack;
    public GameObject lowerBlack;

    public string SceneName;


    [Header("Underworld Level")]
    public List<ItemData_SO> ItemsCollectedFromAbove;

    private float blackT = 0f;
    public bool GroundLevelEnd;
    public GameObject blackCube;
    private bool GroundLevelFadedOut = false;
    public GameObject firstFloorRoom;

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
    public AudioSource tsunamiSFX;
    public AudioSource tape;
    public AudioSource heartbeat;


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

    //[Header("Door Metrics")]
    //public int itemDraggedWrong;
    //public int currentRoomIndex = 1;

    [Header("Inventory Metrics")]
    public int inventoryOpened;

    [Header("First Floor")]
    public GameObject loc1;
    public GameObject loc2;
    public GameObject loc3;
    public GameObject endBlock;
    public GameObject collider1;
    public GameObject collider2;
    public GameObject collider11;
    public GameObject collider12;
    public GameObject collider21;
    public GameObject collider22;

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
        else if (enableNewCutscenes && SceneName == "MainSceneF3")
        {
            
            CMStart.SetActive(true);
            CMStart2.SetActive(true);
            CMStart3.SetActive(true);
            upperBlack.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -80f);
            lowerBlack.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 80f);
            //player.GetComponent<CharacterController>().enabled = false;
            LockPlayerCam();
            PlayerMovement.Instance.moveDisabled = true;
            inventoryEnabled = false;
            pauseMenuEnabled = false;
            CrosshairCanvas.SetActive(false);
            StartCoroutine(NewOpening());
        }
        else if (SceneName == "MainSceneF3")
        {
            
            whitePanel.SetActive(false);
            blackPanel.SetActive(false);
            CMStart.SetActive(false);
            CMStart2.SetActive(false);
            CMStart3.SetActive(false);

            inventoryEnabled = true;
        }
        else if (SceneName == "MainSceneF2")
        {
            StartCoroutine(SceneF2Opening());
        }

        else if (SceneName == "MainSceneF1")
        {
            StartCoroutine(SceneF1Opening());
        }

        //EnableMove();
        //StartFadeIn();
    }

    
    void Update()
    {
        InventoryVisibility();
        TransitionToUnderworldInProgress();


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
                if (/*CommunicationSystem.FocusOnScreen == false &&*/ComputerSystem != null && ComputerSystem.FocusOnScreen == true)
                {
                    Cursor.visible = true;
                    InventoryCanvas.SetActive(false);
                    //CrosshairCanvas.SetActive(true);
                    


                }
                else
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
            }
            else
            {
                //inventoryOpened += 1;
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
        yield return new WaitForSeconds(0.5f);
        playerAudio.clip = AfterOpening1;
        playerAudio.Play();
        yield return new WaitForSeconds(4.5f);
        CMStart2.SetActive(false);
        yield return new WaitForSeconds(4.5f);
        CMStart3.SetActive(false);
        playerAudio.clip = AfterOpening2;
        playerAudio.Play();
        yield return new WaitForSeconds(3f);
        BlackBar(false);
        yield return new WaitForSeconds(1f);
        UnlockPlayerCam();
        PlayerMovement.Instance.moveDisabled = false;
        inventoryEnabled = true;
        pauseMenuEnabled = true;
        CrosshairCanvas.SetActive(true);

        controlsPanel.GetComponent<Image>().DOFade(1f, 1.5f);
        yield return new WaitForSeconds(10f);
        controlsPanel.GetComponent<Image>().DOFade(0f, 1.5f);
    }


    public void StartFadeIn()
    {
        if (blackPanel.GetComponent<Image>().color.a == 1)
        {
            //fadeInT = 0f;

            //fadeIn = true;
            blackPanel.GetComponent<Image>().DOFade(0f, 3.3f);
        }
    }



    public void StartFadeOut()
    {
        if (blackPanel.GetComponent<Image>().color.a == 0)
        {
            //fadeOutT = 0f;

            //fadeOut = true;
            blackPanel.GetComponent<Image>().DOFade(1f, 3.3f);
        }
    }


    public void StartFadeInWhite()
    {
        if (whitePanel.GetComponent<Image>().color.a == 1)
        {
            //WfadeInT = 0f;

            //WfadeIn = true;
            whitePanel.GetComponent<Image>().DOFade(0f, 3.3f);
        }
    }


    public void StartFadeOutWhite()
    {
        if (whitePanel.GetComponent<Image>().color.a == 0)
        {
            //WfadeOutT = 0f;

            //WfadeOut = true;
            whitePanel.GetComponent<Image>().DOFade(1f, 3.3f);
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
        whitePanel.SetActive(false);
        blackPanel.SetActive(true);
        openingVideoScreen.SetActive(true);
        openingVideoPlayer.Play();
        yield return new WaitForSeconds(50f);

        whitePanel.GetComponent<Image>().color = new Color(255, 255, 255, 0);
        whitePanel.SetActive(true);
        openingVideoScreen.SetActive(false);
        StartFadeOutWhite();
        yield return new WaitForSeconds(4f);
        blackPanel.SetActive(false);

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

        
        
        //glass.SetActive(true);
        //player.transform.position = new Vector3(26, 40.4483452f, -34);
        //tunnel3to2.SetActive(false);

        videoScreen.SetActive(true);
        upperBlack.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -80f);
        lowerBlack.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 80f);
        cutsceneVideoPlayer.Play();
        StartCoroutine(AfterVideo1());
    }


    IEnumerator AfterVideo1()
    {
        
        yield return new WaitForSeconds(28f);
        Tinylytics.AnalyticsManager.LogThirdFloorPlaytime();
        //player.transform.position = new Vector3(26, 40.4483452f, -34);
        StartCoroutine(LoadSceneF2());

        

    }

    IEnumerator LoadSceneF2()
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainSceneF2");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        
    }

    public void StartSecondFloorEnding()
    {
        StartCoroutine(SecondFloorEnding());
    }

    IEnumerator SecondFloorEnding()
    {
        yield return new WaitForSeconds(3f);
        BlackBar(true);

        LockPlayerCam();
        PlayerMovement.Instance.moveDisabled = true;
        inventoryEnabled = false;
        pauseMenuEnabled = false;
        Cursor.visible = false;
        CrosshairCanvas.SetActive(false);
        yield return new WaitForSeconds(1f);
        
        tsunamiSFX.Play();

        yield return new WaitForSeconds(3f);
        PlayerMovement.Instance.cam6DShakeOn = true;

        noise.m_NoiseProfile = screenShakeNoise;
        noise.m_AmplitudeGain = 6f;
        noise.m_FrequencyGain = 2.5f;

        

        yield return new WaitForSeconds(4f);
        tsunamiSFX.Stop();
        F2TV.Stop();
        blackPanel.SetActive(true);
        blackPanel.GetComponent<Image>().color = new Color(0, 0, 0, 1);
        yield return new WaitForSeconds(1f);
        PlayVideo2();
    }

    public void PlayVideo2()
    {
        //blackPanel.SetActive(true);
        //blackPanel.GetComponent<Image>().color = new Color(0, 0, 0, 1);

        tunnelTone.Stop();
        ambience.Stop();
        BGM.Stop();

        //player.GetComponent<CharacterController>().enabled = false;
        PlayerMovement.Instance.moveDisabled = true;
        inventoryEnabled = false;
        pauseMenuEnabled = false;



        //glass.SetActive(true);
        //player.transform.position = new Vector3(26, 40.4483452f, -34);
        //tunnel3to2.SetActive(false);

        videoScreen.SetActive(true);
        upperBlack.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -80f);
        lowerBlack.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 80f);
        cutsceneVideoPlayer.Play();
        StartCoroutine(AfterVideo2());
    }


    IEnumerator AfterVideo2()
    {

        yield return new WaitForSeconds(26f);
        
        //player.transform.position = new Vector3(26, 40.4483452f, -34);
        StartCoroutine(LoadSceneF1());



    }

    IEnumerator LoadSceneF1()
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainSceneF1");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }


    }

    IEnumerator SceneF2Opening()
    {
        blackPanel.SetActive(true);
        upperBlack.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -80f);
        lowerBlack.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 80f);

        LockPlayerCam();
        PlayerMovement.Instance.moveDisabled = true;
        inventoryEnabled = false;
        pauseMenuEnabled = false;
        CrosshairCanvas.SetActive(false);

        videoScreen.SetActive(false);
        StartFadeIn();
        ambience.Play();
        BGM.Play();
        yield return new WaitForSeconds(4f);

        BlackBar(false);
        yield return new WaitForSeconds(1f);
        UnlockPlayerCam();

        CrosshairCanvas.SetActive(true);
        PlayerMovement.Instance.moveDisabled = false;
        inventoryEnabled = true;
        pauseMenuEnabled = true;
    }

    IEnumerator SceneF1Opening()
    {
        blackPanel.SetActive(true);
        upperBlack.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -80f);
        lowerBlack.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 80f);

        LockPlayerCam();
        PlayerMovement.Instance.moveDisabled = true;
        inventoryEnabled = false;
        pauseMenuEnabled = false;
        CrosshairCanvas.SetActive(false);

        videoScreen.SetActive(false);
        StartFadeIn();
        ambience.Play();
        BGM.Play();
        yield return new WaitForSeconds(4f);

        BlackBar(false);
        yield return new WaitForSeconds(1f);
        UnlockPlayerCam();

        CrosshairCanvas.SetActive(true);
        PlayerMovement.Instance.moveDisabled = false;
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

    /*private void OnApplicationQuit()
    {
        if (InventoryManager.Instance.itemCollected > 1)
        {
            Tinylytics.AnalyticsManager.LogCustomMetric("Total Collected Items", InventoryManager.Instance.itemCollected.ToString());
            Tinylytics.AnalyticsManager.LogSessionPlaytime();
            Tinylytics.AnalyticsManager.LogCustomMetric("Inventory Opened Times", inventoryOpened.ToString());
        }
        
    }
    */

    public void BlackBar(bool active)
    {
        if (active)
        {
            upperBlack.GetComponent<RectTransform>().DOAnchorPosY(-80f, 0.8f);
            lowerBlack.GetComponent<RectTransform>().DOAnchorPosY(80f, 0.8f);
        }
        else
        {
            upperBlack.GetComponent<RectTransform>().DOAnchorPosY(80f, 0.8f);
            lowerBlack.GetComponent<RectTransform>().DOAnchorPosY(-80f, 0.8f);
        }
    }

    public void End()
    {
        StartCoroutine(WaitForEnd());
    }

    IEnumerator WaitForEnd()
    {
        yield return new WaitForSeconds(2f);
        heartbeat.Play();
        yield return new WaitForSeconds(7f);
        LockPlayerCam();
        PlayerMovement.Instance.moveDisabled = true;
        inventoryEnabled = false;
        pauseMenuEnabled = false;
        CrosshairCanvas.SetActive(false);

        whitePanel.GetComponent<Image>().color = new Color(255, 255, 255, 0);
        whitePanel.SetActive(true);
        StartFadeOutWhite();
        yield return new WaitForSeconds(7f);

        BGM.Stop();
        heartbeat.Stop();
        firstFloorRoom.SetActive(false);
        endVideoScreen.SetActive(true);
        blackPanel.SetActive(true);
        
        upperBlack.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -80f);
        lowerBlack.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 80f);
        endVideoPlayer.Play();
    }


    public void startTape()
    {
        StartCoroutine(ListenTape());
    }

    IEnumerator ListenTape()
    {
        yield return new WaitForSeconds(1f);
        tape.Play();
        yield return new WaitForSeconds(25f);
        loc1.SetActive(false);
        loc2.SetActive(true);
    }

    public void startProjector()
    {
        StartCoroutine(SeeProjector());
    }

    IEnumerator SeeProjector()
    {
        yield return new WaitForSeconds(1f);
        Lv1Projector.Play();
        yield return new WaitForSeconds(20f);
        loc2.SetActive(false);
        loc3.SetActive(true);
    }

    public void showLastDoor()
    {
        endBlock.SetActive(false);
        collider1.SetActive(false);
        collider2.SetActive(false);
    }

    public void blockFirstLoc()
    {
        collider11.SetActive(true);
        collider12.SetActive(true);
    }

    public void blockSecondLoc()
    {
        collider21.SetActive(true);
        collider22.SetActive(true);
    }
}
