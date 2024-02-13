using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCombat : MonoBehaviour
{
    private HashSet<GameObject> ennemiesInRange = new HashSet<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("enter");
        if (col.gameObject.CompareTag("attackable") && col.gameObject.GetComponent<PlayerHealth>() != null) 
        {
            ennemiesInRange.Add(col.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (ennemiesInRange.Contains(col.gameObject))
        {
            ennemiesInRange.Remove(col.gameObject);
        }
    }

    private void OnAttack()
    {
        foreach (var obj in ennemiesInRange)
        {
            obj.GetComponent<PlayerHealth>().Hit();
        }
    }
}
