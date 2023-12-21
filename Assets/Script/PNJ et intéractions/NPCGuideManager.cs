using UnityEngine;

public class NPCGuideManager : MonoBehaviour
{
    public Animator characterAnimator; // Référence au composant Animator du personnage
    public DialogManager dialogManager; // Référence au composant DialogManager existant
    public GameObject characterSprite; // Référence à l'objet GameObject du sprite du personnage
    public string characterName; // Le nom du personnage pour le dialogue
    public string[] dialogueLines; // Tableau de lignes de dialogue à afficher
    private PlayerMovement playerMovement; // Référence au script de mouvement du joueur
    public CinematicBars cinematicBars;

    // Variable statique pour garder une trace du nombre de fois que la cinématique a été jouée
    private static int cinematicPlayCount = 0;

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();

        // Vérifie si c'est la première fois que la cinématique est lancée dans cette session de jeu
        if (cinematicPlayCount == 0)
        {
            // Désactivez le mouvement du joueur et lancez la cinématique
            if (playerMovement != null)
            {
                playerMovement.SetCanMove(false);
            }

            StartCinematic();
            cinematicPlayCount++; // Incrémentez le compteur pour que la cinématique ne se joue plus automatiquement
        }
        else
        {
            // Si la cinématique a déjà été jouée, réactivez immédiatement le mouvement du joueur
            if (playerMovement != null)
            {
                playerMovement.SetCanMove(true);
            }
        }
    }

    public void StartCinematic()
    {
        // Déclenchez l'animation d'entrée du personnage et affichez les barres cinématiques
        characterAnimator.SetTrigger("EnterScreen");
        cinematicBars.ShowBars();

        // Commencez le dialogue après un court délai
        Invoke(nameof(StartDialogue), 1.5f);
    }

    void StartDialogue()
    {
        // Créez un objet Dialog avec le nom du personnage et les lignes de dialogue
        Dialog dialog = new Dialog
        {
            name = characterName,
            sentences = dialogueLines
        };

        // Démarrez le dialogue via le DialogManager
        dialogManager.StartDialog(dialog);
    }

    public void OnDialogueComplete()
    {
        // Terminez la cinématique après un court délai
        Invoke(nameof(EndCinematic), 1f);
    }

    public void EndCinematic()
    {
        // Déclenchez l'animation de sortie du personnage et cachez les barres cinématiques
        characterAnimator.SetTrigger("ExitScreen");
        cinematicBars.HideBars();

        // Réactivez le mouvement du joueur
        if (playerMovement != null)
        {
            playerMovement.SetCanMove(true);
        }
    }
}
