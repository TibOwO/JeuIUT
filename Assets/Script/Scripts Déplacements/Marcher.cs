using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    public bool canMove = true; // Flag pour contr�ler le mouvement du personnage

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

        // R�cup�ration des entr�es pour les d�placements
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Pour emp�cher le mouvement diagonal
        if (horizontal != 0) vertical = 0;

        // Mise � jour du vecteur de mouvement
        movement = new Vector2(horizontal, vertical);
        UpdateAnimator(movement);
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            // D�placement du personnage
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void UpdateAnimator(Vector2 movement)
    {
        // Mise � jour des param�tres de l'Animator
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
    }

    // M�thode publique pour activer/d�sactiver le mouvement
    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }
}
