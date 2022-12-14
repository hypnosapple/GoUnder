using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

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

    public GameObject CMUnderStart1;
    public GameObject CMUnderStart2;

    public GameObject blackPanel;
    private float fadeInT = 0f;
    private bool fadeIn = false;
    private float fadeOutT = 0f;
    private bool fadeOut = false;

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


    void Start()
    {
        SceneName = SceneManager.GetActiveScene().name;
        noise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (SceneName == "MainScene")
        {
            player.GetComponent<CharacterController>().enabled = false;
            gameObject.GetComponent<SubtitleManager>().ShowSubtitle(firstSub);
        }
        else if (SceneName == "UnderScene")
        {
            for (int i = 0; i < ItemsCollectedFromAbove.Count; i ++)
            {
                InventoryCanvas.GetComponentInParent<InventoryManager>().AddItemWithoutReminder(ItemsCollectedFromAbove[i]);
                player.GetComponent<CharacterController>().enabled = false;
                StartCoroutine(UnderWorldOpening());
            }
            
        }

        
        //EnableMove();
        //StartFadeIn();
    }

    
    void Update()
    {
        InventoryVisibility();
        TransitionToUnderworldInProgress();
        

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

    public void EnableOpeningMove()
    {
        player.GetComponent<CharacterController>().enabled = true;
        StartCoroutine(SwitchOpeningCam());
    }

    IEnumerator SwitchOpeningCam()
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
        player.GetComponent<CharacterController>().enabled = true;

    }

    IEnumerator TransitionToUnderworldCoroutine()
    {
        yield return new WaitForSeconds(3f);
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<PlayerMovement>().moveDisabled = true;
        player.GetComponent<PlayerMovement>().cam6DShakeOn = true;

        noise.m_NoiseProfile = screenShakeNoise;
        noise.m_AmplitudeGain = 6f;
        noise.m_FrequencyGain = 2.5f;

        blackCube.SetActive(true);
        GroundLevelEnd = true;
        playerAudio.clip = GroundEndPanic;
        playerAudio.Play();
    }
}
