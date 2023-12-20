using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class DoorTriggerSalle2 : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D called");

        if (other.CompareTag("Player"))
        {

            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, "Video", "porte_anim.mov");
            videoPlayer.url = videoPath;

            videoPlayer.Play();
            videoPlayer.loopPointReached += LoadSalle2Scene; // S'abonner à l'événement de fin de vidéo
        }
    }

    void LoadSalle2Scene(VideoPlayer vp)
    {
        SceneManager.LoadScene("Salle 2");
    }
}
