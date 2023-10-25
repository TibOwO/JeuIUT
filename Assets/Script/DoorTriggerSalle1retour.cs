using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class DoorTriggerSalle1retour : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D called");

        if (other.CompareTag("Player"))
        {
            videoPlayer.Play();
            videoPlayer.loopPointReached += LoadCouloirScene; // S'abonner � l'�v�nement de fin de vid�o
        }
    }

    void LoadCouloirScene(VideoPlayer vp)
    {
        SceneManager.LoadScene("Couloir");
    }
}
