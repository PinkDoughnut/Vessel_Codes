using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public enum EnemyType { Zombie, Ghost, Skeleton }
    public EnemyType enemyType;

    public float speed;
    public float health;
    public float maxHealth;
    public ItemDropper itemDropper;
    public SpriteRenderer sr;
    public LayerMask obstacleMask;

    public AudioSource audioSource;
    public AudioClip zombieDeathSound;
    public AudioClip ghostDeathSound;
    public AudioClip skeletonDeathSound;

    private Rigidbody2D rigid;
    private Collider2D coll;
    private Animator anim;
    private SpriteRenderer spriter;
    private Rigidbody2D target;
    private WaitForFixedUpdate wait;
    private bool isLive;

    private int defaultLayer;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        defaultLayer = gameObject.layer;
    }

    void OnEnable()
    {
        gameObject.layer = defaultLayer;

        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive || !isLive)
            return;

        Vector2 dirVec = target.position - rigid.position;

        // Raycast 거리 증가 및 시체 무시용 레이어 감지 방지
        RaycastHit2D hit = Physics2D.Raycast(rigid.position, dirVec.normalized, 2.5f, obstacleMask);
        Debug.DrawRay(rigid.position, dirVec.normalized * 2.5f, Color.red);

        if (hit.collider != null)
        {
            dirVec = Vector2.Perpendicular(dirVec); // 장애물 피하기
        }

        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.linearVelocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        if (isLive)
            spriter.flipX = target.position.x < rigid.position.x;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if (health > 0)
            StartCoroutine(HitEffect());
        else
            Die();
    }

    IEnumerator KnockBack()
    {
        yield return wait;
        Vector3 dirVec = transform.position - (Vector3)target.position;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    IEnumerator HitEffect()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        sr.color = Color.white;
    }

    void Die()
    {
        isLive = false;
        coll.enabled = false;
        rigid.simulated = false;
        spriter.sortingOrder = 1;
        anim.SetBool("Dead", true);

        // 죽은 시체는 레이어 변경 (Raycast 감지 안 되게)
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        PlayDeathSound();

        GameManager.instance.kill++;
        GameManager.instance.GetExp();

        if (itemDropper != null)
            itemDropper.Drop(transform.position);

        Invoke(nameof(Deactivate), 2f);
    }

    void PlayDeathSound()
    {
        if (audioSource == null) return;

        switch (enemyType)
        {
            case EnemyType.Zombie:
                if (zombieDeathSound != null)
                    audioSource.PlayOneShot(zombieDeathSound);
                break;
            case EnemyType.Ghost:
                if (ghostDeathSound != null)
                    audioSource.PlayOneShot(ghostDeathSound);
                break;
            case EnemyType.Skeleton:
                if (skeletonDeathSound != null)
                    audioSource.PlayOneShot(skeletonDeathSound);
                break;
        }
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
