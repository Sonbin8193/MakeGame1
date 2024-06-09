using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BIState
{
    void OnEnter(Boss boss);
    void OnExit(Boss boss);
    void OnExecute(Boss boss);
}
