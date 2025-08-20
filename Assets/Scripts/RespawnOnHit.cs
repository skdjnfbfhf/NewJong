using UnityEngine;

public class RespawnOnHit : MonoBehaviour
{
    [Header("HP 설정")]
    public int maxHP = 3;            // Inspector에서 설정 가능
    public int currentHP;

    [Header("리스폰 위치")]
    public Transform respawnPoint;   // Inspector에서 지정

    [Header("목숨 관리")]
    public int lives = 3;            // 목숨 개수

    private void Start()
    {
        currentHP = maxHP;
        // 초기 위치를 respawnPoint로 잡아도 됨
        if (respawnPoint == null)
            respawnPoint = new GameObject("RespawnPoint").transform;
    }

    // 데미지 처리
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP > 0)
        {
            Respawn(); // HP 하나라도 남으면 리스폰
        }
        else
        {
            lives--;  // 목숨 감소
            if (lives > 0)
            {
                currentHP = maxHP;
                Respawn(); // 남은 목숨으로 리스폰
            }
            else
            {
                Debug.Log("Game Over!");
                gameObject.SetActive(false);
            }
        }
    }

    // 리스폰 함수
    void Respawn()
    {
        // 위치 초기화
        transform.position = respawnPoint.position;

        // 필요하면 여기서 무적 시간, 깜빡임 처리 가능
    }
}
