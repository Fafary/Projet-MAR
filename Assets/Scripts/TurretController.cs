using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public int turretHealth = 5; // Vie de la tourelle
    public float shootingRange = 10f; // Portée de tir de la tourelle
    public float shootingCooldown = 2f;
    public GameObject bulletPrefab; // Prefab de la balle tirée par la tourelle
    public Transform firePoint; // Point de départ du tir de la tourelle
    private float timeSinceLastShot = 0f;

    void Update()
    {
        TryToShoot();
    }
    
    void TryToShoot()
    {
        // Si le temps écoulé depuis le dernier tir est supérieur au temps de recharge
        if (Time.time - timeSinceLastShot > shootingCooldown)
        {
            Shoot();
            timeSinceLastShot = Time.time; // Réinitialise le temps du dernier tir
        }
    }

    void Shoot()
    {
        // Ajouter un décalage à la position de tir
        float offsetMagnitude = 1f;
        Vector3 offset = -firePoint.up * offsetMagnitude;
        Vector3 spawnPosition = firePoint.position + offset;

        // Instanciation de la balle
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.LookRotation(-firePoint.up));

        BulletController bulletController = bullet.GetComponent<BulletController>();
        if (bulletController != null)
        {
            bulletController.SetDirection(-firePoint.up);
        }
    }

    public void TakeDamage(int damage)
    {
        turretHealth -= damage;
        Debug.Log("Vie de la tourelle : " + turretHealth);

        if (turretHealth <= 0)
        {
            Debug.Log("La tourelle a été détruite");
            Destroy(gameObject);
        }
    }
}
