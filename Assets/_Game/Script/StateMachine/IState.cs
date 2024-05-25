using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void OnEnter(Enemy enemy);
    void OnExit(Enemy enemy);
    void OnExecute(Enemy enemy);
}
