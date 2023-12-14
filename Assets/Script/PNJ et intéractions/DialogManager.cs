using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Le gestionnaire de dialogues pour contrôler l'affichage des textes de dialogue dans le jeu.
public class DialogManager : MonoBehaviour
{
    // Référence à l'Animator, utilisé pour animer la boîte de dialogue.
    public Animator Animator;

    // File d'attente pour stocker les phrases du dialogue.
    private Queue<string> sentences;

    // Références TextMeshPro pour afficher le nom et le texte du dialogue.
    public TMP_Text nameText;
    public TMP_Text dialogText;

    // Instance statique pour accéder au DialogManager depuis d'autres scripts.
    public static DialogManager Instance;

    // Awake est appelé lorsque le script est chargé. 
    // Ici, il s'assure qu'une seule instance de DialogManager est active.
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Plusieurs instances de DialogManager ont été trouvées dans la scène.");
            return;
        }
        Instance = this;

        // Initialisation de la file d'attente des phrases.
        sentences = new Queue<string>();
    }

    // Commence un nouveau dialogue et initialise l'affichage.
    public void StartDialog(Dialog dialog)
    {
        Debug.Log("Starting conversation with " + dialog.name);
        Animator.SetBool("isOpen", true);

        nameText.text = dialog.name;

        sentences.Clear();

        // Ajoute chaque phrase du dialogue à la file d'attente.
        foreach (string sentence in dialog.sentences)
        {
            sentences.Enqueue(sentence);
        }

    }

    // Affiche la phrase suivante du dialogue.
    public void DisplayNextSentence()
    {
        // Si plus de phrases, termine le dialogue.
        if (sentences.Count == 0)
        {
            EndDialog();
            return;
        }
        string sentence = sentences.Dequeue();
        dialogText.text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    // Coroutine pour afficher la phrase lettre par lettre.
    public IEnumerator TypeSentence(string sentence)
    {
        dialogText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }

    // Termine le dialogue et ferme la boîte de dialogue.
    public void EndDialog()
    {
        Animator.SetBool("isOpen", false);
    }
}
