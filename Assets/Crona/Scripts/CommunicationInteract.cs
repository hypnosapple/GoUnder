using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommunicationInteract : MonoBehaviour
{
    public GameObject InteractiveCanvas;
    public Transform ComputerUsingPosition;
    public Camera ComputerCamera;
    private int IsInPos = 0;
    private Camera MainCam;
    private bool IsInside;

    public GameObject IslandCode;
    public GameObject CommunicationCode;
    public GameObject sendButton;
    public GameObject Crosshair;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            InteractiveCanvas.SetActive(true);
            IsInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            InteractiveCanvas.SetActive(false);
            IsInside = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!IsInside)
            return;

        if (Input.GetKey(KeyCode.U))
        {

            if (IsInPos == 0)
            {
                
                ComputerCamera.transform.position = Camera.main.transform.position;
                ComputerCamera.transform.rotation = Camera.main.transform.rotation;
                MainCam = Camera.main;
                Camera.main.enabled = false;
                ComputerCamera.enabled = true;
                StartCoroutine(LerpingPlayerToComputerPos(ComputerUsingPosition.position, ComputerUsingPosition.rotation, 1f, ComputerCamera.transform));
            }
            else if (IsInPos == 2)
            {
                StartCoroutine(LerpingPlayerBackToMain(MainCam.transform.position, MainCam.transform.rotation, 1f, ComputerCamera.transform));
            }
        }
    }

    IEnumerator LerpingPlayerToComputerPos(Vector3 targetPosition, Quaternion targetRotation, float duration, Transform Transformee)
    {
        Crosshair.SetActive(false);
        IsInPos = 1;
        float time = 0;
        InteractiveCanvas.SetActive(false);
        Vector3 startPosition = Transformee.position;
        Quaternion startRotation = Transformee.rotation;
        while (time < duration)
        {
            Transformee.rotation = Quaternion.Lerp(startRotation, targetRotation, time / duration);
            Transformee.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        Transformee.position = targetPosition;
        Transformee.rotation = targetRotation;

        IsInPos = 2;
        //IslandCode.SetActive(true);
        //CommunicationCode.SetActive(true);
        //sendButton.SetActive(true);
        

    }

    

    IEnumerator LerpingPlayerBackToMain(Vector3 targetPosition, Quaternion targetRotation, float duration, Transform Transformee)
    {
        //IslandCode.SetActive(false);
        //CommunicationCode.SetActive(false);
        //sendButton.SetActive(false);

        IsInPos = 1;
        float time = 0;
        Vector3 startPosition = Transformee.position;
        Quaternion startRotation = Transformee.rotation;
        while (time < duration)
        {
            Transformee.rotation = Quaternion.Lerp(startRotation, targetRotation, time / duration);
            Transformee.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        Transformee.position = targetPosition;
        Transformee.rotation = targetRotation;

        IsInPos = 0;
        InteractiveCanvas.SetActive(true);
        Crosshair.SetActive(true);
        MainCam.enabled = true;
        ComputerCamera.enabled = false;
    }
}
