using System.Collections;
using UnityEngine;

public class Boss_2_Proyectile : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 5f; // Tiempo que el proyectil estará activo antes de regresar al pool
    private float targetX = 9; // El punto final en el eje X

    private Vector3 direction; // Dirección en la que se moverá el proyectil
    private PoolProyectilesBoss2 pool; // Referencia al pool del proyectil

    void OnEnable()
    {
        // Reiniciar cualquier valor relevante cuando el proyectil se activa
        CancelInvoke();  // Cancelar cualquier invocación previa
        
    }

    public void SetPool(PoolProyectilesBoss2 proyectilePool)
    {
        pool = proyectilePool;
    }


    void OnDisable()
    {
        CancelInvoke(); // Cancela la desactivación si el proyectil se desactiva por otra razón
    }

    // Método para configurar la dirección y el pool
    public void SetDirection(Vector3 newDirection, PoolProyectilesBoss2 poolReference)
    {
        direction = newDirection.normalized; // Normaliza la dirección
        pool = poolReference; // Asigna el pool
    }

    void Update()
    {
        if (pool == null) return; // Verifica que el pool esté asignado

        // Mueve el proyectil en la dirección especificada
        transform.position += direction * speed * Time.deltaTime;

        // Si el proyectil llega al final de su trayecto (fuera de la pantalla)
        if (Mathf.Abs(transform.position.x) >= Mathf.Abs(targetX))
        {
            // Desactivar el proyectil y devolverlo al pool
            gameObject.SetActive(false);
            pool.ReturnProjectile(gameObject);  // Devuelve el proyectil al pool
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.LoseLive(1f);
            pool.ReturnProjectile(gameObject);
            //Aplicar aqui el instance de la animacion del impacto 
        }
    }
}
