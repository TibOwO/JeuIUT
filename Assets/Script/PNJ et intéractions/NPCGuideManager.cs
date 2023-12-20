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

    void Start()
    {
        // Trouver le script de mouvement du joueur et désactiver le mouvement
        playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.SetCanMove(false);
        }

        StartCinematic();
    }

    public void StartCinematic()
    {
        // Déclencher l'animation d'entrée du personnage
        characterAnimator.SetTrigger("EnterScreen");
        cinematicBars.ShowBars();

        // Ajouter un délai correspondant à la durée de l'animation "EnterScreen"
        // avant de démarrer le dialogue
        Invoke(nameof(StartDialogue), 1.5f); // Ajuster le temps pour correspondre à votre animation
    }

    void StartDialogue()
    {
        // Créer un objet Dialog avec le nom du personnage et les lignes de dialogue
        Dialog dialog = new Dialog
        {
            name = characterName,
            sentences = dialogueLines
        };

        // Démarrer le dialogue via le DialogManager
        dialogManager.StartDialog(dialog);
    }

    public void OnDialogueComplete()
    {
        // Ajouter un délai si vous souhaitez un peu de temps avant que le personnage ne sorte
        Invoke(nameof(EndCinematic), 1f); // Ajuster le temps selon les besoins
    }

    public void EndCinematic()
    {
        // Déclencher l'animation de sortie du personnage
        characterAnimator.SetTrigger("ExitScreen");
        cinematicBars.HideBars();


        // Réactiver le mouvement du joueur une fois la cinématique terminée
        if (playerMovement != null)
        {
            playerMovement.SetCanMove(true);
        }
    }

    // Appelée pour désactiver le sprite du personnage après l'animation de sortie
    void DisableCharacterSprite()
    {
        characterSprite.SetActive(false);
    }
}
