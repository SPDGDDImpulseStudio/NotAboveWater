using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    float attackTimer = 4f, currentTimer = 0f;
    //Control from outside
    public bool rangeAttack = false, nearAttack = false;

    Animator anim;
    public GameObject stonePrefab, circlePrefab;
    public GameObject tipAKAwhereToShootAt;
    CirclePosUpdate circle;

    public bool selectOne = false;

    bool attack = false;

    public UnityEngine.UI.Text debugText;

    Vector3 offSet;
    List<string> animClips = new List<string>();


    public List<AudioClip> hitSFXClips = new List<AudioClip>();

    public List<AudioClip> gotHitSFX = new List<AudioClip>();

    public AudioSource aSource;

    void Start()
    {
        if(!aSource)
        aSource = GetComponent<AudioSource>();
        this.transform.localEulerAngles = new Vector3(0, Random.Range(0f, 180f), 0f);
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
            if (!canvases[i].GetComponent<SceneChanger>())
            {
                go.transform.SetParent(canvases[i].transform);
                break;
            }
        }
        PoolManager.RequestCreatePool(circlePrefab, 10, go.transform);

        GameObject go2 = new GameObject();
        go2.name = "Stones Parent";
        PoolManager.RequestCreatePool(stonePrefab, 10, go2.transform);
        //Debug.Log(circlePrefab.GetInstanceID());
        StartCoroutine(DebugUIUpdate());
        //StartCoroutine(AIUpdate());
    }

    IEnumerator DebugUIUpdate()
    {
        if (!debugText || !debugText.gameObject.activeInHierarchy) yield break;
        AnimatorClipInfo[] newA;
        while (true)
        {
            newA = anim.GetCurrentAnimatorClipInfo(0);
            debugText.text = this.gameObject.name + " | " + newA[0].clip.name + " | " + anim.GetBool("RELEASED");
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
            DamagedToFalse(); return;
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
        if (!nearAttack && !rangeAttack) return;
        if (currentTimer > attackTimer)
        {
            if (newA[0].clip.name == animClips[0] || newA[0].clip.name == animClips[1])
            {
                SetFalse();
                if (rangeAttack)
                {
                    anim.Play(animClips[5], -1);
                    StartCoroutine(WaitTillThrow());
                }
                else if (nearAttack)
                {
                    anim.Play((anim.GetBool("LOOKUP") ? animClips[2] : animClips[3]), -1);
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
        if (aSource.isPlaying) return;
        int x = Random.Range(0, gotHitSFX.Count);
        aSource.clip = gotHitSFX[x];
        aSource.Play();
    }
    public void DamagedToFalse()
    {
        anim.SetBool("DAMAGED", false);
    }

    IEnumerator Charge()
    {
        if (attack) yield break;
        attack = true;
        GetCircle(tipAKAwhereToShootAt);
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
                else if (!nearAttack && (newA[0].clip.name == animClips[2] || newA[0].clip.name == animClips[3]))
                {
                    anim.SetBool("RELEASED", true);
                    break;
                }

            }

            yield return null;
        }
        NullifyCircle();
        attack = false;

    }
    void SetFalse()
    {
        if (anim.GetBool("RELEASED")) //Debug.Log("fas");
            anim.SetBool("RELEASED", false);
    }

    IEnumerator ChargeAttack()
    {
        DamagedToFalse();
        AnimatorClipInfo[] newA;
        Vector3 temp = Player.Instance.transform.localPosition;
        //temp += new Vector3(0, 180f, 0); 
        //Vector3 newPos = Camera.main.WorldToScreenPoint(tipAKAwhereToShootAt.transform.position);
        //Debug.Log(newPos);
        if (!aSource.isPlaying)
        {
            int x = Random.Range(0, hitSFXClips.Count);
            aSource.clip = hitSFXClips[x];
            aSource.Play();
        }
        Player.Instance.ShakeCam(tipAKAwhereToShootAt.transform.position);
        Debug.LogError("AJA");

        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        this.transform.LookAt(temp);

        this.transform.localEulerAngles = this.transform.localEulerAngles + new Vector3(Random.Range(1f, 5f), 180f, 0);
        sw.Start();
        while (true)
        {
            newA = anim.GetCurrentAnimatorClipInfo(0);

            if (newA[0].clip.name == "Attack_Up_After") break;

            yield return null;
        }
        sw.Stop();
        //Debug.Log(sw.ElapsedMilliseconds);

    }

    void GetCircle(GameObject x)
    {
        circle = PoolManager.Instance.ReturnGOFromList(circlePrefab).GetComponent<CirclePosUpdate>();
        circle.Init_(x);
    }
    void NullifyCircle()
    {
        if (circle == null) return;
        circle.TurnOff();
        circle = null;
    }

    IEnumerator WaitTillThrow()
    {
        if (attack) yield break;
        attack = true;
        AnimatorClipInfo[] newA;
       
        //Bul.transform.SetParent(tipAKAwhereToShootAt.transform);
        //Bul.transform.localPosition = new Vector3(0, 0, 0);
        while (true)
        {
            newA = anim.GetCurrentAnimatorClipInfo(0);
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
        //Time.timeScale = 1.0f;
    }
    void StoneAttack()
    {
        GameObject Bul = PoolManager.Instance.ReturnGOFromList(stonePrefab);
        Bul.GetComponent<PoolObject>().Init();
        BulletScript b = RepositionStone(Bul.GetComponent<BulletScript>(), tipAKAwhereToShootAt.transform.position, Quaternion.identity);
        // Instantiate(stonePrefab, tipAKAwhereToShootAt.transform.position + 3* Vector3.forward, Quaternion.identity);
        //GetCircle(Bul);
        b.Init_(PoolManager.Instance.ReturnGOFromList(circlePrefab).GetComponent<CirclePosUpdate>(), Player.Instance.transform);

        Vector3 dir = (Player.Instance.transform.position - tipAKAwhereToShootAt.transform.position) + Random.Range(3,7)*transform.up - Random.Range(25,30)*transform.right;
        //Bul.GetComponent<Rigidbody>().velocity = dir * 30    * Time.deltaTime;

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
