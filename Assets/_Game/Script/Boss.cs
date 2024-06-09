using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss : Character
{
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject hpBottle;
    [SerializeField] private GameObject flashBottle;
    [SerializeField] public GameObject attackArea;
    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;

    private BIState currentState;

    private bool isRight = true;
    public bool isAttack = false;

    private Character target;
    public Character Target => target;
    private void Update()
    {
        if (currentState != null && !IsDead)
        {
            currentState.OnExecute(this);
        }
    }

    public override void OnInit()
    {
        base.OnInit();

        ChangeState(new BIdleState());
        DeActiveAttack();
    }
    protected override void OnDeath()
    {
        ChangeState(null);
        base.OnDeath();
        int random = Random.Range(0,100);
        if (random <90)
        {
            Instantiate(hpBottle, gameObject.transform.position, transform.rotation);
            Instantiate(flashBottle, gameObject.transform.position, transform.rotation);
        }
    }
    public override void OnDespawn()    
    {
        base.OnDespawn();
        Destroy(gameObject);
        Destroy(healthBar.gameObject);
    }
 
    public void ChangeState(BIState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = newState;
        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }
    internal void SetTarget(Character character)
    {
        this.target = character;

        if (IsTargetInRange())
        {
            ChangeState(new BAttackState());
        } else if (!IsTargetInRange())
        {
            Invoke(nameof(Throw), 1f);
        }
        else if (Target != null)
        {
            ChangeState(new BPatrolState());
        }
        else
        {
            ChangeState(new BIdleState());
        }
    }
    public void Moving()
    {
        ChangeAnim("run");
        rb.velocity = transform.right * moveSpeed;
    }
    public void StopMoving()
    {
        ChangeAnim("idle");
        rb.velocity = Vector2.zero;
    }
    public void Attack()
    {
        ChangeAnim("attack");
        ActiveAtack();
        Invoke(nameof(DeActiveAttack), 0.5f);
    }
    public void Throw()
    {
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 5f);
        Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation);
    }

    private void ResetAttack()
    {
        ChangeAnim("idle");
        isAttack = false;
    }
    // public bool IsTargetInRange => Vector2.Distance(target.transform.position,transform.position) <= attackRange;
    public bool IsTargetInRange()
    {
        if (target != null && Vector2.Distance(target.transform.position, transform.position) <= attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    } 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyWall")
        {
            ChangeDirection(!isRight);
        }
    }

    public void ChangeDirection(bool isRight)
    {
        this.isRight = isRight;

        transform.rotation = isRight ? Quaternion.Euler(Vector3.zero):Quaternion.Euler(Vector3.up*180);
    }
    public bool ActiveAtack()
    {
        attackArea.SetActive(true);
        return true;
    }
    public bool DeActiveAttack()
    {
        attackArea.SetActive(false);
        return false;
    }

}

