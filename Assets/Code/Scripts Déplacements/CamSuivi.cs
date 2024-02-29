using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerTransform; // Référence au Transform du personnage
    public Vector3 offset; // Décalage pour la position de la caméra

    private void LateUpdate()
    {
        // Positionne la caméra à la position du joueur + décalage
        transform.position = new Vector3(playerTransform.position.x + offset.x, playerTransform.position.y + offset.y, transform.position.z);
    }
}
