using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : Healable
{
    #region fields

    public GameManager.EnemyTypes enemyType;

    public State currentState;
    public State remainState;
    public GameObject GunBarrel;
    public EnemyInformation enemyStats;
    public float stateTimeElapsed;


    // public float detectionRadius, attackRangeRadius, fieldOfView = 45, patrolAreaRatio;
    //public LayerMask playerArea;
    public int playerArea = 1 << 8;

    public List<GameObject> partolPoints;
    public int currentPoint;

    // public Material materialColor;

    public Transform targetTransform;
    protected internal NavMeshAgent agent;

    protected internal bool playerInRange;
    protected internal bool attackInRange;


    protected internal Vector3 searchTemp;
    protected internal Vector3 lastKownPos;

    protected internal RaycastHit hitInfo;

    protected internal Animator anim;

    #endregion

    #region Properties

    public int CurrentPoint
    {
        get => currentPoint;
        set => currentPoint = value;
    }

    public override int Health
    {
        get => enemyStats.health;
        set => enemyStats.health = value;
    }
    
    public int PlayerAreaMask
    {
        get => playerArea;
    }
    public int Damage
    {
        get => enemyStats.damage;
    }

    #endregion


    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        targetTransform = GameObject.FindWithTag("Player").transform;
        partolPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("PPoint"));
    }

    private void Start() => agent.speed = enemyStats.movementSpeed;


    void Update()
    {
        currentState.UpdateState(this);
        //CheckSphere is a bool operation at gets objects in a layer
        playerInRange = Physics.CheckSphere(transform.position, enemyStats.detectionRadius, playerArea);

        if (!IsAlive())
        {
            GameManager.Instance.SetEnemy(gameObject, true);
            Health = 10;
        }



        //Collider[] colliders = Physics.OverlapSphere(transform.position, enemyStats.detectionRadius);

        //foreach (var collider in colliders)
        //{
        //    if (collider.CompareTag("Player"))
        //    {
        //        Debug.Log("playerinrange");
        //        playerInRange = true;
        //        break;
        //    }
        //    playerInRange = false;
        //}
        attackInRange = Physics.CheckSphere(transform.position, enemyStats.attackRangeRadius, playerArea);
    }


    public void TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            currentState = nextState;
            OnExitState();
        }
    }

    public bool CheckIfCountDownElapsed(float duration) //Works like a Invoke or WaitForSeconds
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    private void OnExitState()
    {
        stateTimeElapsed = 0;
    }


    #region Gizmos and Debugging

    private void OnDrawGizmos()
    {
        Gizmos.color = (playerInRange) ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position, enemyStats.detectionRadius);

        Gizmos.color = (attackInRange) ? Color.yellow : Color.white;
        Gizmos.DrawWireSphere(transform.position, enemyStats.attackRangeRadius);


        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(lastKownPos, .5f);
    }

    #endregion
}