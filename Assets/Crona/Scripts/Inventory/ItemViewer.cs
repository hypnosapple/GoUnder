using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemViewer : MonoBehaviour, IDragHandler
{
    public Transform itemModel;

    public void OnDrag(PointerEventData eventData)
    {
        itemModel.eulerAngles += new Vector3(eventData.delta.y, -eventData.delta.x);
    }
}
