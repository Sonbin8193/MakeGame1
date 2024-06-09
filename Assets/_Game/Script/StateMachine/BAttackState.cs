using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BAttackState : BIState
{
    float timer;
    public void OnEnter(Boss boss)
    {
        if (boss.Target != null)
        {
            boss.ChangeDirection(boss.Target.transform.position.x > boss.transform.position.x);
            boss.StopMoving();
            boss.Attack();

        }
        timer = 0;
    }

    public void OnExecute(Boss boss)
    {
        timer += Time.deltaTime;

        if (timer >= 1.5f)
        {
            boss.ChangeState(new BPatrolState());
        }
    }

    public void OnExit(Boss boss)
    {

    }
}
