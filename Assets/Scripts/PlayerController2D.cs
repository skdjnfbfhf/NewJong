using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("�̵�")]
    [SerializeField] private float moveSpeed = 6f;  // �¿� �̵� �ӵ�

    [Header("����")]
    [SerializeField] private float jumpImpulse = 10f;       // ���� ��(���޽�)
    [SerializeField] private Transform groundCheck;         // �� ��ġ ������
    [SerializeField] private float groundCheckRadius = 0.2f;// �ٴ� ���� �ݰ�
    [SerializeField] private LayerMask groundLayer;         // �ٴ� ���̾�
    [SerializeField] private float coyoteTime = 0.1f;       // �ڿ��� Ÿ��(�ٴ� ���� ���� ���� ��� �ð�)
    [SerializeField] private float jumpBufferTime = 0.1f;   // ���� �Է� ����(���� ���� �ణ �ʰ� �ٴ��̾ ����)

    private Rigidbody2D rb;
    private float moveInput;            // -1 ~ 1 (��/�� �Է�)
    private float lastGroundedTime;     // �ֱ� �ٴ� ���� Ÿ�̸�
    private float lastJumpPressedTime;  // �ֱ� ���� ���� Ÿ�̸�
    private bool isGrounded;
    private Animator Anim;

    [Header("�÷��̾� ü��")]
    public int maxHp = 3;
    public int currentHp = 3;
    private bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();

        // ������ġ
        rb.freezeRotation = true;

        // ü�� �ʱ�ȭ
        currentHp = maxHp;
    }

    private void Update()
    {
        if (isDead) return; // �׾����� �Է� ����

        // 1) �¿� �Է� (A/D, ��/�� �⺻ ����)
        moveInput = Input.GetAxisRaw("Horizontal"); // -1,0,1

        // 2) ���� �Է� ���
        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space))
            lastJumpPressedTime = jumpBufferTime;

        // 3) Ÿ�̸� ����
        if (lastGroundedTime > 0) lastGroundedTime -= Time.deltaTime;
        if (lastJumpPressedTime > 0) lastJumpPressedTime -= Time.deltaTime;

        // 4) �ٶ󺸴� ���� ��ȯ
        if (moveInput != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(moveInput) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        // �ִϸ��̼� ó��
        Anim.SetBool("isRunning", rb.velocity.x != 0);
    }

    private void FixedUpdate()
    {
        if (isDead) return; // �׾����� �̵� �Ұ�

        // A) �ٴ� üũ
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded)
            lastGroundedTime = coyoteTime;

        // B) �̵�
        float targetVelX = moveInput * moveSpeed;
        rb.velocity = new Vector2(targetVelX, rb.velocity.y);

        // C) ����
        if (lastGroundedTime > 0 && lastJumpPressedTime > 0)
        {
            DoJump();
            lastJumpPressedTime = 0;
            lastGroundedTime = 0;
        }
    }

    private void DoJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpImpulse, ForceMode2D.Impulse);
    }

    // Gizmos (�ٴ� üũ �ð�ȭ)
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    // ========== ü��/������ ó�� ==========
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHp -= damage;
        Debug.Log("�÷��̾� HP: " + currentHp);

        if (currentHp <= 0)
        {
            Die();
        }
        else
        {
            // �ǰ� �ִϸ��̼� �߰� ����
            Anim.SetTrigger("Hit");
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("�÷��̾� ���!");

        // ���� �ִϸ��̼� Ʈ����
        Anim.SetTrigger("Die");

        // ���� �ð� �� �����ϰų�, ���� ���� UI ���� ����
        Destroy(gameObject, 1.5f);
    }
}


