using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerTransform; // R�f�rence au Transform du personnage
    public Vector3 offset; // D�calage pour la position de la cam�ra

    private void LateUpdate()
    {
        // Positionne la cam�ra � la position du joueur + d�calage
        transform.position = new Vector3(playerTransform.position.x + offset.x, playerTransform.position.y + offset.y, transform.position.z);
    }
}
