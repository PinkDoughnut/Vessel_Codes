// Bullet.cs
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;

    private Rigidbody2D rigid;
    private Vector3 spawnPos;
    private float maxTravelDistance = 30f; // 최대 거리 제한

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        spawnPos = transform.position;
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        if (per == -100) return; // 근접 무기

        if (dir == Vector3.zero)
            dir = Vector3.up;

        if (rigid != null)
            rigid.linearVelocity = dir.normalized * 15f;

        Debug.DrawRay(transform.position, dir * 2f, Color.yellow, 1f);
    }

    private void Update()
    {
        if (per != -100 && Vector3.Distance(transform.position, spawnPos) > maxTravelDistance)
        {
            if (rigid != null) rigid.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (per == -100) return; // 근접 무기 제외

        // 바닥은 무시
        if (collision.CompareTag("Ground"))
            return;

        // 적이면 관통력 차감
        if (collision.CompareTag("Enemy"))
        {
            per--;
            if (per < 0)
            {
                if (rigid != null)
                    rigid.linearVelocity = Vector2.zero;
                gameObject.SetActive(false);
            }
        }
        else
        {
            // 벽, 장애물 등 다른 것에 부딪히면 사라짐
            gameObject.SetActive(false);
        }
    }
}


