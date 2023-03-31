using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraMoveAndFade : MonoBehaviour
{
    public Button button;
    public float moveDistance = 10f;
    public float moveSpeed = 2f;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        StartCoroutine(MoveCameraForward());
    }

    IEnumerator MoveCameraForward()
    {
        Vector3 targetPosition = mainCamera.transform.position + mainCamera.transform.forward * moveDistance;

        // Move camera forward
        while (Vector3.Distance(mainCamera.transform.position, targetPosition) > 0.01f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        mainCamera.transform.position = targetPosition;
    }
}
