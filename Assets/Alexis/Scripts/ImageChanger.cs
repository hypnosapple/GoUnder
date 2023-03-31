using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageChanger : MonoBehaviour
{
    public Image image1;
    public Image image2;
    public float delay = 1f; // the delay between image changes

    private bool isImage1Active = true;

    private void Start()
    {
        // start the coroutine to change images
        StartCoroutine(ChangeImages());
    }

    private IEnumerator ChangeImages()
    {
        while (true)
        {
            // wait for the specified delay
            yield return new WaitForSeconds(delay);

            // toggle between the two images
            if (isImage1Active)
            {
                image1.enabled = false;
                image2.enabled = true;
                isImage1Active = false;
            }
            else
            {
                image1.enabled = true;
                image2.enabled = false;
                isImage1Active = true;
            }
        }
    }
}
