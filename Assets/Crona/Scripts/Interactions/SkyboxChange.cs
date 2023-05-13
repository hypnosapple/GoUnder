using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SkyboxChange : MonoBehaviour
{
    public static SkyboxChange Instance;

    private bool afternoonChanged;
    private bool nightChanged;
    private bool inTunnel;
    private bool closeTunnelDoor;
    private bool wallChanged;
    private bool corridorClosed;

    [Header("Materials")]
    public Material afternoonSkybox;
    public Material nightSkybox;

    public Material afternoonWater;
    public Material nightWater;

    [Header("Object References")]
    public GameObject sea;
    public GameObject tunnel3to2;
    public List<GameObject> blockedObjects;
    public GameObject blockCollider;
    public GameObject tunnelBlock;
    public GameObject tunnelDoor;
    public GameObject tunnelTV;
    public GameObject changeWallTrigger;
    public GameObject oldWall;
    public GameObject newWall;
    public GameObject corridorBlock;
    public VideoPlayer F2TV;
    public SubtitleData_SO lastPhoneCall;

    public AudioSource tunnelTone;

    void Start()
    {
        Instance = this;

        afternoonChanged = false;
        nightChanged = false;

    }

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ToAfternoon")
        {
            
            if (!afternoonChanged)
            {
                tunnel3to2.SetActive(true);
                tunnelTV.GetComponent<VideoPlayer>().Play();
                blockCollider.SetActive(true);
                //tunnelTone.Play();
                foreach (GameObject blockedObject in blockedObjects){
                    blockedObject.SetActive(false);
                }
                sea.SetActive(false);
                //RenderSettings.skybox = afternoonSkybox;
                //sea.GetComponent<MeshRenderer>().material = afternoonWater;
                afternoonChanged = true;
                
            }
        }

        else if (other.gameObject.tag == "CloseTunnelDoor")
        {
            if (!closeTunnelDoor)
            {
                
                tunnelDoor.GetComponent<TunnelDoor>().CloseDoor();
                closeTunnelDoor = true;
            }
        }

        else if (other.gameObject.tag == "EnterTunnel")
        {
            if (!inTunnel)
            {
                tunnelTone.Play();
                GameManager.Instance.ControlRendererFeature(0, true);
                tunnelBlock.SetActive(true);
                tunnelDoor.GetComponent<TunnelDoor>().CloseDoor();
                inTunnel = true;
            }
        }
                

        else if (other.gameObject.tag == "ToNight")
        {

            if (!nightChanged)
            {
                
                //RenderSettings.skybox = nightSkybox;
                //sea.GetComponent<MeshRenderer>().material = nightWater;
                nightChanged = true;
            }
        }

        else if (other.gameObject.tag == "WallTrigger")
        {

            if (!wallChanged)
            {

                oldWall.SetActive(false);
                newWall.SetActive(true);
                wallChanged = true;
                F2TV.Play();
                StartCoroutine(WaitForPhoneCall(lastPhoneCall));
            }
        }

        

        else if (other.gameObject.tag == "CorridorTrigger")
        {

            if (!corridorClosed)
            {

                corridorBlock.SetActive(true);
                GameManager.Instance.loc1.SetActive(true);
                
                corridorClosed = true;
            }
        }
    }

    IEnumerator WaitForPhoneCall(SubtitleData_SO phoneCall)
    {
        yield return new WaitForSeconds(3f);
        SubtitleManager.Instance.ShowSubtitle(phoneCall);
    }

}
