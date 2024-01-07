using UnityEngine;

public class InventoryRenderer : MonoBehaviour
{
    public void ToggleInventoryUI(bool isOpen)
    {
        // Vérifier si l'objet enfant nommé "Branch" existe
        Transform branch = transform.Find("Branch");

        if (branch != null)
        {
            // Activer ou désactiver l'objet enfant "Branch"
            branch.gameObject.SetActive(isOpen);
        }
        else
        {
            Debug.LogError("L'objet enfant 'Branch' n'a pas été trouvé.");
        }
    }

}
