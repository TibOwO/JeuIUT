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

	public void moveItem()
	{
		if (inventory == null)
		{
			inventory = FindObjectOfType(typeof(ElementalInventory)) as ElementalInventory;
		}
		inventory.moveItemLinkFirst(transform);

		// Afficher la description de l'objet dans le TextMesh

		Cell cell = transform.GetComponent<Cell>();
		int i = ElementalInventory.Instance.getEquals(cell.elementName);
		Debug.Log(i + " - " + this + " - " + cell.elementName + " " + cell.elementColor + " " + cell.elementDescription + " " + inventory.Cells[i].elementDescription);
		descriptionTextMesh.text = inventory.Cells[i].elementDescription;

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
