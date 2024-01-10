using UnityEngine;

public class CleScript : MonoBehaviour
{
    private bool playerInRange = false;
    private bool keyPickedUp = false; // Nouvelle variable

    private Renderer myRenderer; // Ajout d'une référence au composant Renderer
    private DialogTrigger dialogTrigger; // Ajout d'une référence au script de déclenchement de dialogue

    private void Start()
    {
        // Obtient le composant Renderer attaché à cet objet
        myRenderer = GetComponent<Renderer>();

        // Obtient le composant DialogTrigger attaché à cet objet
        dialogTrigger = GetComponent<DialogTrigger>();
    }

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
            TriggerExit();
        }
    }

    private void Update()
    {
        // Vérifie si le joueur est dans la zone, appuie sur la touche E, que la clé n'a pas été ramassée et que la clé est visible
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !keyPickedUp && myRenderer.enabled)
        {
            // Vérifie si l'inventaire contient un emplacement vide
            int emptyCellId = ElementalInventory.Instance.getFirst();

            if (emptyCellId != -1)
            {
                // Ajoute la clé à l'inventaire
                ElementalInventory.Instance.addItem("Cle", 1, Color.yellow);
                keyPickedUp = true; // La clé a été ramassée

                // Rend l'objet invisible
                if (myRenderer != null)
                {
                    myRenderer.enabled = false;
                }
                else
                {
                    Debug.LogError("Le composant Renderer est introuvable sur cet objet.");
                }

                // Désactive également le script de déclenchement de dialogue
                if (dialogTrigger != null)
                {
                    dialogTrigger.enabled = false;
                }
            }
            else
            {
                Debug.Log("L'inventaire est plein. Libérez de l'espace.");
            }
        }
    }

    private void TriggerExit()
    {
        playerInRange = false;
        Debug.Log("Le joueur a quitté la zone de la clé");
    }
}
