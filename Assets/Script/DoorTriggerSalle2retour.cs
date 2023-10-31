using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class DoorTriggerSalle2retour : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D called");

        if (other.CompareTag("Player"))
        {
            videoPlayer.Play();
            videoPlayer.loopPointReached += LoadCouloirScene;

            // Mettre à jour le point de spawn dans les PlayerPrefs
            PlayerPrefs.SetString("PointDeSpawn", "SpawnPorteSalle2");
        }
    }

    void LoadCouloirScene(VideoPlayer vp)
    {
        SceneManager.LoadScene("Couloir");
    }
}
