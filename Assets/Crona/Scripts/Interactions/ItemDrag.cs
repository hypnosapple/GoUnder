using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrag : MonoBehaviour
{
    Vector3 offset;
    private Vector3 originalPos;
    public CheckText checkText;
    private bool used;

    public void Start()
    {

        used = false;
        originalPos = gameObject.GetComponent<RectTransform>().anchoredPosition3D;
    }

    public void Update()
    {
        if (checkText.matches && !used)
        {
            Debug.Log("used");
            gameObject.GetComponent<RectTransform>().anchoredPosition3D = originalPos;
            used = true;
        }
    }

    void OnMouseDown()
    {
        offset = transform.position - MouseWorldPosition();
    }

    void OnMouseDrag()
    {
        transform.position = MouseWorldPosition() + offset;
    }

    void OnMouseUp()
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition3D = originalPos;
    }

    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }
}
