//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;
    CapsuleCollider2D capsuleCollider;
    public int nextMove;
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        Think();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        // Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        if (animator.GetBool("isAlive"))
        {
            // 지형 체크
            Vector2 frontV = new Vector2(rigid.position.x + (nextMove * 0.4f), rigid.position.y);
            Debug.DrawRay(frontV, -Vector2.up, new Color(1, 0, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(frontV, -Vector2.up, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider == null)
            {
                Turn();
            }
        }

    }

    void Think()
    {
        if (!animator.GetBool("isAlive"))
            return;

        // do next walking
        nextMove = Random.Range(-1, 2);
        
        // Set Anime Int
        animator.SetInteger("WalkSpeed", nextMove);
        
        // Flip Sprite
        if(nextMove != 0) // 멈췄을 때 항상 같은 방향을 보지 않도록 함.
            spriteRenderer.flipX = (nextMove == 1);

        // 재귀 recursive
        float nextTime = Random.Range(2f, 5f);
        //Debug.Log("nextTime: " + nextTime);
        Invoke("Think", nextTime);
    }

    // 낭떠러지에서 180도 도는 함수
    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = (nextMove == 1);
        CancelInvoke();
        Invoke("Think", 5);
    }

    public void OnDamaged()
    {
        // Alive -> dead
        animator.SetBool("isAlive", false);

        // Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // Sprite Flip Y
        spriteRenderer.flipY = true;

        // Collider disable
        capsuleCollider.enabled = false;

        // Die Effect Jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        // Destroy
        Invoke("DeActive", 5);
    }
    
    void DeActive()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        
    }
}
