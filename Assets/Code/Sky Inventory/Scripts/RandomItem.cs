using UnityEngine;
using System.Collections;

public class RandomItem : MonoBehaviour
{
	private ElementalInventory inventory;
	private bool isInventoryOpen = false;


	void Start()
	{
		if (inventory == null)
		{
			inventory = FindObjectOfType<ElementalInventory>();
			if (inventory == null)
			{
				Debug.LogError("ElementalInventory non trouvé dans la scène.");
				return;
			}
		}
		inventory.ToggleInventoryRenderer(isInventoryOpen);
	}
	void Update()
	{
		if (inventory == null)
		{
			inventory = FindObjectOfType<ElementalInventory>();
			if (inventory == null)
			{
				Debug.LogError("ElementalInventory non trouvé dans la scène.");
				return;
			}
		}

		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (inventory != null)
			{
				isInventoryOpen = !isInventoryOpen;

				if (inventory.gameObject != null)
				{
					inventory.ToggleInventoryRenderer(isInventoryOpen);
				}
				else
				{
					Debug.LogError("L'objet GameObject de l'inventaire est null.");
				}
				for (int i = 0; i < inventory.Cells.Length; i++)
				{
					if (inventory.Cells[i].elementCount > 0)
					{
						Debug.Log($"Cellule {i}: {inventory.Cells[i].elementName}, Count: {inventory.Cells[i].elementCount}, Color: {inventory.Cells[i].elementColor} - Description: {inventory.Cells[i].elementDescription}");
					}
				}

				InventoryRenderer inventoryRenderer = inventory.GetComponentInChildren<InventoryRenderer>();
				if (inventoryRenderer != null)
				{
					inventoryRenderer.ToggleInventoryUI(isInventoryOpen);
				}
				else
				{
					Debug.LogError("Le composant InventoryRenderer n'est pas attaché à ElementalInventory.");
				}
			}
		}
	}
}

