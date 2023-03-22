using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosetInteract : MonoBehaviour
{
    private Animator closetAnimator;
    private bool closetOpened;

    public bool isMoving;

    void Start()
    {
        closetOpened = false;
        isMoving = false;
        closetAnimator = gameObject.GetComponent<Animator>();
    }


    public void openClosetDoor()
    {
        if (!closetOpened)
        {
            closetAnimator.SetTrigger("OpenDoor");
            //drawerOpen.Play();
            closetAnimator.SetBool("Closed", false);
            closetOpened = true;
            StartCoroutine(ClosetMoving());
        }
        else
        {
            closetAnimator.SetTrigger("CloseDoor");
            closetOpened = false;
            //drawerClose.Play();
            StartCoroutine(ClosetMoving());
        }
    }


    IEnumerator ClosetMoving()
    {
        isMoving = true;
        yield return new WaitForSeconds(2f);

        if (!closetOpened)
        {
            closetAnimator.SetBool("Closed", true);

        }

        isMoving = false;
    }

}
