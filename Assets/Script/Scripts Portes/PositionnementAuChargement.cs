using UnityEngine;

public class PositionnementAuChargement : MonoBehaviour
{
    void Start()
    {
        // V�rifie si le PointDeSpawn existe dans les PlayerPrefs
        if (PlayerPrefs.HasKey("PointDeSpawn"))
        {
            // R�cup�re le nom du point de spawn
            string pointDeSpawnNom = PlayerPrefs.GetString("PointDeSpawn");

            // Trouve l'objet correspondant dans la sc�ne
            Transform pointDeSpawn = GameObject.Find(pointDeSpawnNom).transform;

            // Positionne le joueur � cet emplacement
            transform.position = pointDeSpawn.position;

            PlayerPrefs.DeleteKey("PointDeSpawn");
        }
    }
}
