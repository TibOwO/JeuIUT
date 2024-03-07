using UnityEngine;

public class CleScript : MonoBehaviour, IInteractable
{
    private bool playerInRange = false;
    private bool itemPickedUp = false;
    private Renderer myRenderer;

    //vérifie si l'objet a déjà été ramassé par le joueur
    //si c'est le cas, l'objet est détruit
    private void Start()
    {
        myRenderer = GetComponent<Renderer>();
        string itemName = gameObject.name;

        if (CleManager.Instance.WasItemPickedUp(itemName))
        {
            Destroy(gameObject);
        }
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
        }
    }

    //fonction appelée lorsqu'un objet est ramassé par le joueur
    //elle ajoute l'objet à l'inventaire et le détruit de la scène
    public void Interact()
    {
        int emptyCellId = ElementalInventory.Instance.getFirst();

        if (emptyCellId != -1)
        {
            Cell cell = GetComponent<Cell>();
            if (cell != null)
            {
                Color randomColor = new Color(Random.value, Random.value, Random.value);
                ElementalInventory.Instance.addItem(cell.elementName, cell.elementCount, randomColor, cell.elementDescription);
                itemPickedUp = true;
                CleManager.Instance.MarkItemAsPickedUp(gameObject.name);


                Destroy(gameObject);

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
