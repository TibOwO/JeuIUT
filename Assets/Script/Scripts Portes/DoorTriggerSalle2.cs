using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class DoorTrigger2 : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D called");

        if (other.CompareTag("Player"))
        {
            videoPlayer.Play();
            videoPlayer.loopPointReached += LoadSalle2Scene; // S'abonner � l'�v�nement de fin de vid�o
        }
    }

    void LoadSalle2Scene(VideoPlayer vp)
    {
        SceneManager.LoadScene("Salle 2");
    }
}
