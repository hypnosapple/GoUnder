using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableText : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private Vector3 offset;

    CheckText checkText;


    public void Start()
    {
        checkText = FindObjectOfType<CheckText>();
    }


    public void Update()
    {
        if(checkText.matches == true)
        {
            Destroy(this.gameObject);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        offset = transform.position - GetWorldPosition(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = GetWorldPosition(eventData.position) + offset;
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




