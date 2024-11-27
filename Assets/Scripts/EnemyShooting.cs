using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab untuk EnemyBulletScript
    public GameObject pbPrefab; // Prefab untuk EnemyPBScript
    public GameObject hmPrefab; // Prefab untuk peluru jenis baru
    public Transform bulletPos; // Posisi tetap untuk peluru biasa
    public Transform[] pbPositions; // Array posisi peluru untuk PB
    public Transform[] hmPositions; // Array posisi peluru untuk jenis baru
    public float projectileSpeed = 10f; // Kecepatan peluru
    public float attackCooldown = 3f; // Cooldown setelah semua proyektil ditembakkan
    private int currentRound = 0; // Melacak urutan siklus tembakan
    private float lastAttackTime = 0f; // Waktu terakhir musuh menyerang
    private GameObject player;
    public bool isShooting = false;
    private Animator animator; // Referensi ke Animator

    void Start()
    {
        // Temukan pemain dengan tag "Player"
        player = GameObject.FindGameObjectWithTag("Player");

        // Ambil komponen Animator
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Animator tidak ditemukan pada gameObject ini!");
        }
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < 15)
        {
            if (Time.time >= lastAttackTime + attackCooldown && !isShooting)
            {
                switch (currentRound)
                {
                    case 0:
                    case 1:
                        StartCoroutine(ShootBullet());
                        currentRound++;
                        break;
                    case 2:
                        StartCoroutine(ShootPB());
                        currentRound++;
                        break;
                    case 3:
                    case 4:
                        StartCoroutine(ShootBullet());
                        currentRound++;
                        break;
                    case 5:
                        StartCoroutine(ShootPB());
                        currentRound++;
                        break;
                    case 6:
                        StartCoroutine(ShootHM());
                        currentRound = 0; // Reset kembali ke ronde pertama
                        break;
                }
            }
        }
    }

    private IEnumerator ShootBullet()
    {
        isShooting = true;

        // Trigger animasi tembakan biasa
        animator?.SetTrigger("isShooting");

        // Loop untuk menembakkan 6 peluru dari posisi tetap
        for (int i = 0; i < 6; i++)
        {
            GameObject projectile = Instantiate(bulletPrefab, bulletPos.position, bulletPos.rotation);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 direction = (player.transform.position - bulletPos.position).normalized;
                rb.velocity = direction * projectileSpeed;
            }

            lastAttackTime = Time.time;

            yield return new WaitForSeconds(0.2f); // Jeda 0.2 detik antar peluru
        }

        isShooting = false;
        yield return null;
    }

    private IEnumerator ShootPB()
    {
        isShooting = true;

        // Trigger animasi untuk peluru PB
        animator?.SetTrigger("isShooting");

        // Menembakkan 1 peluru PB dari posisi pertama dalam array
        if (pbPositions.Length > 0)
        {
            Transform shootPosition = pbPositions[0];
            GameObject projectile = Instantiate(pbPrefab, shootPosition.position, shootPosition.rotation);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 direction = (player.transform.position - shootPosition.position).normalized;
                rb.velocity = direction * projectileSpeed;
            }

            yield return new WaitForSeconds(0.2f); // Jeda 0.2 detik untuk peluru PB
        }

        lastAttackTime = Time.time;
        isShooting = false;
        yield return null;
    }

    private IEnumerator ShootHM()
    {
        isShooting = true;

        // Trigger animasi untuk peluru HM
        animator?.SetTrigger("isShooting");

        // Menembakkan 1 peluru HM dari posisi pertama dalam array
        if (hmPositions.Length > 0)
        {
            Transform shootPosition = hmPositions[0];
            GameObject projectile = Instantiate(hmPrefab, shootPosition.position, shootPosition.rotation);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 direction = (player.transform.position - shootPosition.position).normalized;
                rb.velocity = direction * projectileSpeed;
            }

            yield return new WaitForSeconds(0.2f); // Jeda 0.2 detik untuk peluru HM
        }

        lastAttackTime = Time.time;
        isShooting = false;
        yield return null;
    }
}
