using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Cell : MonoBehaviour
{

	public string elementName;
	public int elementCount;
	public Color elementColor;
	public string elementDescription;

	public Transform elementTransform;
	private GameObject elementPrefab;


	public void Start()
	{
		if (elementName == "ElementPrefab")
		{
			DontDestroyOnLoad(gameObject);
		}
	}
	public void UpdateCellInterface()
	{
		if (elementPrefab == null)
		{
			elementPrefab = GameObject.Find("ElementPrefab");
		}
		if (elementPrefab == null)
		{
			Debug.Log("Ici");
		}
		if (elementCount == 0)
		{
			if (elementTransform != null)
			{
				Debug.Log("elementTransform n'est pas null ca devrait détruire l'objet =", elementPrefab);
				//Destroy(elementTransform.gameObject);
			}
			return;
		}
		else
		{
			if (elementTransform == null)
			{
				//spawn a new element prefab
				Transform newElement = Instantiate(elementPrefab).transform;
				newElement.parent = transform;
				newElement.localPosition = new Vector3();
				newElement.localScale = new Vector3(1f, 1f, 1f);
				elementTransform = newElement;
			}
			//init UI elements
			Image bgImage = SimpleMethods.getChildByTag(elementTransform, "backgroundImage").GetComponent<Image>();
			Text elementText = SimpleMethods.getChildByTag(elementTransform, "elementText").GetComponent<Text>();
			Text amountText = SimpleMethods.getChildByTag(elementTransform, "amountText").GetComponent<Text>();
			//change UI options
			bgImage.color = elementColor;
			elementText.text = elementName;
			amountText.text = elementCount.ToString();
		}
	}

	//Change element options
	public void ChangeElement(string name, int count, Color color, string description)
	{
		elementName = name;
		elementCount = count;
		elementColor = color;
		elementDescription = description;
		UpdateCellInterface();
	}

	//Clear element
	public void ClearElement()
	{
		elementName = "";
		elementCount = 0;
		elementColor = Color.clear; // Utilisez Color.clear pour une couleur "transparente"
		elementDescription = "";
		UpdateCellInterface();
	}

}
