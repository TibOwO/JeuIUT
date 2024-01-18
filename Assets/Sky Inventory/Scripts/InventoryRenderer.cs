using UnityEngine;

public class InventoryRenderer : MonoBehaviour
{
    public void ToggleInventoryUI(bool isOpen)
    {
        Transform branch = transform.Find("Branch");

        if (branch != null)
        {
            branch.gameObject.SetActive(isOpen);
        }
        else
        {
            Debug.LogError("L'objet enfant 'Branch' n'a pas été trouvé.");
        }
    }

}
