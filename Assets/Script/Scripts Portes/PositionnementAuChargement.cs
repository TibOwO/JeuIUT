using UnityEngine;

public class PositionnementAuChargement : MonoBehaviour
{
    void Start()
    {
        // Vérifie si le PointDeSpawn existe dans les PlayerPrefs
        if (PlayerPrefs.HasKey("PointDeSpawn"))
        {
            // Récupère le nom du point de spawn
            string pointDeSpawnNom = PlayerPrefs.GetString("PointDeSpawn");

            // Trouve l'objet correspondant dans la scène
            Transform pointDeSpawn = GameObject.Find(pointDeSpawnNom).transform;

            // Positionne le joueur à cet emplacement
            transform.position = pointDeSpawn.position;

            PlayerPrefs.DeleteKey("PointDeSpawn");
        }
    }
}
