using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject videoObject;

    void Start()
    {
        
        if (UniversalVariables.Instance.playAnimation == true)
        {
            videoObject.SetActive(true);
            videoPlayer.Play();
        }

        // Subscribe to the loopPointReached event to handle video end
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // Video has reached the end, hide or disable the video GameObject
        videoObject.SetActive(false);
        UniversalVariables.Instance.playAnimation = false;
    }
}
