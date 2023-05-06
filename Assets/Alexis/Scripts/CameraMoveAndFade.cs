using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraMoveAndFade : MonoBehaviour
{
    public Button button;
    public float moveSpeed = 2f;
    public GameObject targetObject;

    private Camera mainCamera;
    bool startMoving = false;

    public CameraShake cameraShake;

    void Start()
    {
        mainCamera = Camera.main;
        button.onClick.AddListener(OnButtonClick);
    }

    private void Update()
    {
        if (startMoving)
        {
            Vector3 direction = new Vector3(0, 0, targetObject.transform.position.z - mainCamera.transform.position.z).normalized;
            mainCamera.transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    void OnButtonClick()
    {
        startMoving = true;
        cameraShake.enabled = false;

    }
}

