using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    
    [SerializeField] GameObject Player;
    [SerializeField] private IntVariable _score;

    
      void  OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           _score.Value++;
        }
        
    }
}

