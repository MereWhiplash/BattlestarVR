using UnityEngine;
using System.Collections;

public class ShipDock : MonoBehaviour {

    public GameObject shipPrefab;
    public float spawnRadius;
    public int numOfShips = 10;
    int spawned;

	// Use this for initialization
	void Start () {
        InvokeRepeating("SpawnShip", 1f, 1f);
	}

    void SpawnShip()
    {
        if (spawned < numOfShips)
        {
            GameObject ship = Instantiate(shipPrefab);
            ship.transform.position = new Vector3(Random.insideUnitSphere.x * spawnRadius, Random.insideUnitSphere.y * spawnRadius, Random.insideUnitSphere.z * spawnRadius) + transform.position;
            spawned++;
        }
    }
}
