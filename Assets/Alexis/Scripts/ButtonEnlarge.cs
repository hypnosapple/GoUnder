using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEnlarge : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    public float pressedScaleFactor = 1.2f;     // The scale factor of the image when pressed
    public float highlightedScaleFactor = 1.1f; // The scale factor of the image when highlighted

    private Image image;
    private Vector3 originalScale;

    void Start()
    {
        image = GetComponent<Image>();
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Enlarge the image when pressed
        transform.localScale = originalScale * pressedScaleFactor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Restore the original size when released
        transform.localScale = originalScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Enlarge the image when highlighted
        transform.localScale = originalScale * highlightedScaleFactor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Restore the original size when the pointer exits the button
        transform.localScale = originalScale;
    }
}
