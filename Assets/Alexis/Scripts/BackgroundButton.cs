using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BackgroundButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image backgroundImage;

    private Button button;

    private void Start()
    {
        // get the Button component
        button = GetComponent<Button>();

        // hide the background image at the start
        backgroundImage.enabled = false;

        // register event listeners for button events
        button.onClick.AddListener(ShowBackgroundImage);

        // get the EventTrigger component
        EventTrigger trigger = GetComponent<EventTrigger>();

        // create an event entry for PointerEnter and add it to the EventTrigger
        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((data) => { OnPointerEnter((PointerEventData)data); });
        trigger.triggers.Add(entryEnter);

        // create an event entry for PointerExit and add it to the EventTrigger
        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((data) => { OnPointerExit((PointerEventData)data); });
        trigger.triggers.Add(entryExit);
    }

    private void ShowBackgroundImage()
    {
        // show the background image
        backgroundImage.enabled = true;
    }

    private void HideBackgroundImage()
    {
        // hide the background image
        backgroundImage.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // show the background image when pointer enters the button
        ShowBackgroundImage();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // hide the background image when pointer exits the button
        HideBackgroundImage();
    }
}
