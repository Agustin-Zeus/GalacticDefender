using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LaserBoss : MonoBehaviour
{
    private Vector3 moveDirection;

    [SerializeField]
    public float speed; // Velocidad del láser

    [SerializeField]
    public int damage ; // Daño del láser

    [SerializeField]
    public float lifeTime ;

    private void Update()
    {
        transform.Translate(moveDirection * Time.deltaTime);
    }

    public void SetMoveDirection(Vector3 dir)
    {
        moveDirection = dir;
    }

    private void Destroy()

    {
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.LoseLive();
            //Aplicar aqui el instance de la animacion del impacto 
        }
    }
}