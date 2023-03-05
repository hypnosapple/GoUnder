using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableText : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector3 offset;
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
        if(checkText.matches && !used)
        {
            //Destroy(this.gameObject);
            gameObject.GetComponent<RectTransform>().anchoredPosition3D = originalPos;
            used = true;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("dragbegin");
        offset = transform.position - GetWorldPosition(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = offset + GetWorldPosition(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition3D = originalPos;
    }

    private Vector3 GetWorldPosition(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return transform.position;

    }

    
}




