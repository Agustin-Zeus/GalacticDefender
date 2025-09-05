using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class FireBoss : MonoBehaviour
{
    [SerializeField] private int bulletsAmount = 10;
    [SerializeField] private float startAngle = 90f, endAngle = 270f;
    public GameObject ProyectileBoss;
    public GameObject laserPrefab;
    public Transform firePoint;
    [SerializeField] private float laserSpeed = 1;
    [SerializeField] public float lifeTimeLaser;
    public GameObject zigzagPrefab;
    [SerializeField] public float speedZigZag;
    [SerializeField] public float lifeTimeZigZag;

    private Animator animator;

    public float animationDuration = 0.30f;

    private bool isCooldownActive = false;

    public AudioSource clipAttack;

    private void Start()
    {
        StartAttackCycle();

        animator = GetComponent<Animator>();
    }

    private void StartAttackCycle()
    {
        InvokeRepeating("FireBossBullet", 1f, 4.5f);
        InvokeRepeating("ShootLaser", 4f, 6f);
        //InvokeRepeating("ShootZigzagShot", 8f, 10f);
        InvokeRepeating("FireCircle", 9f, 10f);



        // Iniciar el cooldown global cada 10 segundos (3 ataques + cooldown de 3s)
        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f); // Ataques de 10 segundos en total

            CancelInvoke("FireCircle");
            CancelInvoke("FireBossBullet");
            CancelInvoke("ShootLaser");
            CancelInvoke("ShootZigzagShot");
            isCooldownActive = true;

            yield return new WaitForSeconds(3f); // Cooldown

            isCooldownActive = false;
            StartAttackCycle(); // Reactivar el ciclo de ataques
        }
    }

    private void FireCircle()
    {
        if (isCooldownActive) return;

        animator.SetTrigger("DisparoRadial");

        float angleStep = (endAngle - startAngle) / (float)bulletsAmount;
        float angle = startAngle;

        for (int i = 0; i < bulletsAmount + 1; i++)
        {
            float bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180);
            float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180);

            Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;

            GameObject bul = BulletPoolBoss.bulletPoolInstanse.GetBullet();
            bul.transform.position = transform.position;
            bul.transform.rotation = transform.rotation;
            bul.SetActive(true);
            bul.GetComponent<BulletBoss>().SetMoveDirection(bulDir);

            angle += angleStep;
        }
    }

    private void FireBossBullet()
    {
        if (isCooldownActive) return;

        GameObject playerShip = GameObject.Find("Player") ?? GameObject.Find("Player Fast");

        if (playerShip != null)
        {
            clipAttack.Play();
            GameObject BossBullet = Instantiate(ProyectileBoss);
            BossBullet.transform.position = transform.position;

            Vector2 direction = playerShip.transform.position - BossBullet.transform.position;
            BossBullet.GetComponent<BulletBoss>().SetMoveDirection(direction);
        }
    }


    private void ShootLaser()
    {
        if (isCooldownActive) return;

        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;
        GameObject laserInstance = Instantiate(laserPrefab, spawnPosition, Quaternion.identity);
        laserInstance.GetComponent<LaserBoss>().SetMoveDirection(Vector3.down * laserSpeed);
        Destroy(laserInstance, lifeTimeLaser);
    }



    private void ShootZigzagShot()
    {
        if (isCooldownActive) return;

        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;
        GameObject zigzagInstance = Instantiate(zigzagPrefab, spawnPosition, Quaternion.identity);
        zigzagInstance.GetComponent<ZigZagShot>().SetMoveDirection(Vector3.down * speedZigZag);
        Destroy(zigzagInstance, lifeTimeZigZag);
    }
}
