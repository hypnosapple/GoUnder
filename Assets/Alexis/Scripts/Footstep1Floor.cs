using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Footstep1Floor : MonoBehaviour
{
    public AudioSource footstep;
    public AudioSource runstep;
    public GameObject player;


    private void Update()
    {
        if (!player.GetComponent<PlayerMovement>().moveDisabled)
        {
            if (Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.S)|| Input.GetKey(KeyCode.D))
            {
                
                runstep.enabled = false;
                footstep.enabled = true;
                
                
            }
            else
            {
                footstep.enabled = false;
                runstep.enabled = false;
            }
        }
        else
        {
            footstep.enabled = false;
            runstep.enabled = false;
        }
        
    }
}