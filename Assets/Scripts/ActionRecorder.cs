using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionRecorder : MonoBehaviour
{
    public GameObject cloneObj;
    public float clone_jumpPower;
    Rigidbody2D rigid;
    Animator animator;
    private List<string> actionList = new List<string>();
    // Start is called before the first frame update
    void Awake()
    {
        animator = cloneObj.GetComponent<Animator>();
        clone_jumpPower = GetComponent<PlayerMove>().jumpPower;
        rigid = cloneObj.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    // Player action recording function
    public void RecordAction(string action)
    {
        Debug.Log(action);
        actionList.Add(action);
    }

    // Invoke function to delay the minion action
    public void InvokeMinionAction()
    {
        Invoke("MinionAction", 2f);
    }

    // Minion action implementation
    public void MinionAction()
    {
        // Get the action recorded by the player
        //string action = actionList[actionList.Count - 1];
        string action = actionList[0];
        actionList.RemoveAt(0);
        Debug.Log(action);
        // Implement the action for the minion
        switch (action)
        {
            case "MoveRight":
                Debug.Log("오른 쪽");
                animator.SetBool("isWalking", true);
                rigid.AddForce(Vector2.right, ForceMode2D.Impulse);
                break;
            case "MoveLeft":
                Debug.Log("왼 쪽");
                animator.SetBool("isWalking", true);
                rigid.AddForce(Vector2.left, ForceMode2D.Impulse);
                break;
            case "Jump":
                Debug.Log("점프");
                animator.SetBool("isJumping", true);
                rigid.AddForce(Vector2.up * clone_jumpPower, ForceMode2D.Impulse);
                break;
            case "WhenStop":
                animator.SetBool("isWalking", false);
                rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.2f, rigid.velocity.y);
                break;
                // GetComponent<Animator>().SetTrigger("Jump");
                //break;
                // Add more cases for other actions as needed
        }
    }
}
