using UnityEngine;
using System.Collections;

public class ShipDock : MonoBehaviour {

    public GameObject shipPrefab;
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
            ship.transform.position = this.transform.position;
            spawned++;
        }
    }
}
