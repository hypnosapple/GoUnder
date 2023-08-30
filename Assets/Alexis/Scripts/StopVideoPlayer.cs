using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class StopVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    private void Update()
    {
        if (videoPlayer.isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                videoPlayer.Stop();
                SceneManager.LoadScene("Menu Scene");
            }
        }
    }
}
