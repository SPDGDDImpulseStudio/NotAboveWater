using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

public class Swimming : MonoBehaviour
{
    public List<WaterSort> WaterSorts;
    public int currentWaterSort;
    public FirstPersonController FPC;
    public Collider watercol;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UnderWater();
    }

    void OnTriggerEnter(Collider hit)
    {
        watercol = hit;
        if (hit.transform.tag == "Water" && currentWaterSort != 1)
        {
            setSortWater(1);
            currentWaterSort = 1;
        }
    }

    void OnTriggerExit(Collider hit)
    {
        if (currentWaterSort != 0)
        {
            setSortWater(0);
            currentWaterSort = 0;
        }
    }

    public void setSortWater(int i)
    {
        FPC.m_WalkSpeed = WaterSorts[i].WalkSpeed;
        FPC.m_RunSpeed = WaterSorts[i].RunSpeed;
        FPC.m_GravityMultiplier = WaterSorts[i].GravityMultiplier;
        FPC.m_StickToGroundForce = WaterSorts[i].StickToGroundForce;
        FPC.m_JumpSpeed = WaterSorts[i].JumpSpeed;
        FPC.IsSwimming = WaterSorts[i].IsSwimming;
        RenderSettings.fogColor = WaterSorts[i].fogColor;
        RenderSettings.fogDensity = WaterSorts[i].fogDensity;
        RenderSettings.fogMode = WaterSorts[i].fogMode;
    }

    public void UnderWater()
    {
        if (currentWaterSort != 0)
        {
            Vector3 postionFloat = watercol.transform.position + watercol.transform.GetComponent<BoxCollider>().size + watercol.transform.GetComponent<BoxCollider>().center - new Vector3(0, 0, 0);
            print(postionFloat);
            print(this.transform.parent.position.y);
            if (this.transform.parent.position.y <- postionFloat.y)
                RenderSettings.fog = WaterSorts[currentWaterSort].fog;
            else
                RenderSettings.fog = false;
        }
    }
}

[System.Serializable]
public class WaterSort
{
    public string name;
    public float WalkSpeed;
    public float RunSpeed;
    public float GravityMultiplier = 2;
    public float StickToGroundForce = 10;
    public float JumpSpeed;
    public bool IsSwimming;
    public bool fog;
    public Color fogColor;
    public float fogDensity;
    public FogMode fogMode;
}
