using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;


public class DialogTrigger : MonoBehaviour
{
    public Dialog dialog;
    public TextMeshProUGUI interactMessage;
    public bool isInRange = false;
    public bool isBoss = false;
    public bool isDoor = false;

    // Liste des noms des objets à ramasser (dynamique)
    public List<string> requiredItems = new List<string>();


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInRange && !DialogManager.Instance.IsDialogueActive)
        {
            // Vérifier si c'est un boss et si les objets nécessaires sont dans l'inventaire
            if (isBoss && CheckRequiredItems())
            {
                StartBossFight();
            }
            else if (isDoor && !CheckRequiredItems())
            {
                TriggerDialog();
            }
            else
            {
                TriggerDialog();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !DialogManager.Instance.IsDialogueActive)
        {
            isInRange = true;
            if (isDoor && CheckRequiredItems())
            {
                interactMessage.gameObject.SetActive(false);
                Debug.Log("Le joueur est dans la zone de dialogue");

            }
            else
            {
                interactMessage.gameObject.SetActive(true);
                Debug.Log("Le joueur est dans la zone de dialogue");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TriggerExit();
            if (interactMessage != null)
            {
                interactMessage.gameObject.SetActive(false);
            }

        }
    }


    public void TriggerExit()
    {
        isInRange = false;
        Debug.Log("Le joueur a quitté la zone de dialogue");
    }

    public void TriggerDialog()
    {
        if (gameObject.activeSelf && !DialogManager.Instance.IsDialogueActive)
        {
            interactMessage.gameObject.SetActive(false);

            DialogManager.Instance.StartDialog(dialog);
        }
    }

    private bool CheckRequiredItems()
    {
        // Utiliser la liste de noms d'objets configurables depuis l'Inspector
        foreach (string itemName in requiredItems)
        {
            bool hasItem = ElementalInventory.Instance.contains(itemName, 1);
            if (!hasItem)
            {
                return false;
            }
        }

        return true;
    }

    // Fonction pour déclencher la scène de combat avec le boss
    private void StartBossFight()
    {
        SceneManager.LoadScene(gameObject.name);
        Debug.Log("Lancement de la scène de combat avec le boss!");
    }
}

