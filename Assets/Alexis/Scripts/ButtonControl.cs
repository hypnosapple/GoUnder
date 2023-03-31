using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonControl : MonoBehaviour
{
    public float speed = 100f;              // The speed of the image movement
    public float distance = 1000f;          // The distance the image should move
    public float delay = 1f;                // The delay before the images start moving

    private Vector3[] originalPositions;    // The original positions of the images
    private bool isMoving;                  // Whether the images are currently moving

    private void Start()
    {

    }
    // Start is called before the first frame update
    public void NewGame()
    {

        //SceneManager.LoadScene(0);
    }

    public void Continue()
    {

    }

    public void Credits()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }
}
