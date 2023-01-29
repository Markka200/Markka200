using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofTrigger : MonoBehaviour
{
    
    Renderer Re;
    bool Up = false;
    bool Down = false;
    private void Start()
    {
        Re = this.GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Down = false;
            Up = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Up = false;
            Down = true;
        }
    }
    private void FixedUpdate()
    {
        if(Up && Re.material.color.a > 0.1f)
        {
            Re.material.color = new Color(Re.material.color.r, Re.material.color.g, Re.material.color.b, Re.material.color.a - 0.05f);
        }

        if(Down && Re.material.color.a < 0.9f) 
        {
            Re.material.color = new Color(Re.material.color.r, Re.material.color.g, Re.material.color.b, Re.material.color.a + 0.05f);
        }


    }
}
