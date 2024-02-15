using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform spawnPoint;

    void Start()
    {
        PlayerPrefs.SetString("PointDeSpawn", null);
        // Get the name of the spawn point stored in PlayerPrefs
        string spawnPointName = PlayerPrefs.GetString("PointDeSpawn");

        // Find the spawn point in the scene
        GameObject point = GameObject.Find(spawnPointName);

        if (point != null)
        {
            spawnPoint = point.transform;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = spawnPoint.position;
            Debug.Log("Player spawned at: " + spawnPointName);
        }
    }
}
