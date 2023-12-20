using UnityEngine;

public class NPCGuideManager : MonoBehaviour
{
    public Animator characterAnimator;
    public DialogManager dialogManager;
    public GameObject characterSprite;
    public string characterName;
    public string[] dialogueLines;

    private bool enterAnimationComplete = false; // Flag pour suivre la fin de l'animation d'entrée

    void Start()
    {
        StartCinematic();
    }

    public void StartCinematic()
    {
        characterAnimator.SetTrigger("EnterScreen");
        // Planifier une vérification pour la fin de l'animation d'entrée.
        Invoke(nameof(CheckEnterAnimationComplete), 0.1f); // Ajustez ce délai selon la longueur de votre animation
    }

    void CheckEnterAnimationComplete()
    {
        enterAnimationComplete = true;
        StartDialogue();
    }

    void StartDialogue()
    {
        if (enterAnimationComplete)
        {
            Dialog dialog = new Dialog
            {
                name = characterName,
                sentences = dialogueLines
            };
            dialogManager.StartDialog(dialog);
        }
    }

    public void OnDialogueComplete()
    {
        if (enterAnimationComplete)
        {
            Invoke(nameof(EndCinematic), 0.2f);
        }
    }

    public void EndCinematic()
    {
        characterAnimator.SetTrigger("ExitScreen");
    }

    void DisableCharacterSprite()
    {
        characterSprite.SetActive(false);
    }
}
