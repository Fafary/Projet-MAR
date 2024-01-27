using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator playerAnim;
    public Rigidbody playerRigid;
    public Transform playerTrans;

    public float z_speed; // Vitesse de marche
    public float zb_speed; // Vitesse de recul
    public float olw_speed; // Vitesse de l'ancienne vitesse
    public float rn_speed; // Vitesse de course
    public float ro_speed; // Vitesse de rotation

    public bool walking; // Marche
    public bool isRotating; // Tourne ou pas
    private bool move = false; // avance

    public float jumpForce = 5.0f; // Force de saut
    public float distanceToGround = 50f; // Hauteur du sol
    RaycastHit hit;
    public LayerMask layerMask;

    public int playerHealth = 10; // Vie du joueur

    public float attackRange = 5.0f; // Distance d'attaque
    public int attackDamage = 2; // Force d'attaque

    void FixedUpdate()
    {
        // Déplacement avant et arriere en fonction des touches enfoncées
        MoveForwardBackward();
    }

    void Update()
    {
        // Gestion de la course
        HandleRunning();

        // Gestion du mouvement et des animations
        HandleMovementAndAnimations();

        // Rotation du personnage
        RotatePlayer();

        // Gestion de saut
        HandleJump();

        // Gestion de l'attaque
        AttackPlayer();
    }

    void MoveForwardBackward()
    {
        if (IsGrounded())
        {
            if (Input.GetKey(KeyCode.W))
            {
                playerRigid.velocity = transform.forward * z_speed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                playerRigid.velocity = -transform.forward * zb_speed * Time.deltaTime;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            float horizontalInput = Input.GetAxis("Horizontal");

            if (Mathf.Approximately(horizontalInput, 0f))
            {
                // Aucune touche de déplacement enfoncée, saut vertical
                playerRigid.velocity = new Vector3(0, jumpForce, 0);
            }
            else
            {
                // Ajuste la vélocité horizontale pendant le saut en fonction de l'entrée du joueur
                Vector3 horizontalVelocity = playerTrans.right * horizontalInput * (walking ? z_speed : rn_speed) * Time.deltaTime;
                playerRigid.velocity += horizontalVelocity;
            }
        }
    }

    void HandleMovementAndAnimations()
    {
        // Avancer en appuyant sur la touche ou s pour reculer
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            walking = true;
            move=true;
            playerAnim.SetTrigger("WalkForward");
            playerAnim.ResetTrigger("Idle");
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            walking = false;
            move=false;
            playerAnim.ResetTrigger("WalkForward");
            playerAnim.SetTrigger("Idle");
        }

        // Gestion de l'animation de rotation
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            playerAnim.SetTrigger("WalkForward");
            playerAnim.ResetTrigger("Idle");
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            if (!walking)
            {
                isRotating = false;
                move=false;
                playerAnim.SetTrigger("Idle");
                playerAnim.ResetTrigger("WalkForward");
            }
        }
    }

    void RotatePlayer()
    {
        // Tourne à gauche
        if (Input.GetKey(KeyCode.A))
        {
            playerTrans.Rotate(0, -ro_speed * Time.deltaTime, 0);
            isRotating = true;
        }
        // Tourne à droite
        else if (Input.GetKey(KeyCode.D))
        {
            playerTrans.Rotate(0, ro_speed * Time.deltaTime, 0);
            isRotating = true;
        }
        // Aucune touche enfoncée, arrête l'animation
        else if (isRotating && !walking)
        {
            playerAnim.SetTrigger("Idle");
            playerAnim.ResetTrigger("WalkForward");
            isRotating = false;
            move=false;
        }
    }

    void HandleRunning()
    {
        // Courir
        if (walking)
        {
            if (Input.GetKey(KeyCode.LeftShift) || z_speed == rn_speed)
            {
                z_speed = rn_speed;
                playerAnim.SetTrigger("RunForward");
                playerAnim.ResetTrigger("WalkForward");
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                z_speed = olw_speed;
                playerAnim.ResetTrigger("RunForward");
                playerAnim.SetTrigger("WalkForward");
            }
        }
        else
        {
            // Aucune touche de déplacement enfoncée, réinitialise les animations
            playerAnim.ResetTrigger("RunForward");
            playerAnim.ResetTrigger("WalkForward");
            playerAnim.ResetTrigger("WalkBackward");
            playerAnim.SetTrigger("Idle");
            z_speed = olw_speed;
            move=false;
        }
    }

    // Fonction de saut
    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            // Réinitialiser la vélocité horizontale avant le saut
            playerRigid.velocity = new Vector3(playerRigid.velocity.x, 0, playerRigid.velocity.z);

            playerRigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            playerAnim.SetTrigger("Jump");
        }
    }

    // Verifie si le personnage touche le sol
    bool IsGrounded()
    {
        // Parametres pour le boxcast qui sert a avoir une detection plus large
        Vector3 boxCenter = transform.position;
        Vector3 boxHalfExtents = new Vector3(0.45f, 0.25f, 0.51f);
        Quaternion boxOrientation = Quaternion.identity;
        
        return Physics.BoxCast(boxCenter, boxHalfExtents, Vector3.down, boxOrientation, distanceToGround, layerMask);
    }

    void AttackPlayer()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (walking && !move)
            {
                // Si le joueur est en train de marcher, interrompre la marche et déclencher l'animation d'attaque
                playerAnim.ResetTrigger("WalkForward");
                playerAnim.SetTrigger("Attack1");
                move = true;
                playerRigid.velocity = Vector3.zero; // Arrête le mouvement
            }
            else if (!move)
            {
                // Si le joueur n'est pas en train de marcher et n'est pas déjà en mode attaque, déclencher l'animation d'attaque
                playerAnim.SetTrigger("Attack1");
            }

            // Vérifie les collisions avec la tourelle pour infliger des dégats
            RaycastHit hitInfo;
            if (Physics.Raycast(playerTrans.position, playerTrans.forward, out hitInfo, attackRange))
            {
                TurretController turret = hitInfo.collider.GetComponent<TurretController>();
                if (turret != null)
                {
                    turret.TakeDamage(attackDamage);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        // Verifie le bon Tag pour le collectible
        if (other.gameObject.CompareTag("PickUp")) 
        {
            // Desactive le collectible
            other.gameObject.SetActive(false);
            if(playerHealth < 10)
            {
                playerHealth+=1;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if(playerHealth > 0.0)
        {
            playerHealth -= damage;
            Debug.Log("Vie du joueur : " + playerHealth);
        }
    }

    // Propriété pour accéder à playerHealth
    public float PlayerHealth
    {
        get { return playerHealth; }
    }
}
