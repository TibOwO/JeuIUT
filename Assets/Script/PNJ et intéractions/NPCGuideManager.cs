using UnityEngine;

public class NPCGuideManager : MonoBehaviour
{
    public Animator characterAnimator; // R�f�rence au composant Animator du personnage
    public DialogManager dialogManager; // R�f�rence au composant DialogManager existant
    public GameObject characterSprite; // R�f�rence � l'objet GameObject du sprite du personnage
    public string characterName; // Le nom du personnage pour le dialogue
    public string[] dialogueLines; // Tableau de lignes de dialogue � afficher
    private PlayerMovement playerMovement; // R�f�rence au script de mouvement du joueur
    public CinematicBars cinematicBars;

    // Variable statique pour garder une trace du nombre de fois que la cin�matique a �t� jou�e
    private static int cinematicPlayCount = 0;

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();

        // V�rifie si c'est la premi�re fois que la cin�matique est lanc�e dans cette session de jeu
        if (cinematicPlayCount == 0)
        {
            // D�sactivez le mouvement du joueur et lancez la cin�matique
            if (playerMovement != null)
            {
                playerMovement.SetCanMove(false);
            }

            StartCinematic();
            cinematicPlayCount++; // Incr�mentez le compteur pour que la cin�matique ne se joue plus automatiquement
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
        Invoke(nameof(EndCinematic), 1f);
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
