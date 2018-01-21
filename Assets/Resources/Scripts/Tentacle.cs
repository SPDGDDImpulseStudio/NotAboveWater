using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    float attackTimer = 4f, currentTimer = 0f;
    //Control from outside
    public bool rangeAttack = false;
    
    Animator anim;
    public GameObject stonePrefab;
    public GameObject tipAKAwhereToShootAt;
    CirclePosUpdate circle;
    
    public bool selectOne = false;

    bool attack = false;

    public UnityEngine.UI.Text debugText;

    Vector3 offSet;

    void Start()
    {
        anim = GetComponent<Animator>();
        currentTimer += Random.Range(-1.2f, 1.2f);
        offSet += Random.Range(-15, 15) * transform.right;
        StartCoroutine(DebugUIUpdate());
    }

    IEnumerator DebugUIUpdate()
    {
        if (!debugText) yield break;
        while (true)
        {
            AnimatorClipInfo[] newA = anim.GetCurrentAnimatorClipInfo(0);
            debugText.text = newA[0].clip.ToString();
            yield return null;
        }
    }

    void Update()
    {
        if (currentTimer < attackTimer) currentTimer += Time.deltaTime;
        if (!selectOne)
        {
            DamagedToFalse();            return;
        }

        AnimatorClipInfo[] newA = anim.GetCurrentAnimatorClipInfo(0);

        if (newA.Length == 0) return;

        if (newA[0].clip.name == "DamagedNHide") { DamagedToFalse(); NullifyCircle(); }

        if (currentTimer > attackTimer)
        {
            if (newA[0].clip.name == "Idle")
            {
                if (rangeAttack)
                {
                    StartCoroutine(WaitTillThrow());
                }
                else
                {
                    StartCoroutine(Charge());
                }
                currentTimer = 0f;
                attackTimer = Random.Range(3f, 6f);
            }
        }

        Vector3 temp = new Vector3(
            Player.Instance.transform.position.x + offSet.x,
            this.transform.position.y,
            Player.Instance.transform.position.z + offSet.z);

        this.transform.LookAt(temp);
        this.transform.localEulerAngles = this.transform.localEulerAngles + new Vector3(0, 180f, 0);
        //This is added cause the animation/model is reversed
        //Thanks to JR's impact i feel like changing these to coroutines
    }

    public void OnHit()
    {
        anim.SetBool("DAMAGED", true);
    }
    public void DamagedToFalse()
    {
        anim.SetBool("DAMAGED", false);
    }

   IEnumerator Charge()
    {
        if (attack) yield break;
        attack = true;
        anim.Play("Charge", -1);
        GetCircle();
        while (true)
        {
            AnimatorClipInfo[] newA = anim.GetCurrentAnimatorClipInfo(0);
            
            if (newA[0].clip.name == "DamagedNHide")
            {
                DamagedToFalse();
                break;
            }
            else if (newA[0].clip.name == "Strike")
            {
                NullifyCircle();
                ChargeAttack();
                break;
            }

            yield return null;
        }
        NullifyCircle();
        attack = false;
    }
    void ChargeAttack()
    {
        DamagedToFalse();
        Debug.Log("Charge");
    }

    void GetCircle()
    {
        circle = PoolManager.Instance.DeqCircle();
        circle.Init_(tipAKAwhereToShootAt);
    }
    void NullifyCircle()
    {
        if (circle == null) return;
        circle.TurnOff();
        PoolManager.Instance.EnqCircle(circle);
        circle = null;
    }
    IEnumerator WaitTillThrow()
    {
        if (attack) yield break;
        attack = true;
        anim.Play("Dig", -1);
        Time.timeScale = 0.6f;
        GetCircle();
        while (true)
        {
            AnimatorClipInfo[] newA = anim.GetCurrentAnimatorClipInfo(0);
            if (newA[0].clip.name == "Throw")
            {
                StoneAttack();
                break;
            }
            else if (newA[0].clip.name == "DamagedNHide")
            {
                DamagedToFalse();
                break;
            }
            yield return null;
        }
        NullifyCircle();
        attack = false;
        Time.timeScale = 1.0f;
    }
    void StoneAttack()
    {
        GameObject bul = //RepositionStone(PoolManager.Instance.DeqBullet(), tip.transform.position + 3 * Vector3.forward, Quaternion.identity).gameObject ; 
        Instantiate(stonePrefab, tipAKAwhereToShootAt.transform.position + 3* Vector3.forward, Quaternion.identity);

        Vector3 dir = (Player.Instance.transform.position - tipAKAwhereToShootAt.transform.position) + Random.Range(3,7)*transform.up - Random.Range(25,30)*transform.right;
        bul.GetComponent<Rigidbody>().velocity = dir * 15 * Time.deltaTime;

        DamagedToFalse();
    }


    BulletScript RepositionStone(BulletScript x ,Vector3 pos, Quaternion quat)
    {
        Debug.Log(x.gameObject.name);
        if (x == null) return null;
        x.transform.position = pos;
        x.transform.rotation = quat;
        return x;
    }


}
