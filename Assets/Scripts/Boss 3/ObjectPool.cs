using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject objectPrefab;   // Prefab del primer tipo
    public GameObject objectPrefab2;  // Prefab del segundo tipo
    public int poolSize = 20;

    private readonly Queue<GameObject> objectPool = new Queue<GameObject>();
    private readonly Queue<GameObject> objectPool2 = new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            var obj1 = Instantiate(objectPrefab);
            var obj2 = Instantiate(objectPrefab2);

            obj1.SetActive(false);
            obj2.SetActive(false);

            // (opcional) parentear al contenedor para orden en jerarquía
            obj1.transform.SetParent(transform);
            obj2.transform.SetParent(transform);

            objectPool.Enqueue(obj1);
            objectPool2.Enqueue(obj2);
        }
    }

    public GameObject GetObject(bool useSecondPrefab = false)
    {
        if (useSecondPrefab)
        {
            // Desencola hasta encontrar uno válido o vaciar
            while (objectPool2.Count > 0)
            {
                var obj = objectPool2.Dequeue();
                if (obj) // Unity null: false si está destruido
                {
                    obj.transform.SetParent(null, false);
                    obj.SetActive(true);
                    return obj;
                }
                // si estaba destruido, seguimos buscando
            }

            // No había válidos -> crear nuevo
            var created = Instantiate(objectPrefab2);
            created.SetActive(true);
            return created;
        }
        else
        {
            while (objectPool.Count > 0)
            {
                var obj = objectPool.Dequeue();
                if (obj)
                {
                    obj.transform.SetParent(null, false);
                    obj.SetActive(true);
                    return obj;
                }
            }

            var created = Instantiate(objectPrefab);
            created.SetActive(true);
            return created;
        }
    }

    public void ReturnObject(GameObject obj, bool useSecondPrefab = false)
    {
        // Si el objeto fue destruido (MissingReference), no intentes usarlo
        if (!obj) return; // Unity null check

        obj.SetActive(false);
        obj.transform.SetParent(transform, false);

        if (useSecondPrefab)
            objectPool2.Enqueue(obj);
        else
            objectPool.Enqueue(obj);
    }
}
