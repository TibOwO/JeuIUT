using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform spawnPoint;

    void Start()
    {
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
        //        else
        //        {
        //            spawnPoint = point.transform;
        //            point = GameObject.Find("SpawnBase");
        //            GameObject player = GameObject.FindGameObjectWithTag("Player");
        //            player.transform.position = spawnPoint.position;
        //            Debug.Log("Player spawned at: " + spawnPointName);
        //        }
    }
}
