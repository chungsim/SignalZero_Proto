using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBaseState : MonoBehaviour
{
    public abstract void OnStateEnter();
    public abstract void OnStateUpdate();
    public abstract void OnStateExit();
}
