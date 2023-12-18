using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D called");

        if (other.CompareTag("Player") && CleManager.Instance.HasKey)
        {
            videoPlayer.Play();
            videoPlayer.loopPointReached += LoadSalle1Scene; // S'abonner � l'�v�nement de fin de vid�o
        }
        else if (other.CompareTag("Player") && !CleManager.Instance.HasKey)
        {
            Debug.Log("You don't have the key");
        }
    }

    void LoadSalle1Scene(VideoPlayer vp)
    {
        SceneManager.LoadScene("Salle 1");
    }
}
