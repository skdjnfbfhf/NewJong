using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("이동")]
    [SerializeField] private float moveSpeed = 6f;  // 좌우 이동 속도

    [Header("점프")]
    [SerializeField] private float jumpImpulse = 10f;       // 점프 힘(임펄스)
    [SerializeField] private Transform groundCheck;         // 발 위치 기준점
    [SerializeField] private float groundCheckRadius = 0.2f;// 바닥 판정 반경
    [SerializeField] private LayerMask groundLayer;         // 바닥 레이어
    [SerializeField] private float coyoteTime = 0.1f;       // 코요테 타임(바닥 떠난 직후 점프 허용 시간)
    [SerializeField] private float jumpBufferTime = 0.1f;   // 점프 입력 버퍼(누른 직후 약간 늦게 바닥이어도 점프)

    private Rigidbody2D rb;
    private float moveInput;            // -1 ~ 1 (왼/오 입력)
    private float lastGroundedTime;     // 최근 바닥 감지 타이머
    private float lastJumpPressedTime;  // 최근 점프 누름 타이머
    private bool isGrounded;
    private Animator Anim;

    [Header("플레이어 체력")]
    public int maxHp = 3;
    public int currentHp = 3;
    private bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();

        // 안전장치
        rb.freezeRotation = true;

        // 체력 초기화
        currentHp = maxHp;
    }

    private void Update()
    {
        if (isDead) return; // 죽었으면 입력 무시

        // 1) 좌우 입력 (A/D, ←/→ 기본 지원)
        moveInput = Input.GetAxisRaw("Horizontal"); // -1,0,1

        // 2) 점프 입력 기록
        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space))
            lastJumpPressedTime = jumpBufferTime;

        // 3) 타이머 감소
        if (lastGroundedTime > 0) lastGroundedTime -= Time.deltaTime;
        if (lastJumpPressedTime > 0) lastJumpPressedTime -= Time.deltaTime;

        // 4) 바라보는 방향 전환
        if (moveInput != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(moveInput) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        // 애니메이션 처리
        Anim.SetBool("isRunning", rb.velocity.x != 0);
    }

    private void FixedUpdate()
    {
        if (isDead) return; // 죽었으면 이동 불가

        // A) 바닥 체크
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded)
            lastGroundedTime = coyoteTime;

        // B) 이동
        float targetVelX = moveInput * moveSpeed;
        rb.velocity = new Vector2(targetVelX, rb.velocity.y);

        // C) 점프
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

    // Gizmos (바닥 체크 시각화)
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    // ========== 체력/데미지 처리 ==========
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHp -= damage;
        Debug.Log("플레이어 HP: " + currentHp);

        if (currentHp <= 0)
        {
            Die();
        }
        else
        {
            // 피격 애니메이션 추가 가능
            Anim.SetTrigger("Hit");
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("플레이어 사망!");

        // 죽음 애니메이션 트리거
        Anim.SetTrigger("Die");

        // 일정 시간 뒤 삭제하거나, 게임 오버 UI 띄우기 가능
        Destroy(gameObject, 1.5f);
    }
}


