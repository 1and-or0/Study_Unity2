using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderController : MonoBehaviour
{
    //Rigidbody2D rigid;
    //public float maxSpeed;
    private ActionRecorder actionRecorder;
    //public GameObject minionPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        //rigid = GetComponent<Rigidbody2D>();
        actionRecorder = GetComponent<ActionRecorder>();
    }

    void FixedUpdate()
    {
        // 최대 이동속도 제한
        //if (rigid.velocity.x > maxSpeed)  // right
        //    rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        //else if (rigid.velocity.x < -maxSpeed)  // left
        //    rigid.velocity = new Vector2(-maxSpeed, rigid.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if (h == 1)
        {
            //transform.Translate(Vector3.forward * 2f);
            actionRecorder.RecordAction("MoveRight");
            actionRecorder.InvokeMinionAction();
        }
        else if (h == -1)
        {
            //transform.Translate(Vector3.back * 2f);
            actionRecorder.RecordAction("MoveLeft");
            actionRecorder.InvokeMinionAction();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //GetComponent<Animator>().SetTrigger("Jump");
            actionRecorder.RecordAction("Jump");
            actionRecorder.InvokeMinionAction();
        }
    }

    // Instantiate minion at the player's position and rotation
    //public void SpawnMinion()
    //{
    //    Instantiate(minionPrefab, transform.position, transform.rotation);
    //}
}
