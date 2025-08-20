using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 3;
    public int damage = 1;

    public void TakeDamage(int damage)
    {
        //yes
        
        hp -= damage;
        Debug.Log("Enemy took damage. HP: " + hp); 

        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject);
    }
}
