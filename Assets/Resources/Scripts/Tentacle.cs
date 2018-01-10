using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    float attackTimer = 3f, currentTimer = 0f;
    void TentacleAttack()
    {

    }
    public enum TentacleState
    {
        //Animation Control
        IDLE,
        PREATTACK,
        WINDUP,
        ATTACK,
        POSTATTACK,
    }

    public bool Trigger = false;
    TentacleState currentState = TentacleState.IDLE;

    void Update()
    {
        if (currentTimer < attackTimer) currentTimer += Time.deltaTime;

        if (Trigger)
        {
            if(currentTimer> attackTimer)
            {
                currentState = TentacleState.PREATTACK;
               
            }
        }

        if (currentState == TentacleState.IDLE) return;
        Vector3 temp = new Vector3(
            Player.Instance.transform.position.x,
            0,
            Player.Instance.transform.position.z);


        this.transform.LookAt(temp);
    }

    void LateUpdate()
    {
        if (currentState == TentacleState.IDLE) return;
        this.transform.localEulerAngles = new Vector3(0, this.transform.localEulerAngles.y, 0);

    }

    public void OnHit()
    {
        switch (currentState)
        {
            case TentacleState.PREATTACK:
                
                break;
            case TentacleState.WINDUP:

                break;
            case TentacleState.POSTATTACK:

                break;
        }
    }

    public void SwitchState(TentacleState state)
    {
        currentState = state;
    }
    
}
