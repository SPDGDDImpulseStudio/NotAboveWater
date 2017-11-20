using UnityEngine;

public class Gun : MonoBehaviour {

    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCam;

    public GameObject VFX_Hit;
    GameObject targetHit;
    public static Vector3 pointHit;

    // Update is called once per frame
    void Update () {
		
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
	}

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            targetHit = hit.transform.gameObject;
            pointHit = hit.point;

            Instantiate(VFX_Hit, pointHit, targetHit.transform.rotation);

            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }
}

