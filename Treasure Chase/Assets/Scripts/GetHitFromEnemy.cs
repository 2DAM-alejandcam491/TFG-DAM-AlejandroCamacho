using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHitFromEnemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (ScriptCharacterController.Instance.canGetHit)
            {
                Debug.Log("El jugador ha sido golpeado por el enemigo");
                GameManager.Instance.PerderVida();
            }
        }
    }
}
