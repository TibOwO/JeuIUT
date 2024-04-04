using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform spawnPoint;

    void Awake()
    {
        Debug.Log("PlayerSpawner Start called");
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
        else
        {
            Debug.Log("Spawn point not found, spawning at default spawn point");
            point = GameObject.Find("SpawnBase");
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = spawnPoint.position;
            Debug.Log("Player spawned at: " + spawnPointName);
        }
    }
}
