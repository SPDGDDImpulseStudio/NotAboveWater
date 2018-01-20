using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFish : MonoBehaviour {


    public float spawnArea;

    public GameObject fishPrefab;
    public List<GameObject> fishes = new List<GameObject>();
    void Start () {
        Init();
	}
	public void Init()
    {
        StartCoroutine(SpawnFishes());
    }

    IEnumerator SpawnFishes()
    {
        float timer = 0f;
        while (true)
        {
            timer += Time.deltaTime;
            if (!spawnFish)  break;
            if(timer> spawnRate)
            {
                timer = 0f;
                if (fishes.Count - 1 == maxNumberOfFish)
                    continue;

                Vector3 pos = this.transform.position+Random.insideUnitSphere * spawnArea;
                GameObject newFish = Instantiate(fishPrefab, pos, Quaternion.identity);
                fishes.Add(newFish);
            }

            
            yield return null;
        }
    }

    public bool spawnFish = true;
    public int maxNumberOfFish = 5;
    public float spawnRate = 2.5f;
#if UNITY_EDITOR


    public bool showEditor;
    public Color editorColor = Color.black;
    void OnDrawGizmos()
    {
        if (!showEditor) return;
        Gizmos.color = editorColor;
        Gizmos.DrawWireSphere(this.transform.position, spawnArea);
    } 
#endif
}
