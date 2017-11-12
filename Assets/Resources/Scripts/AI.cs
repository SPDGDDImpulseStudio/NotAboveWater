using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AI : MonoBehaviour {

    public float currHealth, maxHealth, speed,damage;
    int currInt = 0;
    public GameObject currWaypoint;
    public NavMeshAgent nav;
    public UnityEngine.UI.Slider slider;

    public List<GameObject> waypoints = new List<GameObject>();

    [Header("[Draw Waypoints]")]
    public Color colorOfLines;

    [Range(0.1f, 2.0f)]
    public float sizeOfDrawnSphere;

    public static AI Instance
    {
        get
        {
            return _instance;
        }
    }

    public Animator anim;

    static AI _instance;

    public void SliderTo(bool x )
    {
        slider.gameObject.SetActive(x);
    }

	// Use this for initialization
	void Start () {
        _instance = this;

        currHealth = maxHealth;

        NavMeshHit closestHit;

        if (NavMesh.SamplePosition(gameObject.transform.position, out closestHit, 500f, NavMesh.AllAreas))
            gameObject.transform.position = closestHit.position;
        else
            Debug.LogError("Could not find position on NavMesh!");
        nav = gameObject.AddComponent<NavMeshAgent>();
        
        //nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        UpdateWaypoint();
        nav.speed = speed;

    }

    public void Update()
    {
        slider.value = currHealth / maxHealth;
        //Debug.Log(nav.destination);
    }
	
    //Navmesh not working fuck this shit im out
    public void UpdateWaypoint()
    {
       
        currWaypoint = waypoints[currInt];
        Debug.Log(nav.destination + " | " + currWaypoint + " | " + currWaypoint.transform.position);

        nav.destination = currWaypoint.transform.position;///new Vector3(currWaypoint.transform.position.x, this.transform.position.y, currWaypoint.transform.position.z);
                                                          ///nav.SetDestination(currWaypoint.transform.position);

        Debug.Log(nav.destination +" | " + currWaypoint + " | " +currWaypoint.transform.position);
        currInt++;

    }

    public void UpdateWaypoint(int x )
    {
        currWaypoint = waypoints[x];
    }
    public Vector3 GetPos(int x)
    {
        return waypoints[x].transform.position;
    }
    //For some fucking reason this is not working
#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            Gizmos.color = colorOfLines;

            Gizmos.DrawWireSphere(waypoints[i].transform.position, sizeOfDrawnSphere);
            if (i != waypoints.Count && i != waypoints.Count - 1)

                Gizmos.DrawLine(waypoints[i].transform.position, waypoints[i + 1].transform.position);

        }
    }
#endif 
}

