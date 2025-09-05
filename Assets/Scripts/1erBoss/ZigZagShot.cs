using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigZagShot : MonoBehaviour
{
    public float speed = 5f; 
    public float zigzagAmplitude = 2f; 
    public float zigzagFrequency = 2f; 

    private Vector3 moveDirection = Vector3.down; 
    private float elapsedTime; 

    void Update()
    {
        
        elapsedTime += Time.deltaTime * zigzagFrequency;

       
        float xOffset = Mathf.Sin(elapsedTime) * zigzagAmplitude;
        //  Vector3 zigzagMovement = new Vector3(xOffset, moveDirection.y, 0);
        Vector3 zigzagMovement = new Vector3(xOffset, -1f, 0);

        transform.Translate(zigzagMovement * speed * Time.deltaTime);
    }

    public void SetMoveDirection(Vector3 newDirection)
    {
        moveDirection = newDirection;
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
