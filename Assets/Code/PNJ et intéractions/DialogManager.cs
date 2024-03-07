using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogManager : MonoBehaviour
{
    public Animator Animator;
    private Coroutine typingCoroutine;
    private Queue<string> sentences;
    public TMP_Text nameText;
    public TMP_Text dialogText;
    private NPCGuideManager npcGuideManager;
    public PlayerMovement playerMovement;
    public Image dialogueImage;
    public int imageDisplaySentenceIndex = 3;
    private int currentSentenceIndex = 0;
    public string requiredSceneForImage = "Couloir";

    private HashSet<string> playedDialogues = new HashSet<string>(); // Pour suivre les dialogues déjà joués

    public static DialogManager Instance;
    public bool IsDialogueActive { get; private set; }
    private bool isTyping = false;

    private void Awake()
    {
        Instance = this;
        sentences = new Queue<string>();
        npcGuideManager = FindObjectOfType<NPCGuideManager>();
        if (dialogueImage != null)
        {
            dialogueImage.gameObject.SetActive(false);
        }
    }

    //commente ma fonction StartDialog
    //est appelée par le script DialogTrigger
    //permet de lancer un dialogue
    //prend en paramètre un objet de type Dialog

    public void StartDialog(Dialog dialog)
    {
        IsDialogueActive = true;
        currentSentenceIndex = 0;

        // met en pause le mouvement du joueur
        if (playerMovement != null)
        {
            playerMovement.SetCanMove(false);
        }
        Animator.SetBool("isOpen", true);
        nameText.text = dialog.name;
        sentences.Clear();

        foreach (string sentence in dialog.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();

        // Marquer le dialogue comme joué
        playedDialogues.Add(dialog.name);
    }

    //est appelée par le script DialogTrigger
    //permet d'afficher la phrase suivante
    //prend en paramètre un objet de type Dialog
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialog();
            return;
        }

        if (isTyping)
        {
            return;
        }

        string sentence = sentences.Dequeue();
        if (SceneManager.GetActiveScene().name == requiredSceneForImage &&
            nameText.text == "Mysterieux Guide" &&
            currentSentenceIndex == imageDisplaySentenceIndex)
        {
            // S'assurer que l'objet Image n'est pas nul avant de l'activer
            if (dialogueImage != null)
            {
                dialogueImage.gameObject.SetActive(true); // Afficher l'image
            }
        }
        else
        {
            if (dialogueImage != null)
            {
                dialogueImage.gameObject.SetActive(false); // Cacher l'image
            }
        }
        currentSentenceIndex++;
        typingCoroutine = StartCoroutine(TypeSentence(sentence));
    }

    //Gere l'affichage lettre par lettre des phrases
    private IEnumerator TypeSentence(string sentence)
    {
        dialogText.text = "";
        isTyping = true;

        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }

        isTyping = false;
    }

    public void EndDialog()
    {
        IsDialogueActive = false;
        Animator.SetBool("isOpen", false);
        if (SceneManager.GetActiveScene().name == requiredSceneForImage)
        {
            dialogueImage.gameObject.SetActive(false);
        }// Cacher l'image à la fin du dialogue

        if (npcGuideManager != null)
        {
            npcGuideManager.OnDialogueComplete();
        }

        if (playerMovement != null)
        {
            playerMovement.SetCanMove(true);
        }
    }
}
