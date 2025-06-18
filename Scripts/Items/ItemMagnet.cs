using UnityEngine;

public class ItemMagnet : MonoBehaviour
{
    public float magnetRange = 3f;
    public float suctionSpeed = 10f;
    public float repelPower = 1.5f;
    public float absorbDelay = 0.1f;

    private Transform player;
    private bool isAbsorbing = false;
    private float absorbTimer = 0f;
    private Rigidbody2D rb;
    private bool wasRepelled = false;
    private bool absorbed = false; // ✅ 중복 방지용 플래그

    public ItemData data;

    [Header("사운드")]
    public AudioClip pickupSound;
    public AudioSource sfxAudioSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (sfxAudioSource == null)
        {
            Transform root = GameObject.Find("MasterSoundManager")?.transform;
            Transform sfx = root?.Find("SoundEffectManager/ItemPickUpSound");
            sfxAudioSource = sfx?.GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (player == null || absorbed) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (!isAbsorbing && distance < magnetRange)
        {
            if (!wasRepelled)
            {
                Vector2 repelDir = (transform.position - player.position).normalized;
                rb.AddForce(repelDir * repelPower, ForceMode2D.Impulse);
                wasRepelled = true;
            }

            absorbTimer += Time.deltaTime;
            if (absorbTimer >= absorbDelay)
            {
                isAbsorbing = true;
                rb.angularVelocity = 0f;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }

        if (isAbsorbing)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            transform.position += (Vector3)(dir * suctionSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.position) < 0.2f)
            {
                Absorb();
            }
        }
    }

    void Absorb()
    {
        if (absorbed) return;
        absorbed = true;

        Debug.Log($"{data.itemName} 획득!");

        if (pickupSound != null && sfxAudioSource != null)
        {
            sfxAudioSource.PlayOneShot(pickupSound);
        }

        MaterialInventoryManager.instance.AddMaterial(data);
        Destroy(gameObject, 0.1f);
    }
}
