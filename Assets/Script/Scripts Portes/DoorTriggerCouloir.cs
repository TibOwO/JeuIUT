using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class DoorTriggerCouloir : MonoBehaviour
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
            videoPlayer.loopPointReached += LoadCouloirScene;

            // Mettre à jour le point de spawn dans les PlayerPrefs
            PlayerPrefs.SetString("PointDeSpawn", "SpawnPorteSalle1");
        }
    }

    void LoadCouloirScene(VideoPlayer vp)
    {
        SceneManager.LoadScene("Couloir");
    }
}
