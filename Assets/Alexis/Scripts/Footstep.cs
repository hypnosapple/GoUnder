using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Footstep : MonoBehaviour
{
    public AudioSource footstep;
    public GameObject player;

    private void Update()
    {
        if (!player.GetComponent<PlayerMovement>().moveDisabled)
        {
            if (Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.S)|| Input.GetKey(KeyCode.D))
            {
                footstep.enabled = true;
            }
            else
            {
                footstep.enabled = false;
            }
        }
        else
        {
            footstep.enabled = false;
        }
        
    }
}
