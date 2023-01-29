using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    
    public float WalkSpeed = 5;
    public float RunSpeed = 1   ; 
    float moveSpeed;
    Rigidbody2D rb;
    [HideInInspector]
    public bool canMove = true;
    Vector2 mousepos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = RunSpeed;
         
        }
        else
        {
            moveSpeed = WalkSpeed;

        }

        float posx = canMove ? moveSpeed * Input.GetAxis("Horizontal") : 0;
        float posy = canMove ? moveSpeed * Input.GetAxis("Vertical") : 0;


        rb.velocity = new Vector2(posx,posy);


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.PauseGame();
        }

        mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);     // hiirtä kohti katsominen
    

        transform.up = mousepos - (Vector2) transform.position;

}
}
