using UnityEngine;

public class RespawnOnHit : MonoBehaviour
{
    [Header("HP ����")]
    public int maxHP = 3;            // Inspector���� ���� ����
    public int currentHP;

    [Header("������ ��ġ")]
    public Transform respawnPoint;   // Inspector���� ����

    [Header("��� ����")]
    public int lives = 3;            // ��� ����

    private void Start()
    {
        currentHP = maxHP;
        // �ʱ� ��ġ�� respawnPoint�� ��Ƶ� ��
        if (respawnPoint == null)
            respawnPoint = new GameObject("RespawnPoint").transform;
    }

    // ������ ó��
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP > 0)
        {
            Respawn(); // HP �ϳ��� ������ ������
        }
        else
        {
            lives--;  // ��� ����
            if (lives > 0)
            {
                currentHP = maxHP;
                Respawn(); // ���� ������� ������
            }
            else
            {
                Debug.Log("Game Over!");
                gameObject.SetActive(false);
            }
        }
    }

    // ������ �Լ�
    void Respawn()
    {
        // ��ġ �ʱ�ȭ
        transform.position = respawnPoint.position;

        // �ʿ��ϸ� ���⼭ ���� �ð�, ������ ó�� ����
    }
}
