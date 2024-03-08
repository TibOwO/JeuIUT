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
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, "Video", "porte_anim.mp4");
            videoPlayer.url = videoPath;

            videoPlayer.Play();
            videoPlayer.loopPointReached += LoadCouloirScene;
        }
    }

    void LoadCouloirScene(VideoPlayer vp)
    {
        Scene currScene = SceneManager.GetActiveScene();

        string spawnPoint = null;
        if (currScene.name == "Salle 1")
        {
            spawnPoint = "Salle 1";
        }
        else if (currScene.name == "Salle 2")
        {
            spawnPoint = "Salle 2";
        }
        else if (currScene.name == "Salle 3")
        {
            spawnPoint = "Salle 3";
        }
        else if (currScene.name == "Salle 4")
        {
            spawnPoint = "Salle 4";
        }
        else if (currScene.name == "Salle 5")
        {
            spawnPoint = "Salle 5";
        }
        else if (currScene.name == "Salle 6")
        {
            spawnPoint = "Salle 6";
        }
        else if (currScene.name == "Salle 7")
        {
            spawnPoint = "Salle 7";
        }
        else
        {
            Debug.LogError(currScene.name);
        }

        PlayerPrefs.SetString("PointDeSpawn", spawnPoint);

        SceneManager.LoadScene("Couloir");
    }
}
