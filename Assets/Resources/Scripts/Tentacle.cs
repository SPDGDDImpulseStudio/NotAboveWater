using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    float attackTimer = 3f, currentTimer = 0f;
    public bool Trigger = false;
    TentacleState currentState = TentacleState.IDLE;
    Animator anim;

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
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        /*
         Animclip name

                Idle

            (Attack)
                Dig
                Digging
                Throw
         
         */

        if (currentTimer < attackTimer) currentTimer += Time.deltaTime;

        if (Trigger)
        {

            if(currentTimer> attackTimer)
            {
                currentState = TentacleState.PREATTACK;
            }

        }

        
        //if (currentState == TentacleState.IDLE) return;
        Vector3 temp = new Vector3(
            Player.Instance.transform.position.x,
            this.transform.position.y,
            Player.Instance.transform.position.z);
        this.transform.LookAt(temp);
    }


    public void OnHit()
    {
        anim.SetBool("DAMAGED", true);
        AnimatorClipInfo[] newA = anim.GetCurrentAnimatorClipInfo(0);
        if (newA.Length == 0 || newA[0].clip.name == "Throw") return;
        else
        {

        }
        Debug.Log(newA[0].clip.name)
    ;
    }

    public void SwitchState(TentacleState state)
    {
        currentState = state;
    }
    
}
