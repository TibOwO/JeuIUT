using UnityEngine;

public class CleScript : MonoBehaviour
{
    private bool playerInRange = false;
    private bool keyPickedUp = false; // Nouvelle variable

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
        // Vérifie si le joueur est dans la zone, appuie sur la touche E et que la clé n'a pas été ramassée
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !keyPickedUp)
        {
            // Vérifie si l'inventaire contient un emplacement vide
            int emptyCellId = ElementalInventory.Instance.getFirst();

            if (emptyCellId != -1)
            {
                // Ajoute la clé à l'inventaire
                ElementalInventory.Instance.addItem("Cle", 1, Color.yellow);
                keyPickedUp = true; // La clé a été ramassée
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("L'inventaire est plein. Libérez de l'espace.");
            }
        }
    }
}
