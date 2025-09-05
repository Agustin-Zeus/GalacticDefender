using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBoss : MonoBehaviour
{
    private Vector2 moveDirection;
    [SerializeField]
    private float moveSpeed;

    private void OnEnable()
    {
        Invoke("Destroy", 3f);
    }

    private void Start()
    {
       
    }


     void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        //this is the bottom-left point of the screen
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        //top-rigth point of the screen
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        //if the bullet goes outside the sreen, destroy
        if ((transform.position.x < min.x) || (transform.position.x > max.x) ||
            (transform.position.y < min.y) || (transform.position.y > max.y))
        {
            gameObject.SetActive(false);
        }
    }

    public void SetMoveDirection(Vector2 dir)
    {
        moveDirection = dir.normalized;
    }
    
    private void Destroy() 
    
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.LoseLive(1f);
            gameObject.SetActive(false);
            //Aplicar aqui el instance de la animacion del impacto 
        }
    }
}
