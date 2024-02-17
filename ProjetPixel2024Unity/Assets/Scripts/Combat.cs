using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Combat : MonoBehaviour
{
    private int damage = 1;

    public float timeUntilDestroy = 0.5f;

    public void Start() 
    {
        StartCoroutine(TimerDestroy(timeUntilDestroy));
    }

    private IEnumerator TimerDestroy(float time) 
    {
        yield return new WaitForSeconds(time);
        DestroyHitbox();
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.GetComponent<Health>() != null && col.gameObject.tag != gameObject.tag) 
        {
            Attack(col.gameObject.GetComponent<Health>());
        }
    }

    public void Attack(Health health) 
    {
        health.Hit(damage);
    }

    public void DestroyHitbox() 
    {
        Destroy(gameObject);
    }
}
