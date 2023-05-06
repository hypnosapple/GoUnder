using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{
    public AudioSource buttonAudioSource;
    public VideoPlayer videoPlayer;
    public RawImage rawImage;

    private void Start()
    {
        videoPlayer.Stop();
        videoPlayer.loopPointReached += OnVideoFinished;
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
        buttonAudioSource.Play();
        rawImage.texture = null;
        InitializeVideoPlayer();
        PlayVideo();
    }

    public void InitializeVideoPlayer()
    {
        videoPlayer.Prepare();
    }

    public void PlayVideo()
    {
        if (!videoPlayer.isPlaying)
        {
            rawImage.enabled = true;
            rawImage.texture = videoPlayer.targetTexture;
            videoPlayer.Play();
        }
    }

    public void Exit()
    {
        buttonAudioSource.Play();
        Application.Quit();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(1);
    }
}
