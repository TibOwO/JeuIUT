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
            // Si la cin�matique a d�j� �t� jou�e, r�activez imm�diatement le mouvement du joueur
            if (playerMovement != null)
            {
                playerMovement.SetCanMove(true);
            }
        }
    }

    public void StartCinematic()
    {
        // D�clenchez l'animation d'entr�e du personnage et affichez les barres cin�matiques
        characterAnimator.SetTrigger("EnterScreen");
        cinematicBars.ShowBars();

        // Commencez le dialogue apr�s un court d�lai
        Invoke(nameof(StartDialogue), 1.5f);
    }

    void StartDialogue()
    {
        // Cr�ez un objet Dialog avec le nom du personnage et les lignes de dialogue
        Dialog dialog = new Dialog
        {
            name = characterName,
            sentences = dialogueLines
        };

        // D�marrez le dialogue via le DialogManager
        dialogManager.StartDialog(dialog);
    }

    public void OnDialogueComplete()
    {
        // Terminez la cin�matique apr�s un court d�lai
        Invoke(nameof(EndCinematic), 1f);
    }

    public void EndCinematic()
    {
        // D�clenchez l'animation de sortie du personnage et cachez les barres cin�matiques
        characterAnimator.SetTrigger("ExitScreen");
        cinematicBars.HideBars();

        // R�activez le mouvement du joueur
        if (playerMovement != null)
        {
            playerMovement.SetCanMove(true);
        }
    }
}
