using UnityEngine;
using System.Collections;

public class Flock : MonoBehaviour {

    public FlockManager controller;

    public float speed = 10.0f;
    public float rotationSpeed = 5.0f;

    Vector3 averageHeading;
    Vector3 averagePosition;

    public float neighbourThreshold = 16.0f;

    bool turning = false;

	// Use this for initialization
	void Start () {
        if(Vector3.Distance(transform.position, controller.goalPos) >= controller.targetArea)
        {
            turning = true;
        }else
        {
            turning = false;
        }
        if (turning)
        {
            Vector3 direction = controller.goalPos - transform.position;
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                rotationSpeed * Time.deltaTime);
            speed = Random.Range(speed - 4, speed + 4);
        }else
        {
            ApplyRules();
        }
        
        transform.Translate(controller.goalPos);    
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(0, 0, Time.deltaTime * speed);
        ApplyRules();//Basic 3 rule flocking
	}

    void ApplyRules()
    {
        GameObject[] otherBoids = controller.boids;

        Vector3 centre = Vector3.zero;
        Vector3 avoid = Vector3.zero;

        float gSpeed = 0.1f;

        Vector3 goal = controller.goalPos;
        float distance;

        int groupSize = 0;

        foreach(var boid in otherBoids)
        {
            if(boid != this.gameObject)
            {
                distance = Vector3.Distance(boid.transform.position, this.transform.position);
                if(distance <= neighbourThreshold)
                {
                    centre = boid.transform.position;
                    groupSize++;

                    if(distance < 50)//May need changing
                    {
                        avoid = avoid + (this.transform.position - boid.transform.position);
                    }

                    Flock newFlock = boid.GetComponent<Flock>();
                    gSpeed = gSpeed + newFlock.speed;
                }
            }
        }

        if(groupSize > 0)
        {
            centre = centre / groupSize + (goal - this.transform.position);
            speed = gSpeed / groupSize;

            Vector3 direction = (centre + avoid) - transform.position;

            if(direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation, 
                    Quaternion.LookRotation(direction),
                    rotationSpeed * Time.deltaTime
                    );
            }
        }


    }
}
