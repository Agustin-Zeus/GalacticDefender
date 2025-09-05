using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBulletPlayer : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab de la bala
    public int poolSize = 20;       // Tamaño inicial del pool
    private List<GameObject> bulletPool;

    void Start()
    {
        // Inicializa la lista del pool
        bulletPool = new List<GameObject>();

        // Crea las balas inactivas y las agrega al pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);  // Desactiva la bala inmediatamente
            bulletPool.Add(bullet);   // Agrega la bala a la lista
        }
    }

    // Función para obtener una bala del pool
    public GameObject GetBullet(float lifespan)
    {
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                // Activa la bala cuando sea reutilizada
                bullet.SetActive(true);
                return bullet;
            }
        }

        // Si todas las balas están en uso, puedes optar por agregar una nueva bala al pool
        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.SetActive(true);
        bulletPool.Add(newBullet); // Agrega la nueva bala al pool
        return newBullet;
    }

    // Función para devolver la bala al pool
    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false); // Desactiva la bala para que se pueda reutilizar
    }
    private IEnumerator DeactivateBulletAfterTime(GameObject bullet, float time)
    {
        yield return new WaitForSeconds(time);
        ReturnBullet(bullet);

    }
}
