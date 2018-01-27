using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    float attackTimer = 4f, currentTimer = 0f;
    //Control from outside
    public bool rangeAttack = false;
    
    Animator anim;
    public GameObject stonePrefab, circlePrefab;
    public GameObject tipAKAwhereToShootAt;
    CirclePosUpdate circle;
    
    public bool selectOne = false;

    bool attack = false;

    public UnityEngine.UI.Text debugText;

    Vector3 offSet;
    List<string> animClips = new List<string>();
    public enum ANIM
    {
        
    }
    void Start()
    {
        animClips = new List<string>()
        {
            "Idle_Lookup",
            "Idle_Lookdown",

            "Attack_Up_Charge",
            "Attack_Down_Charge",

            "Damaged",
            "Shoot_Charge",
        };
        anim = GetComponent<Animator>();
        currentTimer += Random.Range(-1.2f, 1.2f);
        offSet += Random.Range(-15, 15) * transform.right;

        GameObject go = new GameObject();
        go.name = "Circle parent";
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        for (int i = 0; i < canvases.Length; i++)
        {
            if (canvases[i].name != "SceneChanger")
            {
                go.transform.SetParent(canvases[i].transform);
                break;
            }
        }
        PoolManager.RequestCreatePool(circlePrefab, 10, go.transform);
        
        GameObject go2 = new GameObject();
        go2.name = "Stones Parent";
        PoolManager.RequestCreatePool(stonePrefab, 10, go2.transform);
        Debug.Log(circlePrefab.GetInstanceID());
        StartCoroutine(DebugUIUpdate());
        //StartCoroutine(AIUpdate());
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

    //IEnumerator AIUpdate()
    //{
    //    AnimatorClipInfo[] animClips;
    //    while (true)
    //    {
    //        animClips = anim.GetCurrentAnimatorClipInfo(0);

    //        if(!selectOne || animClips.Length  == 0)
    //        {


    //        }
    //        else
    //        {

    //        }






    //        yield return new WaitForFixedUpdate();
    //    }
    //}


    void Update()
    {
        if (currentTimer < attackTimer) currentTimer += Time.deltaTime;
        if (!selectOne)
        {
            DamagedToFalse();            return;
        }

        AnimatorClipInfo[] newA = anim.GetCurrentAnimatorClipInfo(0);

        if (newA.Length == 0) return;

        if (newA[0].clip.name == animClips[4]) { DamagedToFalse(); NullifyCircle(); }

        if (newA[0].clip.name != "Attack_Up_After" && 
            newA[0].clip.name != "Attack_Up_Release") {
            Vector3 temp = new Vector3(
              Player.Instance.transform.position.x + offSet.x,
              this.transform.position.y,
              Player.Instance.transform.position.z + offSet.z);

            this.transform.LookAt(temp);
        }
        //this.transform.localEulerAngles = this.transform.localEulerAngles + new Vector3(0, 180f, 0);

        if (currentTimer > attackTimer)
        {
            if (newA[0].clip.name == animClips[0] || newA[0].clip.name == animClips[1])
            {
                if (rangeAttack)
                {
                    anim.Play(animClips[5], -1);
                    StartCoroutine(WaitTillThrow());
                }
                else
                {
                    anim.Play((anim.GetBool("LOOKUP") ? animClips[2]:animClips[3]), -1);
                    StartCoroutine(Charge());
                }
                currentTimer = 0f;
                attackTimer = Random.Range(3f, 6f);
            }
        }
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
        GetCircle();
        while (true)
        {
            AnimatorClipInfo[] newA = anim.GetCurrentAnimatorClipInfo(0);
            if (newA.Length > 0)
            {
                if (newA[0].clip.name == animClips[4])
                {
                    DamagedToFalse();
                    break;
                }
                else if (newA[0].clip.name == "Attack_Up_Release" || newA[0].clip.name == "Attack_Down_Release")
                {
                    NullifyCircle();
                    StartCoroutine(ChargeAttack());
                    break;
                }
            }
           // else Debug.Log("ZEROOOOOOOO");
            

            yield return null;
        }
        NullifyCircle();
        attack = false;
    }

    IEnumerator ChargeAttack()
    {
        DamagedToFalse();
        AnimatorClipInfo[] newA;
        Vector3 temp = Player.Instance.transform.localPosition;
        Debug.LogError("AJA");
        //temp += new Vector3(0, 180f, 0);
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        this.transform.LookAt(temp);

        this.transform.localEulerAngles = this.transform.localEulerAngles + new Vector3(Random.Range(1f,5f), 180f, 0);

        sw.Start();
        while (true)
        {
           newA = anim.GetCurrentAnimatorClipInfo(0);

            if(newA[0].clip.name == "Attack_Up_After") break;
            yield return null;
        }
        sw.Stop();
        Debug.Log(sw.ElapsedMilliseconds);
    }

    void GetCircle()
    {
        //circle = PoolManager.Instance.DeqCircle();
        circle = PoolManager.Instance.ReturnGOFromList(circlePrefab).GetComponent<CirclePosUpdate>();
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
        Time.timeScale = 0.6f;
        GetCircle();
        while (true)
        {
            AnimatorClipInfo[] newA = anim.GetCurrentAnimatorClipInfo(0);
            if (newA.Length > 0)
            {
                if (newA[0].clip.name == "Shoot_Release")
                {
                    StoneAttack();
                    break;
                }
                else if (newA[0].clip.name == animClips[4])
                {
                    DamagedToFalse();
                    break;
                }
            }
          //  else Debug.Log("ZEROOOOOOOO");
           
            yield return null;
        }
        NullifyCircle();
        attack = false;
        Time.timeScale = 1.0f;
    }
    void StoneAttack()
    {
        GameObject x = PoolManager.Instance.ReturnGOFromList(stonePrefab);
        GameObject bul = RepositionStone(x.GetComponent<BulletScript>(), tipAKAwhereToShootAt.transform.position + 3 * Vector3.forward, Quaternion.identity).gameObject ; 
       // Instantiate(stonePrefab, tipAKAwhereToShootAt.transform.position + 3* Vector3.forward, Quaternion.identity);

        Vector3 dir = (Player.Instance.transform.position - tipAKAwhereToShootAt.transform.position) + Random.Range(3,7)*transform.up - Random.Range(25,30)*transform.right;
        bul.GetComponent<Rigidbody>().velocity = dir * 10    * Time.deltaTime;

        DamagedToFalse();
    }

    BulletScript RepositionStone(BulletScript x ,Vector3 pos, Quaternion quat)
    {
        //Debug.Log(x.gameObject.name);
        if (x == null) return null;
        x.transform.position = pos;
        x.transform.rotation = quat;
        return x;
    }
}
