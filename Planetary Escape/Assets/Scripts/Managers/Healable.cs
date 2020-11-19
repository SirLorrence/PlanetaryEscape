using UnityEngine;


public abstract class Healable : MonoBehaviour
{
    public abstract int Health { get; set; }

    public bool IsAlive() => (Health <= 0) ? false : true;
}