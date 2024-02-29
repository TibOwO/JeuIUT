using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public AudioSource footstepAudioSource; 
    public AudioClip[] footstepSounds; 
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    public bool canMove = true; // Flag pour contrôler le mouvement du personnage
    private bool isMoving = false; //Flag Pour vérifier si le personnage se déplace

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!canMove)
        {
            movement = Vector2.zero;
            UpdateAnimator(movement);
            return;
        }

        // Récupération des entrées pour les déplacements
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Pour empêcher le mouvement diagonal
        if (horizontal != 0) vertical = 0;

        // Mise à jour du vecteur de mouvement
        movement = new Vector2(horizontal, vertical);
        isMoving = movement != Vector2.zero;
        UpdateAnimator(movement);

        // Jouer le son de pas si le personnage se déplace
        if (isMoving && !footstepAudioSource.isPlaying)
        {
            PlayFootstepSound();
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            // Déplacement du personnage
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void UpdateAnimator(Vector2 movement)
    {
        // Mise à jour des paramètres de l'Animator
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
    }

    private void PlayFootstepSound()
    {
        // Choisir un son de pas aléatoire
        int index = Random.Range(0, footstepSounds.Length);
        footstepAudioSource.clip = footstepSounds[index];
        footstepAudioSource.Play();
    }

    // Méthode publique pour activer/désactiver le mouvement
    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }
}
