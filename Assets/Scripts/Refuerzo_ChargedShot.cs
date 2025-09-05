using System.Collections;
using UnityEngine;

public class ReinforcementShip : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab del proyectil
    public Transform firePoint; // Punto desde donde se dispararán los proyectiles
    public float bulletSpeed = 5f; // Velocidad del proyectil
    public float fireInterval = 0.5f; // Intervalo entre disparos
    private bool isShooting = true; // Control de si la nave debe disparar o no

    private void Start()
    {
        // Iniciar la corrutina para disparar continuamente
        StartCoroutine(ShootAtCenter());
    }

    private IEnumerator ShootAtCenter()
    {
        // Disparar continuamente mientras la nave esté activa
        while (isShooting)
        {
            FireProjectile();
            yield return new WaitForSeconds(fireInterval);
        }
    }

    private void FireProjectile()
    {
        // Crear un proyectil y dispararlo hacia el centro (hacia arriba)
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.up * bulletSpeed; // Dirección hacia el centro (vertical hacia arriba)
    }

    public void StopShooting()
    {
        // Detener los disparos (por ejemplo, si la nave debe dejar de disparar)
        isShooting = false;
    }
}

