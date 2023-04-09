using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove1 : MonoBehaviour
{
    ActionRecorder actionRecorder;
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;
    private void Awake()
    {
        actionRecorder = GetComponent<ActionRecorder>();
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
        // Move
        float h = Input.GetAxisRaw("Horizontal");
        if (h != 0f)
        {
            Debug.Log("이동중");
            rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
        }

        if (h == 1)
        { // move right
            Debug.Log("right");
            actionRecorder.RecordAction("MoveRight");
            actionRecorder.InvokeMinionAction();
        }
        else if (h == -1)
        { // move left
            Debug.Log("left");
            actionRecorder.RecordAction("MoveLeft");
            actionRecorder.InvokeMinionAction();
        }

        // 최대 이동속도 제한
        if (rigid.velocity.x > maxSpeed)  // right
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < -maxSpeed)  // left
            rigid.velocity = new Vector2(-maxSpeed, rigid.velocity.y);

        // Landing 
        if(rigid.velocity.y < 0)
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && !animator.GetBool("isJumping"))
        {   // when wanna jump
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            animator.SetBool("isJumping", true);
        }

        if (Input.GetButtonUp("Horizontal"))
        {  // when wanna stop, 
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.2f, rigid.velocity.y);
            actionRecorder.RecordAction("WhenStop");
            actionRecorder.InvokeMinionAction();
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
