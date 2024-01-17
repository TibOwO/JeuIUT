using UnityEngine;
using UnityEngine.SceneManagement;


public class DialogTrigger : MonoBehaviour
{
    public Dialog dialog;
    public bool isInRange = false;
    public bool isBoss = false;

    // Noms des objets à ramasser
    public string requiredItem1 = "Cle";
    public string requiredItem2 = "Objet2";
    public string requiredItem3 = "Objet3";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInRange)
        {
            // Vérifier si c'est un boss et si les objets nécessaires sont dans l'inventaire
            if (isBoss && CheckRequiredItems())
            {
                // Déclencher la scène de combat ici
                StartBossFight();
            }
            else
            {
                TriggerDialog();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = true;
            Debug.Log("Le joueur est dans la zone de dialogue");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TriggerExit();
        }
    }

    public void TriggerExit()
    {
        isInRange = false;
        Debug.Log("Le joueur a quitté la zone de dialogue");
    }

    public void TriggerDialog()
    {
        // Ajout de la condition pour vérifier si l'objet est actif (visible)
        if (gameObject.activeSelf)
        {
            FindObjectOfType<DialogManager>().StartDialog(dialog);
        }
    }

    // Fonction pour vérifier si les objets nécessaires sont dans l'inventaire
    private bool CheckRequiredItems()
    {
        // Utiliser les noms configurables depuis l'Inspector
        bool hasItem1 = ElementalInventory.Instance.contains(requiredItem1, 1);
        bool hasItem2 = ElementalInventory.Instance.contains(requiredItem2, 1);
        bool hasItem3 = ElementalInventory.Instance.contains(requiredItem3, 1);

        // Retourner vrai si tous les objets sont présents
        return hasItem1 && hasItem2 && hasItem3;
    }

    // Fonction pour déclencher la scène de combat avec le boss
    private void StartBossFight()
    {
        SceneManager.LoadScene(gameObject.name);
        Debug.Log("Lancement de la scène de combat avec le boss!");
    }
}
