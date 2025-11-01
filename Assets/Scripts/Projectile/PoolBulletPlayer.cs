using System.Collections.Generic;
using UnityEngine;

public class PoolBulletPlayer : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int poolSize = 40;
    [SerializeField] private Transform poolParent;

    private readonly Queue<GameObject> availableBullets = new Queue<GameObject>();
    private readonly List<GameObject> allBullets = new List<GameObject>();

    private void Awake()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("[PoolBulletPlayer] No se ha asignado un prefab de bala.", this);
            return;
        }

        for (int i = 0; i < poolSize; i++)
        {
            CreateBullet();
        }
    }

    private GameObject CreateBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, poolParent);
        bullet.SetActive(false);
        availableBullets.Enqueue(bullet);
        allBullets.Add(bullet);
        return bullet;
    }

    public GameObject GetBullet(float lifespan = 0f)
    {
        GameObject bullet;
        if (availableBullets.Count > 0)
        {
            bullet = availableBullets.Dequeue();
        }
        else
        {
            bullet = CreateBullet();
        }

        bullet.transform.SetParent(null, false);
        bullet.SetActive(true);

        /*
        if (lifespan > 0f)
        {
            StartCoroutine(DeactivateAfterTime(bullet, lifespan));
        }
        */
        StopCoroutine(DeactivateAfterTime(bullet, lifespan));
        StartCoroutine(DeactivateAfterTime(bullet, lifespan));

        return bullet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        if (bullet == null) return;
        bullet.SetActive(false);
        bullet.transform.SetParent(poolParent, false);
        availableBullets.Enqueue(bullet);
    }

    private System.Collections.IEnumerator DeactivateAfterTime(GameObject bullet, float time)
    {
        yield return new WaitForSeconds(time);
        if (bullet != null)
        {
            ReturnBullet(bullet);
        }
    }
}
