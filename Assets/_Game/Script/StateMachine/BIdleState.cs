using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BIdleState : BIState
{
    float randomTime;
    float timer;
    public void OnEnter(Boss boss)
    {
        boss.StopMoving();
        timer = 0;
        randomTime = Random.Range(2f, 4f);
    }

    public void OnExecute(Boss boss)
    {
        timer += Time.deltaTime;

        if (timer > randomTime)
        {
            boss.ChangeState(new BPatrolState());
        }
    }

    public void OnExit(Boss boss)
    {

    }
}
