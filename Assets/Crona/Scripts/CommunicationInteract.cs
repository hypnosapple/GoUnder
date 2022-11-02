using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CommunicationInteract : MonoBehaviour
{
    public GameObject UseECanvas, ExitCanvas;
    public Transform ComputerUsingPosition;
    public Camera ComputerCamera;
    public GameObject EricScreen;
    private int IsInPos = 0;
    private Camera MainCam;
    private bool IsInside;
    public bool FocusOnScreen = false;

    public TMP_InputField IslandCode;
    public TMP_InputField CommunicationCode;
    public GameObject sendButton;
    public GameObject Crosshair;

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            UseECanvas.SetActive(true);
            IsInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            UseECanvas.SetActive(false);
            IsInside = false;
        }
    }
    */

    private void Update()
    {
        if (FocusOnScreen && Input.GetKey(KeyCode.Escape))
        {
            FocusOnScreen = false;
            Cursor.visible = false;
            if (IsInPos == 2)
            {
                CommunicationCode.interactable = false;
                IslandCode.interactable = false;
                Debug.Log("InputField deactivated");
                StartCoroutine(LerpingPlayerBackToMain(MainCam.transform.position, MainCam.transform.rotation, 1f, ComputerCamera.transform));
                IslandCode.DeactivateInputField();
            }
        }
    }

    public void ToScreen()
    {
        /*
        if (!IsInside)
            return;
        */

            FocusOnScreen = true;
            Cursor.visible = true;
            CommunicationCode.interactable = true;
            IslandCode.interactable = true;
            if (IsInPos == 0)
            {
                Debug.Log("InputField activated");
                ComputerCamera.transform.position = Camera.main.transform.position;
                ComputerCamera.transform.rotation = Camera.main.transform.rotation;
                MainCam = Camera.main;
                Camera.main.enabled = false;
                EricScreen.SetActive(false);
                ComputerCamera.enabled = true;
                StartCoroutine(LerpingPlayerToComputerPos(ComputerUsingPosition.position, ComputerUsingPosition.rotation, 1f, ComputerCamera.transform));
                IslandCode.ActivateInputField();
            }
        
        
    }

    IEnumerator LerpingPlayerToComputerPos(Vector3 targetPosition, Quaternion targetRotation, float duration, Transform Transformee)
    {
        Crosshair.SetActive(false);
        IsInPos = 1;
        float time = 0;
        UseECanvas.SetActive(false);
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
        ExitCanvas.SetActive(true);
        IsInPos = 2;

    }

    

    IEnumerator LerpingPlayerBackToMain(Vector3 targetPosition, Quaternion targetRotation, float duration, Transform Transformee)
    {
        ExitCanvas.SetActive(false);
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
        //UseECanvas.SetActive(true);
        Crosshair.SetActive(true);
        MainCam.enabled = true;
        EricScreen.SetActive(true);
        ComputerCamera.enabled = false;
        //eric added
        ComputerCamera.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
    }
}
