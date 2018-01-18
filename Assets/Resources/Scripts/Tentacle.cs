using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    float attackTimer = 4f, currentTimer = 0f;
    public bool Trigger = false;
    TentacleState currentState = TentacleState.IDLE;
    Animator anim;
    public GameObject stonePrefab;
    public GameObject tip;
    CirclePosUpdate circle;
    public enum TentacleState
    {
        IDLE,
        PREATTACK,
        WINDUP,
        ATTACK,
        POSTATTACK,
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        currentTimer += Random.Range(-1.2f, 1.2f);
        offSet += Random.Range(-15, 15) * transform.right;
        StartCoroutine(DebugUIUpdate());
    }
    public UnityEngine.UI.Text text;
    IEnumerator DebugUIUpdate()
    {
        while (true)
        {
            AnimatorClipInfo[] newA = anim.GetCurrentAnimatorClipInfo(0);
            text.text = newA[0].clip.ToString();

            yield return null;
        }
    }
    void Update()
    {
        if (currentTimer < attackTimer) currentTimer += Time.deltaTime;
        if (!selectOne) {
            anim.SetBool("DAMAGED", false);
            return;
        }

        AnimatorClipInfo[] newA = anim.GetCurrentAnimatorClipInfo(0);
        
        if (newA.Length == 0)   return;

        if (newA[0].clip.name == "DamagedNHide") anim.SetBool("DAMAGED", false);
        
        if (currentTimer > attackTimer)
        {
            if (newA[0].clip.name == "Idle")
            {
                if (Trigger)
                {
                    anim.Play("Dig", -1);
                    StartCoroutine(WaitTillThrow());
                }
                else
                {
                    anim.Play("Charge", -1);
                    StartCoroutine(ChargeAttack());
                }
                //Vector2 offset1 = new Vector2(15f, 15f),
                //        offset2 = new Vector2(15f, 15f);

                //List<Vector2> offset = new List<Vector2>() { offset1, offset2 };

                //CircleManager.Instance.SpawnButtons(2, offset, offset, tip);
                currentTimer = 0f;
                attackTimer = Random.Range(3f, 6f);
            }
        }

        Vector3 temp = new Vector3(
            Player.Instance.transform.position.x + offSet.x,
            this.transform.position.y,
            Player.Instance.transform.position.z + offSet.z);
        this.transform.LookAt(temp);
        this.transform.localEulerAngles = this.transform.localEulerAngles + new Vector3( 0, 180f, 0);
    }

    public bool selectOne = false;

    bool attack = false;

    IEnumerator ChargeAttack()
    {
        if (attack) yield break;
        attack = true;
        GetCircle();
        while (true)
        {
            AnimatorClipInfo[] newA = anim.GetCurrentAnimatorClipInfo(0);
            if (newA[0].clip.name == "DamagedNHide")
            {
                anim.SetBool("DAMAGED", false);
                break;
            }
            else if (newA[0].clip.name == "Strike")
            {
                Charge();
                break;
            }

            yield return null;
        }
        NullifyCircle();
        attack = false;
    }
    void GetCircle()
    {
        circle = PoolManager.Instance.DeqCircle();
        circle.Init(tip);
    }
    void NullifyCircle()
    {
        circle.TurnOff();
        Debug.Log(circle.name + " IS GONE");
        PoolManager.Instance.EnqCircle(circle);
        circle = null;
    }
    IEnumerator WaitTillThrow()
    {
        if (attack) yield break;
        attack = true;
        GetCircle();
       while (true)
        {
            AnimatorClipInfo[] newA = anim.GetCurrentAnimatorClipInfo(0);
            if (newA[0].clip.name == "Throw")
            {
                ThrowStone();
                break;
            }
            else if (newA[0].clip.name == "DamagedNHide")
            {
                anim.SetBool("DAMAGED", false);
                break;
            }
            yield return null;
        }
        NullifyCircle();
        attack = false;
    }
    void ThrowStone()
    {
        GameObject bul = //RepositionStone(PoolManager.Instance.DeqBullet(), tip.transform.position + 3 * Vector3.forward, Quaternion.identity).gameObject ; 
        Instantiate(stonePrefab, tip.transform.position + 3* Vector3.forward, Quaternion.identity);

        Vector3 dir = (Player.Instance.transform.position - tip.transform.position) + Random.Range(3,7)*transform.up - Random.Range(25,30)*transform.right;
        bul.GetComponent<Rigidbody>().velocity = dir * 120 * Time.deltaTime;
        
    }

    Vector3 offSet;
    void Charge()
    {
        Debug.Log("Charge");
    }

    BulletScript RepositionStone(BulletScript x ,Vector3 pos, Quaternion quat)
    {
        Debug.Log(x.gameObject.name);
        if (x == null) return null;
        x.transform.position = pos;
        x.transform.rotation = quat;
        return x;
    }

    public void OnHit()
    {
        anim.SetBool("DAMAGED", true);
        
        //    AnimatorClipInfo[] newA = anim.GetCurrentAnimatorClipInfo(0);
        //    Debug.Log("1 " + anim.GetBool("DAMAGED") + " " + newA[0].clip.name);
        //    //PostHit();
        //    if (newA.Length == 0 || newA[0].clip.name == "Throw") return;
        //    else
        //    {

        //    }

        //    Debug.Log(newA[0].clip.name);
        //}
    }

    public void SwitchState(TentacleState state)
    {
        currentState = state;
    }
}
