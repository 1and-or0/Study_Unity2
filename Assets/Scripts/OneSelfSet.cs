using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class OneSelfSet : MonoBehaviour
{
    public GameObject parentObj;
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        maxSpeed = parentObj.GetComponent<PlayerMove>().maxSpeed;
        jumpPower = parentObj.GetComponent<PlayerMove>().jumpPower;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // oneSelf speed max
        if (rigid.velocity.x > maxSpeed) // right
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if(rigid.velocity.x < -maxSpeed)  // left
            rigid.velocity = new Vector2(-maxSpeed, rigid.velocity.y);
    }
}
