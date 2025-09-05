using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Boss3 : MonoBehaviour
{
    [SerializeField] private HealthbarBoss healthbar;
    [SerializeField] public float maxHealth;

    private float health;
    private float currentHealth;
    public int valueBoss = 200;

    private SpriteRenderer spriteRenderer;
    public AudioSource Clip;

    // cache de yield para evitar GC en cada golpe
    private static readonly WaitForSeconds hitFlashDelay = new WaitForSeconds(0.1f);

    private bool isDying;

    void Awake()
    {
        // Autowire defensivo para evitar NRE si te olvidaste de arrastrar refs
        if (!spriteRenderer) TryGetComponent(out spriteRenderer);
        if (!Clip) TryGetComponent(out Clip);
        if (!healthbar) healthbar = GetComponentInChildren<HealthbarBoss>(true); // incluye hijos inactivos

        if (!spriteRenderer) Debug.LogError("[Boss3] Falta SpriteRenderer.", this);
        if (!Clip) Debug.LogWarning("[Boss3] Falta AudioSource (Clip). No sonará al recibir daño.", this);
        if (!healthbar) Debug.LogWarning("[Boss3] Falta HealthbarBoss. No se actualizará la UI.", this);
    }

    void Start()
    {
        health = maxHealth;
        if (healthbar) healthbar.UpdateHealthbar(maxHealth, health);
    }

    // No hagas GetComponent cada frame — cacheado en Awake
    // void Update() { }

    public void TakeDamage(float damage)
    {
        if (isDying) return;           // evita reentradas
        StartCoroutine(DamageCoroutine(damage));
    }

    private IEnumerator DamageCoroutine(float damage)
    {
        health -= damage;
        if (healthbar) healthbar.UpdateHealthbar(maxHealth, health);

        if (health > 0f)
        {
            if (Clip) Clip.Play();
            if (spriteRenderer)
            {
                spriteRenderer.color = Color.red;
                yield return hitFlashDelay; // cacheado
                if (spriteRenderer) spriteRenderer.color = Color.white;
            }
            yield break;
        }

        // Muerte (una sola vez)
        isDying = true;

        // Puntaje/fin de nivel ANTES de cambiar de escena
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(valueBoss);
            // si tenés lógica de fin, podrías llamarla aquí también
        }
        else
        {
            Debug.LogWarning("[Boss3] GameManager.Instance es null al morir Boss3.", this);
        }

        Destroy(gameObject);

        // LoadScene se completa en el PRÓXIMO frame (no inmediato)
        SceneManager.LoadScene(11); // victoria boss 3
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject && collision.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance != null) GameManager.Instance.LoseLive(3);
            else Debug.LogWarning("[Boss3] GameManager.Instance es null en OnCollisionEnter2D.", this);
        }
    }
}
