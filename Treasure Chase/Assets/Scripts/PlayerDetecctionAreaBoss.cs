using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetecctionAreaBoss : MonoBehaviour
{
    public BossController boss;


    private void Awake()
    {
        if (boss == null)
        {
            boss = GetComponentInParent<BossController>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boss.PlayerDetected(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boss.PlayerLost();
        }
    }
}

