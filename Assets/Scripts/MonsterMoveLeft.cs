using UnityEngine;

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MonsterAI : MonoBehaviour
{
    [Header("���� ����")]
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

    [Header("�̵� ����")]
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float patrolRange = 3f; // �¿� ���� ����
    private Vector2 startPos;
    private bool movingRight = true;

    private Rigidbody2D rb;

    [Header("�÷��̾� ����")]
    public Transform player;
    [SerializeField] private float detectionRange = 5f; // �߰� �Ÿ�
    [SerializeField] private float attackRange = 0.5f;  // ���� �Ÿ�
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackCooldown = 1f; // 1�� ��Ÿ��
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
            // �÷��̾� �߰�
            ChasePlayer(distanceToPlayer);
        }
        else
        {
            // ���� ���
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
        Debug.Log("���� HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("���� ���!");
        Destroy(gameObject);
    }
}

