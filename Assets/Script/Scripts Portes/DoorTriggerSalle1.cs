using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D called");

        if (other.CompareTag("Player"))
        {
            videoPlayer.Play();
            videoPlayer.loopPointReached += LoadSalle1Scene; // S'abonner à l'événement de fin de vidéo
        }
    }

    void LoadSalle1Scene(VideoPlayer vp)
    {
        SceneManager.LoadScene("Salle 1");
    }
}
