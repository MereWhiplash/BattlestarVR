using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour {

    public GameObject[] Asteroids;

    public float spawnArea;
    public int asteroidCount;

    // Use this for initialization
    void Start () {
        System.Random r = new System.Random();
	    for(int i = 0; i <= asteroidCount; i++)
        {
            int index = r.Next(0, 8);
            Instantiate(Asteroids[index], Random.insideUnitSphere * spawnArea, Quaternion.identity);
        }
	}
	

}
