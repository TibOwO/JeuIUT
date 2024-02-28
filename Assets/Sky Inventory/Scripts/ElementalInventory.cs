using UnityEngine;
using System.Collections;

public class ElementalInventory : MonoBehaviour
{

	//Cell massive
	public Cell[] Cells = new Cell[31];
	//Max element stack
	public int maxStack;
	public GameObject elementPrefab;
	private Transform choosenItem;

	private InventoryRenderer inventoryRenderer;
	public static ElementalInventory Instance { get; private set; }



	private void Start()
	{
		elementPrefab = GameObject.Find("ElementPrefab");

		if (elementPrefab == null)
		{
			Debug.Log("ELEMENTPREFAB IS NULL");
		}
		inventoryRenderer = GetComponentInChildren<InventoryRenderer>();

		if (inventoryRenderer == null)
		{
			Debug.LogError("InventoryRenderer component not found on ElementalInventory.");
		}
	}

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject); // Empêche la destruction lors d'un changement de scène
		}
		else
		{
			// S'il existe déjà une instance dans une autre scène, détruire cet objet
			if (Instance != this)
			{
				Destroy(gameObject);
				return;
			}
		}
	}
	public bool contains(string name, int count)
	{
		int inventoryCount = 0;
		for (int i = 0; i < Cells.Length; i++)
		{
			if (Cells[i].elementCount != 0 && Cells[i].elementName == name)
			{
				inventoryCount += Cells[i].elementCount;
			}
		}
		if (count <= inventoryCount)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	//Set item from link
	public void setItemLink(string name, int count, Color color, string description, Transform cell)
	{
		Cell thisCell = cell.GetComponent<Cell>();
		thisCell.elementName = name;
		thisCell.elementCount = count;
		thisCell.elementColor = color;
		thisCell.elementDescription = description;
	}

	//Moves item
	public void moveItem(int moveFrom, int moveTo)
	{
		setItem(Cells[moveFrom].elementName, Cells[moveFrom].elementCount, Cells[moveFrom].elementColor, moveTo, Cells[moveFrom].elementDescription);
		setItem("", 0, new Color(), moveFrom, "");
	}


	public void moveItemLink(Transform moveFrom, Transform moveTo)
	{
		if (moveFrom != null && moveTo != null)
		{
			Cell moveFromCell = moveFrom.parent.GetComponent<Cell>();
			moveTo.GetComponent<Cell>().elementTransform = moveFromCell.elementTransform;
			moveFromCell.elementTransform = null;

			// Mise à jour de la cellule de destination avec les informations de l'élément
			setItemLink(moveFromCell.elementName, moveFromCell.elementCount, moveFromCell.elementColor, moveFromCell.elementDescription, moveTo);

			// Réinitialisation de la cellule d'origine
			moveFromCell.ClearElement(); // Utilisez la méthode ClearElement pour réinitialiser la cellule

			moveFrom.parent = moveTo;
			moveFrom.localPosition = new Vector3();
		}
	}



	public void moveItemLinkFirst(Transform t)
	{
		choosenItem = t;
	}

	public void moveItemLinkSecond(Transform t)
	{
		moveItemLink(choosenItem, t);
		choosenItem = null;
	}

	//Sets item
	public void setItem(string name, int count, Color color, int cellId, string description)
	{
		Cells[cellId].ChangeElement(name, count, color, description);
		Cells[cellId].UpdateCellInterface();
	}

	//Loads inventory from string
	public void loadFromString(string s_Inventory)
	{
		string[] splitedInventory = s_Inventory.Split("\n"[0]);
		for (int i = 0; i < Cells.Length; i++)
		{
			string[] splitedLine = splitedInventory[i].Split(" "[0]);
			setItem(splitedLine[0], int.Parse(splitedLine[1]), SimpleMethods.stringToColor(splitedLine[2]), i, "");
		}
	}

	//Returns inventory as string
	public string convertToString()
	{
		string s_Inventory = "";
		for (int i = 0; i < Cells.Length - 1; i++)
		{
			s_Inventory += Cells[i].elementName + " ";
			s_Inventory += Cells[i].elementCount + " ";
			s_Inventory += SimpleMethods.colorToString(Cells[i].elementColor);
			if (i != Cells.Length - 1)
			{
				s_Inventory += "\n";
			}
		}
		return s_Inventory;
	}

	//Clear inventory
	public void clear()
	{
		for (int i = 0; i < Cells.Length; i++)
		{
			if (Cells[i].elementCount != 0)
			{
				Cells[i].elementCount = 0;
				Cells[i].UpdateCellInterface();
			}
		}
	}

	//Add element to inventory
	// Add element to inventory
	public void addItem(string name, int count, Color color, string description)
	{
		Debug.Log("Adding item: " + name + " - Count: " + count + " - Color: " + color.ToString() + " - Description: " + description);
		int cellId = getEquals(name);
		if (cellId != -1)
		{
			Cells[cellId].elementCount += count;
		}
		else
		{
			cellId = getFirst();
			if (cellId == -1)
			{
				return;
			}
			Cells[cellId].elementCount += count;
		}

		// Set up element count
		if (Cells[cellId].elementCount > maxStack)
		{
			int remain = Cells[cellId].elementCount - maxStack;
			Cells[cellId].elementCount = maxStack;
			addItem(name, remain, color, description); // Ajouter la description pour l'élément restant
		}

		Cells[cellId].elementName = name;
		Cells[cellId].elementColor = color;
		Cells[cellId].elementDescription = description; // Attribuer la description à l'élément
		Cells[cellId].UpdateCellInterface();


	}



	//Returns id of first clear cell
	public int getFirst()
	{
		for (int i = 0; i < Cells.Length; i++)
		{
			if (Cells[i].elementCount == 0)
			{

				return i;
			}
		}
		return -1;
	}

	//Returns id of first same element cell
	public int getEquals(string name)
	{
		for (int i = 0; i < Cells.Length; i++)
		{
			Debug.Log("Element name: " + Cells[i].elementName + " - " + name);
			if (Cells[i].elementName == name)
			{
				return i;
			}
		}
		return -1;
	}

	public void ToggleInventoryRenderer(bool isEnabled)
	{

		inventoryRenderer = GetComponentInChildren<InventoryRenderer>();
		if (inventoryRenderer != null)
		{
			inventoryRenderer.enabled = isEnabled;
		}
		else
		{
			Debug.LogError("InventoryRenderer component not found on ElementalInventory.");
		}
	}

}
