using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Récupération des entrées pour les déplacements
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Pour empêcher le mouvement diagonal
        if (horizontal != 0) vertical = 0;

        // Mise à jour du vecteur de mouvement
        movement = new Vector2(horizontal, vertical);

        // Mise à jour des paramètres de l'Animator
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
    }

    void FixedUpdate()
    {
        // Déplacement du personnage
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
