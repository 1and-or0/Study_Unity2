using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // 물리 기능 *Time.deltaTime 곱할 필요가 없는 Update함수
    void FixedUpdate()
    {
        // 최대 이동속도 제한
        if (rigid.velocity.x > maxSpeed)  // right
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < -maxSpeed)  // left
            rigid.velocity = new Vector2(-maxSpeed, rigid.velocity.y);

        if (animator.GetBool("isAlive"))
        {
            // Move
            float h = Input.GetAxisRaw("Horizontal");
            rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

            // Landing 
            if (rigid.velocity.y < 0)
            {
                Debug.DrawRay(rigid.position, -Vector2.up, new Color(0, 1, 0));
                RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, -Vector2.up, 1, LayerMask.GetMask("Platform"));
                if (rayHit.collider != null)
                {
                    if (rayHit.distance < 0.5f)
                    {
                        animator.SetBool("isJumping", false);
                        // Debug.Log(rayHit.collider.name);
                    }
                }
            }
        }
        else
        {
            // 최대 천국 이송 속도 제한
            if (rigid.velocity.y > maxSpeed)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, maxSpeed);
            }
            else if (rigid.velocity.y < -maxSpeed)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, maxSpeed);
            }
            Debug.Log("날아라~");
            Welcom_Heaven(30);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(animator.GetBool("isAlive"))
        {
            if (Input.GetButtonDown("Jump") && !animator.GetBool("isJumping"))
            {   // when wanna jump
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                animator.SetBool("isJumping", true);
            }

            if (Input.GetButtonUp("Horizontal"))
            {  // when wanna stop, 
                rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.2f, rigid.velocity.y);
            }

            // Switch Direction of Sprite
            if (Input.GetButton("Horizontal"))
                spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

            // Animation - walk
            if (Mathf.Abs(rigid.velocity.x) < 0.2f)
                animator.SetBool("isWalking", false);
            else
                animator.SetBool("isWalking", true);
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Item")
        {
            // Point
            bool isBonze = other.gameObject.name.Contains("Bronze");
            bool isSilver = other.gameObject.name.Contains("Silver");
            bool isGold = other.gameObject.name.Contains("Gold");

            if (isBonze)
                gameManager.stagePoint += 50;
            else if (isSilver)
                gameManager.stagePoint += 150;
            else if (isGold)
                gameManager.stagePoint += 400;

            // DeActivate
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Finish")
        {
            // Next Stage
            gameManager.NextStage();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // Attack by stepping 
            if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.gameObject);
            }
            else // Damaged
            {
                OnDamaged(collision.transform.position);
            }
        }
    }

    void OnAttack(GameObject enemy)
    {
        // Point
        gameManager.stagePoint += 500;

        // Reaction Force
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

        // Kill Enemy
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();
    }

    // when touch Enemy
    void OnDamaged(Vector2 targetPos)
    {
        // Lose health 
        gameManager.LoseHealth();

        // Sprite Alpha Repeating
        StartCoroutine("ChangeSpriteAlpha");
        gameObject.layer = 9;

        // view alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // Reaction Force
        //int dirc = 0;
        float ReactPower = 200f;
        // rigid.AddForce(Vector2.right * dirc * ReactPower, ForceMode2D.Impulse);
        if (transform.position.x - targetPos.x > 0)
            rigid.AddForce(Vector2.right * ReactPower, ForceMode2D.Impulse);
        else if (transform.position.x - targetPos.x < 0)
            rigid.AddForce(Vector2.left * ReactPower, ForceMode2D.Impulse);

        // Animation
        animator.SetTrigger("Damaged");

        Invoke("OffDamage", 1f);
    }

    IEnumerator ChangeSpriteAlpha()
    {
        while(true)
        {
            float repeatTime = 0.07f;
            yield return new WaitForSeconds(repeatTime);

            // 함수 내용
            if (spriteRenderer.color.a == 1)
                spriteRenderer.color = new Color(1, 1, 1, 0.4f);
            else
                spriteRenderer.color = new Color(1, 1, 1, 1);
        }
    }

    void OffDamage()
    {
        gameObject.layer = 8;
        spriteRenderer.color = new Color(1, 1, 1, 1);
        
        StopCoroutine("ChangeSpriteAlpha");
    }

    // Welcome Heaven~
    void Welcom_Heaven(float flySpeed)
    {
        StopAllCoroutines();

        // Sprite Alpha
        spriteRenderer.color = new Color(0.75f, 0.9f, 0.97f, 0.35f);

        // 승천
        rigid.AddForce(Vector2.up * flySpeed, ForceMode2D.Force); 
    }

    public void youDie()
    { 
        // Alive -> dead
        animator.SetBool("isAlive", false);

        // Sprite Alpha
        //spriteRenderer.color = new Color(0.75f, 0.9f, 0.97f, 0.35f);

        // Sprite Flip Y
        //spriteRenderer.flipY = true;

        // Collider disable
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;

        // Die Effect Jump
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Force);
        
        // Welcome Heaven~
        //Invoke("Welcom_Heaven", 0.5f);
        
        // Destroy
        // Invoke("DeActive", 5);
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
        animator.SetBool("isAlive", true);
        gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

}
