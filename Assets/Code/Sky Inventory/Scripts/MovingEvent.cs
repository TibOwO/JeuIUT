using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MovingEvent : MonoBehaviour
{
	private ElementalInventory inventory;
	public TMP_Text descriptionTextMesh; // Référence au TextMesh pour afficher la description
	private bool isInventoryOpen = false;


	void Start()
	{

		if (transform.tag == "Cell")
		{
			GetComponent<Button>().onClick.AddListener(delegate { moveHere(); });
		}
		else
		{
			GetComponent<Button>().onClick.AddListener(delegate { moveItem(); });
		}
	}
	private void Awake()
	{
		DontDestroyOnLoad(gameObject); // Empêche la destruction lors d'un changement de scène
	}



	public void moveItem()
	{
		if (inventory == null)
		{
			inventory = FindObjectOfType(typeof(ElementalInventory)) as ElementalInventory;
		}
		inventory.moveItemLinkFirst(transform);

		// Afficher la description de l'objet dans le TextMesh

		CellIndexHolder indexHolder = transform.parent.GetComponent<CellIndexHolder>();
		Debug.Log(indexHolder.Index);
		if (indexHolder != null)
		{
			int cellIndex = indexHolder.Index - 1; // L'index est stocké dans chaque bouton.
			if (cellIndex >= 0 && cellIndex < inventory.Cells.Length)
			{
				// Maintenant que vous avez l'index, utilisez-le pour accéder à la cellule de l'inventaire.
				Cell cell = inventory.Cells[cellIndex];
				Debug.Log(cell);
				if (cell != null)
				{
					// Mettez à jour la description.
					Debug.Log(cell.elementDescription);
					descriptionTextMesh.text = cell.elementDescription;
				}
			}
		}
		else
		{
			Debug.LogError("CellIndexHolder component is missing on this button's GameObject!");
		}
	}

	public void moveHere()
	{
		if (inventory == null)
		{
			inventory = FindObjectOfType(typeof(ElementalInventory)) as ElementalInventory;
		}
		inventory.moveItemLinkSecond(transform);

		// Afficher la description de l'objet dans le TextMesh
		Cell cell = transform.GetComponent<Cell>();
		descriptionTextMesh.text = cell.elementDescription;

	}
}
