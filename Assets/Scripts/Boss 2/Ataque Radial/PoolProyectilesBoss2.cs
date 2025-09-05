using System.Collections.Generic;
using UnityEngine;

public class PoolProyectilesBoss2 : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefabricado del proyectil
    public int poolSize = 20; // Tamaño inicial del pool
    private List<GameObject> pool; // Lista que almacena los proyectiles

    void Start()
    {
        pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject proj = Instantiate(projectilePrefab); // Instanciamos los proyectiles
            proj.SetActive(false); // Los desactivamos para que no estén en la escena
            pool.Add(proj); // Los añadimos al pool
        }
    }

    // Obtiene un proyectil del pool
    public GameObject GetProjectile()
    {
        foreach (var proj in pool)
        {
            if (!proj.activeInHierarchy) // Si el proyectil no está activo
            {
                proj.SetActive(true); // Lo activamos
                return proj; // Lo devolvemos
            }
        }

        // Si no hay proyectiles inactivos, expandimos el pool
        Debug.LogWarning("Expandiendo el pool de proyectiles.");
        GameObject newProj = Instantiate(projectilePrefab); // Creamos un nuevo proyectil
        newProj.SetActive(true); // Lo activamos
        pool.Add(newProj); // Lo agregamos al pool
        return newProj; // Lo devolvemos
    }

    // Devuelve un proyectil al pool
    public void ReturnProjectile(GameObject projectile)
    {
        projectile.SetActive(false); // Desactivamos el proyectil

        // Si el proyectil tiene físicas (colisionador, etc.), reseteamos su posición y velocidad para evitar problemas
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>(); // Suponiendo que es un Rigidbody2D
        if (rb != null)
        {
            rb.velocity = Vector2.zero; // Reseteamos la velocidad
            rb.angularVelocity = 0f; // Reseteamos la velocidad angular
        }

        projectile.transform.position = Vector3.zero; // Ajusta según sea necesario
    }
}
