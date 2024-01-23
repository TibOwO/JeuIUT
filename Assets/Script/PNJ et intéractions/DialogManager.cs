using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public Animator Animator;
    private Coroutine typingCoroutine;
    private Queue<string> sentences;
    public TMP_Text nameText;
    public TMP_Text dialogText;
    private NPCGuideManager npcGuideManager; 
    public PlayerMovement playerMovement;

    public static DialogManager Instance;

    // Ajout d'un indicateur pour vérifier si une phrase est en train d'être écrite
    private bool isTyping = false;
    public bool IsDialogueActive { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Plusieurs instances de DialogManager ont été trouvées dans la scène.");
            return;
        }
        Instance = this;
        sentences = new Queue<string>();
        npcGuideManager = FindObjectOfType<NPCGuideManager>(); 

    }

    public void StartDialog(Dialog dialog)
    {
        IsDialogueActive = true;
        if (playerMovement != null)
        {
            playerMovement.SetCanMove(false); // Désactiver le mouvement
        }
        Animator.SetBool("isOpen", true);
        nameText.text = dialog.name;
        sentences.Clear();

        foreach (string sentence in dialog.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialog();
            return;
        }

        // Ne pas commencer à écrire une nouvelle phrase si la coroutine est déjà en cours.
        if (isTyping)
        {
            return;
        }

        string sentence = sentences.Dequeue();
        typingCoroutine = StartCoroutine(TypeSentence(sentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogText.text = "";
        isTyping = true; // Définir l'indicateur sur vrai puisque la frappe commence.

        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }

        isTyping = false; // Remettre l'indicateur à faux lorsque la frappe est terminée.
    }

    public void EndDialog()
    {
        IsDialogueActive = false;
        Animator.SetBool("isOpen", false);

        if (npcGuideManager != null)
        {
            npcGuideManager.OnDialogueComplete();
        }

        if (playerMovement != null)
        {
            playerMovement.SetCanMove(true); // Réactiver le mouvement
        }
    }

}
