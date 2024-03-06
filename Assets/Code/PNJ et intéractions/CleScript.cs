using UnityEngine;

public class CleScript : MonoBehaviour, IInteractable
{
    private bool playerInRange = false;
    private bool itemPickedUp = false;
    private Renderer myRenderer;

    private void Start()
    {
        myRenderer = GetComponent<Renderer>();
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
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !itemPickedUp)
        {
            Interact();
            GameObject targetobject = GameObject.Find("Porte_salle1");
            ArrowOrbit.ChangeTarget(targetobject.transform);
        }
    }

    public void Interact()
    {
        int emptyCellId = ElementalInventory.Instance.getFirst();

        if (emptyCellId != -1)
        {
            // Get the Cell component from this GameObject
            Cell cell = GetComponent<Cell>();
            if (cell != null)
            {
                Color randomColor = new Color(Random.value, Random.value, Random.value);
                // Use the values from the Cell component when adding the item
                ElementalInventory.Instance.addItem(cell.elementName, cell.elementCount, randomColor, cell.elementDescription);
                itemPickedUp = true;

                if (myRenderer != null)
                {
                    myRenderer.enabled = false;
                }
                else
                {
                    Debug.LogError("Le composant Renderer est introuvable sur cet objet.");
                }
            }
            else
            {
                Debug.LogError("Le composant Cell est introuvable sur cet objet.");
            }
        }
        else
        {
            Debug.Log("L'inventaire est plein. Libérez de l'espace.");
        }
    }


}
