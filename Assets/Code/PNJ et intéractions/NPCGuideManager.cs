using UnityEngine;

public class NPCGuideManager : MonoBehaviour
{
    public Animator characterAnimator; 
    public DialogManager dialogManager; 
    public GameObject characterSprite; 
    public string characterName; 
    public string[] dialogueLines; 
    private PlayerMovement playerMovement; 
    public CinematicBars cinematicBars;

   
    private static int cinematicPlayCount = 0;

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();

        // V�rifie si c'est la premi�re fois que la cin�matique est lanc�e dans cette session de jeu
        if (cinematicPlayCount == 0)
        {

            if (playerMovement != null)
            {
                playerMovement.SetCanMove(false);
            }

            StartCinematic();
            cinematicPlayCount++; 
        }
        else
        {
            if (playerMovement != null)
            {
                playerMovement.SetCanMove(true);
            }
        }
    }

    public void StartCinematic()
    {
        characterAnimator.SetTrigger("EnterScreen");
        cinematicBars.ShowBars();
        Invoke(nameof(StartDialogue), 1.5f);
    }

    void StartDialogue()
    {
        Dialog dialog = new Dialog
        {
            name = characterName,
            sentences = dialogueLines
        };

        dialogManager.StartDialog(dialog);
    }

    public void OnDialogueComplete()
    {
        Invoke(nameof(EndCinematic), 0.5f);
    }

    public void EndCinematic()
    {
        characterAnimator.SetTrigger("ExitScreen");
        cinematicBars.HideBars();

        if (playerMovement != null)
        {
            playerMovement.SetCanMove(true);
        }
    }
}
