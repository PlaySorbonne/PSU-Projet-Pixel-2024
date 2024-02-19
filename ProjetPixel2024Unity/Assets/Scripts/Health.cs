using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int health = 10;

    public void Hit(int damage) 
    {
        health -= damage;
    }
}
