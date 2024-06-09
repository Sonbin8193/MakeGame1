using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPatrolState : BIState
{
    float randomTime;
    float timer;
    public void OnEnter(Boss boss)
    {
        timer = 0;
        randomTime = Random.Range(3f, 6f);
    }

    public void OnExecute(Boss boss)
    {
        timer += Time.deltaTime;

        if (boss.Target != null)
        {
            boss.ChangeDirection(boss.Target.transform.position.x > boss.transform.position.x);

            if (boss.IsTargetInRange())
            {
                boss.ChangeState(new BAttackState());
            }
            else
            {
                boss.Moving();
            }

        }
        else
        {
            if (timer < randomTime)
            {
                boss.Moving();
            }
            else
            {
                boss.ChangeState(new BIdleState());
            }
        }
    }

    public void OnExit(Boss boss)
    {
        
    }
}
