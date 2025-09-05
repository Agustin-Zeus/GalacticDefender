using System.Collections;
using UnityEngine;

public class Boss_2_Ataques : MonoBehaviour
{
    public PoolProyectilesBoss2 projectilePool;
    public float projectileSpeed = 5f;

    // Ataque lateral
    public PoolMisil_boss2 MisilPool;
    public float timeBetweenShots = 0.5f;
    public int numProjectiles = 5;
    public float attackHeight = 5f;
    public float distanceOffScreen = 10f;
    private bool isAttacking = false;

    public float cooldownTime = 5f;
    private bool isCooldown = false;
    private bool isCooldownActive = false;

    public AudioSource clipAttack;
    public AudioSource clipTentaculos;

    // Sprite de advertencia
    public GameObject missileWarningSprite;
    public float warningDuration = 1.5f;

    void Start()
    {
        AttackCycle();
        if (missileWarningSprite != null)
        {
            missileWarningSprite.SetActive(false); // Asegúrate de que esté desactivado al inicio
        }
    }

    private void AttackCycle()
    {
        InvokeRepeating(nameof(RadialAttack), 1f, 3f);
        InvokeRepeating(nameof(LaunchMissiles), 0f, 3f);
        StartCooldown();
    }

    private void StartCooldown()
    {
        Invoke("StartCooldownCycle", 12f);
    }

    private void StartCooldownCycle()
    {
        CancelInvoke("RadialAttack");
        CancelInvoke("LaunchMissiles");

        isCooldownActive = true;

        Invoke("EndCooldown", 3f);
    }

    private void EndCooldown()
    {
        isCooldownActive = false;
        AttackCycle();
    }

    void LaunchMissiles()
    {
        if (isCooldownActive) return;

        StartCoroutine(LaunchMissileWithWarning());
    }

    IEnumerator LaunchMissileWithWarning()
    {
        // Mostrar advertencia
        if (missileWarningSprite != null)
        {
            missileWarningSprite.SetActive(true);
        }

        yield return new WaitForSeconds(warningDuration);

        // Ocultar advertencia
        if (missileWarningSprite != null)
        {
            missileWarningSprite.SetActive(false);
        }

        // Disparar misiles
        for (int i = 0; i < numProjectiles; i++)
        {
            LaunchMissile(i);
        }
    }

    void LaunchMissile(int index)
    {
        GameObject missile = MisilPool.GetProjectile();
        missile.GetComponent<Misil_boss_2>().SetPool(MisilPool);

        float side = index % 2 == 0 ? -1f : 1f;
        missile.transform.position = new Vector3(side * distanceOffScreen, Random.Range(-attackHeight, 2), 0);
        missile.SetActive(true);

        missile.GetComponent<Misil_boss_2>().SetHorizontalMovement(side);
        clipTentaculos.Play();
    }

    void RadialAttack()
    {
        if (isCooldownActive) return;

        int numProjectiles = 12;
        float angleStep = 360f / numProjectiles;
        float angle = 0f;

        for (int i = 0; i < numProjectiles; i++)
        {
            clipAttack.Play();

            GameObject proj = projectilePool.GetProjectile();
            proj.transform.position = transform.position;

            float dirX = Mathf.Sin(angle * Mathf.Deg2Rad);
            float dirY = Mathf.Cos(angle * Mathf.Deg2Rad);
            Vector3 direction = new Vector3(dirX, dirY, 0);

            proj.GetComponent<Boss_2_Proyectile>().SetDirection(direction, projectilePool);

            angle += angleStep;
        }
    }
}
