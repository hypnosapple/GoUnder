using UnityEngine;

public class SpacebarToFlicker : MonoBehaviour
{
   public LightFlickering lightFlickering;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            lightFlickering.Flicker();
        }
    }
}
