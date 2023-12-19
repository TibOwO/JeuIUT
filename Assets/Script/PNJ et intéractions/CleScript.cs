using UnityEngine;

public class CleScript : MonoBehaviour
{
    private bool playerInRange = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        // Vérifie si le joueur est dans la zone et appuie sur la touche E
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            CleManager.Instance.HasKey = true;
            Destroy(gameObject); // Détruit l'objet clé

        }
    }
}
