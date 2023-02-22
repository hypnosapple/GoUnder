using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerInteract : MonoBehaviour
{
    private bool drawerOpened;
    
    public bool isMoving;

    public Animator drawerAnimator;

    public AudioSource drawerOpen;
    public AudioSource drawerClose;

    void Start()
    {
        drawerOpened = false;
        isMoving = false;
        drawerAnimator = gameObject.GetComponent<Animator>();
    }


    public void MoveDrawer()
    {
        if (!drawerOpened)
        {
            drawerAnimator.SetTrigger("OpenDrawer");
            drawerOpen.Play();
            drawerAnimator.SetBool("Closed", false);
            drawerOpened = true;
            StartCoroutine(DrawerMoving());
        }
        else
        {
            drawerAnimator.SetTrigger("CloseDrawer");
            drawerOpened = false;
            StartCoroutine(DrawerMoving());
        }
    }


    IEnumerator DrawerMoving()
    {
        isMoving = true;
        yield return new WaitForSeconds(1.5f);

        if (!drawerOpened)
        {
            drawerAnimator.SetTrigger("Closed");
            drawerClose.Play();
        }
        
        isMoving = false;
    }
}
