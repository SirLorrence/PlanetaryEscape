using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Action/Attack")]
public class AttackState : AIState
{
    private static readonly int Attacking = Animator.StringToHash("AttackAnim");
    private float time = 0;
    float fireElapsedTime = 0;


    public override void DoState(EnemyController controller) => Attack(controller);

    void Attack(EnemyController controller)
    {
        controller.anim.SetBool(Attacking, controller.attackInRange);

        var temp = new Vector3(controller.targetTransform.position.x, controller.transform.position.y,
            controller.targetTransform.position.z);

        controller.transform.LookAt(temp);

        if (Vector3.Distance(controller.targetTransform.position, controller.transform.position) < controller.enemyStats.attackRangeRadius)
        {
            if (fireElapsedTime >= controller.enemyStats.rateOfFire)
            {
                fireElapsedTime = 0;
                Shoot(controller);
            }
            //else Debug.Log(time);
        }

        fireElapsedTime += Time.deltaTime;
    }

    void Shoot(EnemyController controller)
    {
        GameObject go = GameManager.Instance.GetEnemyBullet();
        if (go != null)
        {
            go.transform.position = controller.GunBarrel.transform.position;
            go.transform.rotation = controller.GunBarrel.transform.rotation;
            go.GetComponent<Bullet>().OnStart(controller.enemyStats.bulletSpeed, controller.enemyStats.damage);
            go.SetActive(true);
        }
    }  

}