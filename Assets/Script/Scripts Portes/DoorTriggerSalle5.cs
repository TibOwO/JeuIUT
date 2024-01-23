using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class DoorTriggerSalle5 : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D called");

        if (other.CompareTag("Player") && ElementalInventory.Instance.contains("Cle 5", 1))
        {
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, "Video", "porte_anim.mp4");
            videoPlayer.url = videoPath;

            videoPlayer.Play();
            videoPlayer.loopPointReached += LoadSalle5Scene; // S'abonner à l'événement de fin de vidéo
        }
        else if (other.CompareTag("Player") && !ElementalInventory.Instance.contains("Cle 5", 1))
        {
            Debug.Log("Vous n'avez pas la clé nécessaire.");
        }
    }

    void LoadSalle5Scene(VideoPlayer vp)
    {
        SceneManager.LoadScene("Salle 5");
    }
}
