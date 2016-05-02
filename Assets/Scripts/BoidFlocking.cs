using UnityEngine;
using System.Collections;

public class BoidFlocking : MonoBehaviour {

    GameObject controller;
    bool init = false;
    float minVelocity;
    float maxVelocity;
    float randomness;
    GameObject target;


	// Use this for initialization
	void Start () {
        StartCoroutine("BoidSteering");
	}

    IEnumerator BoidSteering()
    {
        while (true)
        {
            if (init)
            {
                GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity + Calc() * Time.deltaTime;

                float speed = GetComponent<Rigidbody>().velocity.magnitude;
                if(speed > maxVelocity)
                {
                    GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * maxVelocity;
                }else if(speed < minVelocity)
                {
                    GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * minVelocity;
                }
            }
            float waitTime = Random.Range(0.3f, 0.5f);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private Vector3 Calc()
    {
        Vector3 randomize = new Vector3((Random.value * 2) - 1, (Random.value * 2) - 1, (Random.value * 2) - 1);

        randomize.Normalize();
        BoidController boidController = controller.GetComponent<BoidController>();
        Vector3 flockCentroid = boidController.centroid;
        Vector3 flockVelocity = boidController.velocity;
        Vector3 follow = target.transform.localPosition;

        flockCentroid = flockCentroid - transform.localPosition;
        flockVelocity = flockVelocity - GetComponent<Rigidbody>().velocity;
        follow = follow - transform.localPosition;

        return (flockCentroid - flockVelocity + (follow * 2) + (randomize * randomness));
    }

    public void SetController (GameObject theController)
    {
        controller = theController;
        BoidController boidController = controller.GetComponent<BoidController>();
        minVelocity = boidController.minSpeed;
        maxVelocity = boidController.maxSpeed;
        randomness = boidController.randomness;
        target = boidController.target;
        init = true;
    }

	
}
