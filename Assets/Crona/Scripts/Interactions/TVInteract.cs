using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVInteract : MonoBehaviour
{
    public GameObject TVCam;
    public GameObject EPanel;
    public GameObject crosshairCanvas;

    private bool TVOn = false;


    private void Update()
    {
        if (TVCam != null && TVOn)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                TVOn = false;
                EPanel.SetActive(false);
                TVCam.SetActive(false);
                StartCoroutine(WaitCameraOut());
            }
        }
    }


    public void SwitchTV()
    {
        if (TVCam != null)
        {
            if (!TVOn)
            {
                TVCam.SetActive(true);
                StartCoroutine(WaitCameraIn());
                GameManager.Instance.LockPlayerCam();
                PlayerInteraction.Instance.interactAllowed = false;

                if (!PlayerMovement.Instance.moveDisabled)
                {
                    PlayerMovement.Instance.moveDisabled = true;
                }

                if (crosshairCanvas.activeInHierarchy)
                {
                    crosshairCanvas.SetActive(false);
                }
            }
        }
    }


    IEnumerator WaitCameraIn()
    {
        yield return new WaitForSeconds(1f);
        EPanel.SetActive(true);
        TVOn = true;
    }

    IEnumerator WaitCameraOut()
    {
        yield return new WaitForSeconds(1f);

        PlayerMovement.Instance.moveDisabled = false;
        crosshairCanvas.SetActive(true);
        GameManager.Instance.UnlockPlayerCam();
        PlayerInteraction.Instance.interactAllowed = true;
    }
}
