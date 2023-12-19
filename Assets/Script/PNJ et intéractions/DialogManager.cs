using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



// Le gestionnaire de dialogues pour contr�ler l'affichage des textes de dialogue dans le jeu.
public class DialogManager : MonoBehaviour
{
    // R�f�rence � l'Animator, utilis� pour animer la bo�te de dialogue.
    public Animator Animator;


    // Coroutine pour afficher la phrase lettre par lettre.
    private Coroutine typingCoroutine;


    // File d'attente pour stocker les phrases du dialogue.
    private Queue<string> sentences;


    // R�f�rences TextMeshPro pour afficher le nom et le texte du dialogue.
    public TMP_Text nameText;
    public TMP_Text dialogText;

    // Instance statique pour acc�der au DialogManager depuis d'autres scripts.
    public static DialogManager Instance;

    // Awake est appel� lorsque le script est charg�. 
    // Ici, il s'assure qu'une seule instance de DialogManager est active.
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Plusieurs instances de DialogManager ont �t� trouv�es dans la sc�ne.");
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

        // Vérifie si une coroutine est en cours et l'arrête.
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // Affiche la première phrase du dialogue.
        DisplayNextSentence();
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

        // Affiche la phrase actuelle.
        string sentence = sentences.Dequeue();
        typingCoroutine = StartCoroutine(TypeSentence(sentence));
    }

    // Coroutine pour afficher la phrase lettre par lettre.
    private IEnumerator TypeSentence(string sentence)
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