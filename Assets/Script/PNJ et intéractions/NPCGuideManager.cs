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

    void Start()
    {
        // Trouver le script de mouvement du joueur et d�sactiver le mouvement
        playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.SetCanMove(false);
        }

        StartCinematic();
    }

    public void StartCinematic()
    {
        // D�clencher l'animation d'entr�e du personnage
        characterAnimator.SetTrigger("EnterScreen");
        cinematicBars.ShowBars();

        // Ajouter un d�lai correspondant � la dur�e de l'animation "EnterScreen"
        // avant de d�marrer le dialogue
        Invoke(nameof(StartDialogue), 1.5f); // Ajuster le temps pour correspondre � votre animation
    }

    void StartDialogue()
    {
        // Cr�er un objet Dialog avec le nom du personnage et les lignes de dialogue
        Dialog dialog = new Dialog
        {
            name = characterName,
            sentences = dialogueLines
        };

        // D�marrer le dialogue via le DialogManager
        dialogManager.StartDialog(dialog);
    }

    public void OnDialogueComplete()
    {
        // Ajouter un d�lai si vous souhaitez un peu de temps avant que le personnage ne sorte
        Invoke(nameof(EndCinematic), 1f); // Ajuster le temps selon les besoins
    }

    public void EndCinematic()
    {
        // D�clencher l'animation de sortie du personnage
        characterAnimator.SetTrigger("ExitScreen");
        cinematicBars.HideBars();


        // R�activer le mouvement du joueur une fois la cin�matique termin�e
        if (playerMovement != null)
        {
            playerMovement.SetCanMove(true);
        }
    }

    // Appel�e pour d�sactiver le sprite du personnage apr�s l'animation de sortie
    void DisableCharacterSprite()
    {
        characterSprite.SetActive(false);
    }
}
