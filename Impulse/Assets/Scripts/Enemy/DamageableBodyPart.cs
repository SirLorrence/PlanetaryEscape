using UnityEngine;

public class DamageableBodyPart : MonoBehaviour
{
    
    public enum BodyParts
    {
        Head,
        Body,
        Arm,
        Leg
    }

    public BodyParts bodyPart;

    private Dummy dummy;
    void Awake()
    {
        dummy = GetComponentInParent<Dummy>();
    }
    public void TakeDamage(float amount)
    {
        //var multiplier = bodyPart switch
        //{
        //    BodyParts.Head => 2f,
        //    BodyParts.Body => 1f,
        //    BodyParts.Arm => .5f,
        //    BodyParts.Leg => .5f,
        //};

        float multiplier = 0;

        switch (bodyPart)
        {
            case BodyParts.Head: multiplier = 2f; break;
            case BodyParts.Body: multiplier = 1f; break;
            case BodyParts.Arm: multiplier = 0.5f; break;
            case BodyParts.Leg: multiplier = 0.75f; break;
        }

        dummy.TakeDamage(amount * multiplier);
    }

}
