using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 1.0f; //범위
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
        Debug.Log("근접 공격");

        // attackPoint 주변 범위 내에 있는 적을 감지
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, EnemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("적 타격: " + enemy.name);
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