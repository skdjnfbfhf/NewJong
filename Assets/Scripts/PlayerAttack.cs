using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 1.0f; //����
    public Transform attackPoint;

    public float attackCooldown = 0.1f;
    public int attackDamage = 3;
    public LayerMask EnemyLayer;
    private float lastAttackTime = -1f;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time - lastAttackTime >= attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    void Attack()
    {
        Debug.Log("���� ����");

        // attackPoint �ֺ� ���� ���� �ִ� ���� ����
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, EnemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("�� Ÿ��: " + enemy.name);
            enemy.GetComponent<Enemy>()?.TakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}