using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Boss : MonoBehaviour
{
    public float speed = 2f;
    public float health = 100f;
    public float maxHealth = 100f;

    public AudioSource audioSource;
    public AudioClip spawnClip;
    public AudioClip hitClip;
    public AudioClip deathClip;

    public GameObject healthTextUI;
    public GameObject specialDropPrefab; // 드롭할 아이템 프리팹 추가

    private Rigidbody2D rb;
    private Animator anim;
    private Transform player;
    private bool isDead = false;
    private Coroutine hitFlashRoutine;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.freezeRotation = true;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (healthTextUI != null)
            healthTextUI.SetActive(false);
    }

    void Start()
    {
        OnBossSpawn();
    }

    void FixedUpdate()
    {
        if (isDead || player == null || !GameManager.instance.isLive)
            return;

        Vector2 dir = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);

        Vector3 scale = transform.localScale;
        scale.x = player.position.x < transform.position.x ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;

        if (healthTextUI != null)
            healthTextUI.transform.position = transform.position + Vector3.down * 1.5f;
    }

    public void OnBossSpawn()
    {
        if (audioSource != null && spawnClip != null)
            audioSource.PlayOneShot(spawnClip);

        if (healthTextUI != null)
            healthTextUI.SetActive(true);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        health -= damage;

        // 20% 확률로 피격 사운드 재생
        if (audioSource != null && hitClip != null && Random.value <= 0.2f)
            audioSource.PlayOneShot(hitClip);

        StartCoroutine(HitFlash());

        if (health <= 0)
            Die();
    }
    IEnumerator HitFlash()
    {
        if (hitFlashRoutine != null)
        {
            StopCoroutine(hitFlashRoutine);
            GetComponent<SpriteRenderer>().color = Color.white; // 색상 초기화
        }

        hitFlashRoutine = StartCoroutine(HitFlashRoutine());
        yield return null;
    }

    void Die()
    {
        isDead = true;
        anim.SetTrigger("Death");
        rb.simulated = false;

        if (audioSource != null && deathClip != null)
            audioSource.PlayOneShot(deathClip);

        if (healthTextUI != null)
            healthTextUI.SetActive(false);

        // 특별한 아이템 드롭
        if (specialDropPrefab != null)
        {
            Instantiate(specialDropPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null)
                TakeDamage(bullet.damage);
        }
    }

    IEnumerator HitFlashRoutine()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        sr.color = Color.white;
        hitFlashRoutine = null;
    }
}
