using UnityEngine;

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MonsterAI : MonoBehaviour
{
    [Header("몬스터 설정")]
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

    [Header("이동 세팅")]
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float patrolRange = 3f; // 좌우 순찰 범위
    private Vector2 startPos;
    private bool movingRight = true;

    private Rigidbody2D rb;

    [Header("플레이어 관련")]
    public Transform player;
    [SerializeField] private float detectionRange = 5f; // 추격 거리
    [SerializeField] private float attackRange = 0.5f;  // 공격 거리
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackCooldown = 1f; // 1초 쿨타임
    private float lastAttackTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1f;
        rb.freezeRotation = true;

        currentHealth = maxHealth;
        startPos = transform.position;
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // 플레이어 추격
            ChasePlayer(distanceToPlayer);
        }
        else
        {
            // 순찰 모드
            Patrol();
        }
    }

    private void Patrol()
    {
        if (movingRight)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            if (transform.position.x >= startPos.x + patrolRange)
                movingRight = false;
        }
        else
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            if (transform.position.x <= startPos.x - patrolRange)
                movingRight = true;
        }
    }

    private void ChasePlayer(float distanceToPlayer)
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

        if (distanceToPlayer <= attackRange)
        {
            TryAttackPlayer();
        }
    }

    private void TryAttackPlayer()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;

            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(attackDamage);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("몬스터 HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("몬스터 사망!");
        Destroy(gameObject);
    }
}

