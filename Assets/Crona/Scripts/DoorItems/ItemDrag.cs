using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDrag : MonoBehaviour
{
    Vector3 offset;
    private Vector3 originalPos;
    public CheckText checkText;
    private bool selected;

    public void Awake()
    {
        selected = false;
        originalPos = gameObject.GetComponent<RectTransform>().anchoredPosition3D;
    }

    public void Update()
    {
        if (checkText != null)
        {
            if (checkText.matches)
            {
                
                gameObject.GetComponentInParent<ItemList>().itemsInThisPage -= 1;
                if (gameObject.GetComponentInParent<ItemList>().itemsInThisPage == 0 && transform.parent.name != "Page1")
                {
                    gameObject.GetComponentInParent<PageArrows>().ResetToFirstPage();
                    gameObject.GetComponentInParent<PageArrows>().pageList.Remove(transform.parent.gameObject);
                }
                
                Destroy(gameObject);
                /*StartCoroutine(SetItemActive());
                gameObject.GetComponent<Image>().enabled = false;
                gameObject.GetComponent<RectTransform>().anchoredPosition3D = originalPos;
                used = true;
                */
            }
        }
        
    }

    void OnMouseDown()
    {
        offset = transform.position - MouseWorldPosition();
    }

    void OnMouseDrag()
    {
        if (!selected)
        {
            transform.position = MouseWorldPosition() + offset;
        }
        
        
    }

    void OnMouseUp()
    {
        resetItemPosition();
        if (selected)
        {
            selected = false;
        }
    }

    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }

    IEnumerator SetItemActive()
    {
        yield return new WaitForSeconds(0.99f);
        gameObject.GetComponent<Image>().enabled = true;
    }

    public void resetItemPosition()
    {
        selected = true;
        gameObject.SetActive(false);
        gameObject.SetActive(true);
        gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(gameObject.GetComponent<RectTransform>().anchoredPosition3D.x, gameObject.GetComponent<RectTransform>().anchoredPosition3D.y, 0);
    }
}
