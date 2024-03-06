using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MovingEvent : MonoBehaviour
{
	private ElementalInventory inventory;
	public TMP_Text descriptionTextMesh;


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
			int cellIndex = indexHolder.Index - 1;
			if (cellIndex >= 0 && cellIndex < inventory.Cells.Length)
			{
				Cell cell = inventory.Cells[cellIndex];
				Debug.Log(cell);
				if (cell != null)
				{
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
		Cell cell = transform.GetComponent<Cell>();
		descriptionTextMesh.text = cell.elementDescription;

	}
}
