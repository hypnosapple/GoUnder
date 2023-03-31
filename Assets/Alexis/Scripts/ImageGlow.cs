using UnityEngine;
using UnityEngine.UI;

public class ImageGlow : MonoBehaviour
{
    public float glowSpeed = 1f;    // The speed of the glow effect
    public float minAlpha = 0.1f;   // The minimum alpha value of the image
    public float maxAlpha = 1f;     // The maximum alpha value of the image

    private Image image;
    private float targetAlpha;

    void Start()
    {
        image = GetComponent<Image>();
        targetAlpha = minAlpha;
    }

    void Update()
    {
        // Calculate the new alpha value based on time
        float newAlpha = Mathf.Lerp(image.color.a, targetAlpha, Time.deltaTime * glowSpeed);

        // If the alpha value reaches the minimum or maximum, reverse the target alpha
        if (newAlpha <= minAlpha || newAlpha >= maxAlpha)
        {
            targetAlpha = targetAlpha == minAlpha ? maxAlpha : minAlpha;
        }

        // Set the new alpha value for the image color
        image.color = new Color(image.color.r, image.color.g, image.color.b, newAlpha);
    }
}
