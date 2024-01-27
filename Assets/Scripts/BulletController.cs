using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed = 10; // Vitesse de la balle
    public int bulletDamage = 1; // Dégats infligés par la balle

    private Vector3 bulletDirection; // Direction de la balle

    private PlayerController player;

    void Start()
    {
        // Trouver le PlayerController dans la scene et le stocker dans la variable player
        player = FindObjectOfType<PlayerController>();
        
        if (player == null)
        {
            Debug.LogError("PlayerController non trouvé dans la scene.");
        }
    }

    void Update()
    {
        // Déplacer la balle dans sa direction
        transform.Translate(bulletDirection * bulletSpeed * Time.deltaTime, Space.World);
    }

    // Définit la direction de la balle lors de sa création
    public void SetDirection(Vector3 direction)
    {
        bulletDirection = direction.normalized;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Si la balle touche le joueur, inflige des dégats et détruit la balle
            player.TakeDamage(bulletDamage);

            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            // Si la balle touche le mur, détruit la balle
            Destroy(gameObject);
        }
    }
    
}