using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIInteract : MonoBehaviour
{
    public GameObject InteractiveCanvas;
    public Transform ComputerUsingPosition;
    public Camera ComputerCamera;
    private int IsInPos = 0;
    private Camera MainCam;
    private bool IsInside;
    public GameObject ComputerScreen;
    public GameObject ComputerBackground;
    public GameObject OtherpartOfComputer;
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerMovement>() != null)
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
            Debug.Log("I am pressed");
            if (IsInPos == 0)
            {
                //other.GetComponent<PlayerMovement>().enabled = false;
                //other.GetComponent<CharacterController>().enabled = false;
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
        //targetTransform.gameObject.SetActive(false);
        Debug.Log("I am locked");
        IsInPos = 2;
        ComputerScreen.SetActive(true);
        //StartCoroutine(StartComputer());
    }

    IEnumerator StartComputer()
    {
        OtherpartOfComputer.SetActive(false);
        float time = 0;
        float duration = 0.5f;
        Color tempColor = new Color(1, 1, 1, 0);
        while (time < duration)
        {
            ComputerBackground.GetComponent<RawImage>().color = Color.Lerp(tempColor, Color.white, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        ComputerBackground.GetComponent<RawImage>().color = Color.white;
        OtherpartOfComputer.SetActive(true);
    }

    IEnumerator EndComputer()
    {
        OtherpartOfComputer.SetActive(false);
        float time = 0;
        float duration = 0.2f;
        Color tempColor = new Color(1, 1, 1, 0);
        while (time < duration)
        {
            ComputerBackground.GetComponent<RawImage>().color = Color.Lerp(Color.white, tempColor, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        ComputerBackground.GetComponent<RawImage>().color = tempColor;
    }

    IEnumerator LerpingPlayerBackToMain(Vector3 targetPosition, Quaternion targetRotation, float duration, Transform Transformee)
    {
        //ComputerScreen.SetActive(false);
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
        //targetTransform.gameObject.SetActive(false);
        Debug.Log("I am free");
        IsInPos = 0;
        InteractiveCanvas.SetActive(true);
        MainCam.enabled = true;
        ComputerCamera.enabled = false;
    }

}
