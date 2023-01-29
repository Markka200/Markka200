using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weakpoint : MonoBehaviour
{
  
    public void Hit()
    {
        GetComponentInParent<EnemyHealth>().TakeDamage(999);
    }
}
