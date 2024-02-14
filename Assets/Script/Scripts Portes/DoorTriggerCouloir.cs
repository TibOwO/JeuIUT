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
        // Get current scene
        Scene currScene = SceneManager.GetActiveScene();

        // Determine spawn point based on the current scene
        string spawnPoint = null;
        if (currScene.name == "Salle 1")
        {
            spawnPoint += "Salle1";
        }
        else if (currScene.name == "Salle 2")
        {
            spawnPoint += "Salle2";
        }
        else if (currScene.name == "Salle 3")
        {
            spawnPoint += "Salle3";
        }
        else if (currScene.name == "Salle 4")
        {
            spawnPoint += "Salle4";
        }
        else if (currScene.name == "Salle 5")
        {
            spawnPoint += "Salle5";
        }
        else if (currScene.name == "Salle 6")
        {
            spawnPoint += "Salle6";
        }
        else if (currScene.name == "Salle 7")
        {
            spawnPoint += "Salle7";
        }

        // Set spawn point
        PlayerPrefs.SetString("PointDeSpawn", spawnPoint);

        // Load Couloir scene
        SceneManager.LoadScene("Couloir");
    }
}
