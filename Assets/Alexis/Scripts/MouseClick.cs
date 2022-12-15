using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClick : MonoBehaviour
{
    public AudioSource click;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            click.enabled = true;
        }
        else
        {
            click.enabled = false;
        }

    }
}
