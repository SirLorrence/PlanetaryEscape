using UnityEngine;


public abstract class Healable : MonoBehaviour
{
    public abstract int Health { get; set; }
    public abstract int Shield { get; set; }

    public bool IsAlive() => (Health <= 0) ? false : true;
}