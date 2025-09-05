using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletPoolBoss : MonoBehaviour
{
   public static BulletPoolBoss bulletPoolInstanse;

    [SerializeField]
    private GameObject pooledBullet;
    private bool notEnoughBulletsPool = true;

    private List<GameObject> bullets;

    private void Awake()
    {
        bulletPoolInstanse = this;
    }

    private void Start()
    {
        bullets = new List<GameObject>();   
    }

    public GameObject GetBullet() 
    
    {
        if (bullets.Count > 0)
        
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                if (!bullets[i].activeInHierarchy)
                {
                    return bullets[i];
                }
            }
        }

        if (notEnoughBulletsPool)
        {
            GameObject bul = Instantiate(pooledBullet);
            bul.SetActive(false);
            bullets.Add(bul);
            return bul;

        }
        return null;

    }

 


}
