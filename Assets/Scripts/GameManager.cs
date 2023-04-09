using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public PlayerMove playerMove;
    public GameObject[] stages;

    public void NextStage()
    {
        // Change Stage
        if(stageIndex < stages.Length-1)
        {
            stages[stageIndex].SetActive(false);
            stageIndex++;
            stages[stageIndex].SetActive(true);
            PlayerReposition(); // Player Reposition
        }
        else
        {
            // Game Clear
            Debug.Log("Game Clear!!!");
            Time.timeScale = 0;
            
            // Player Control Lock
        }

        // Point Summary
        totalPoint += stagePoint;
        stagePoint = 0;
    }

    public void LoseHealth()
    { 
        if (health > 1) 
            health--;
        else
        {
            // Player Die Effect
            playerMove.youDie();

            // Result UI
            Debug.Log("You Die!!");

            // Retry Button UI
            Invoke("PlayerReposition", 2f);
            stages[stageIndex].SetActive(false);
            stageIndex = 0;
            stages[stageIndex].SetActive(true);
            health = 3;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            LoseHealth();

            // Repositioning Player
            // collision.attachedRigidbody.velocity = Vector2.zero;
            PlayerReposition();
            // collision.gameObject.transform.position = new Vector2(0, 0);
        }
    }

    void PlayerReposition()
    {
        playerMove.transform.position = Vector2.zero;
        playerMove.VelocityZero();
        
    }

    void Awake()
    {
        
    }

    void Update()
    {
        
    }
}
