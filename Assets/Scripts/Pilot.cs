using UnityEngine;
using System.Collections.Generic;

public class Pilot : MonoBehaviour {


    public float health;
    public bool isChasing;
    public bool beingChased;
    public string idealTarget;

    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 force;
    public float mass;

    public float maxSpeed = 10.0f;
    public float maxForce = 10.0f;

    public bool seekEnabled;
    public Vector3 seekTarget;

    public bool arriveEnabled;
    public Vector3 arriveTarget;
    public float slowingDistance;

    public bool pursueEnabled;
    public GameObject pursueTarget;
    Vector3 pursueTargetPos;

    public List<Vector3> waypoints;

    public GameObject projectile;
    public Transform shotPos;
    public float shotForce;

    public bool launchEnabled;
    public bool encircleEnabled;
   

    public int waypointcount = 10;
    public int radius = 10;

    int current = 0;


	// Use this for initialization
	void Start () {

        waypoints = new List<Vector3>();

        for(int i = 0; i < 9; i++)
        {
            Vector3 waypoint = new Vector3(Random.insideUnitSphere.x * radius, Random.insideUnitSphere.y * radius, Random.insideUnitSphere.z * radius);
            waypoints.Add(waypoint);
        }

	}
	
	// Update is called once per frame
	void Update () {

        force = Vector3.zero;

        if (Vector3.Distance(transform.position, waypoints[current]) < 0.5f)
        {
            current = (current + 1) % waypoints.Count;
        }

        if (launchEnabled)
        {
            transform.Translate(transform.forward * 1000);
            launchEnabled = false;
        }

        if (seekEnabled) {
			seekTarget = waypoints [current];
			force = Seek (seekTarget);
			transform.LookAt (seekTarget);
		}

        if (arriveEnabled) {
			arriveTarget = waypoints [current];
			force = Arrive (arriveTarget);
		}

        if (pursueEnabled && !isChasing) {
			FindNewTarget ();
			force = Pursue (pursueTarget);
		}

        if (isChasing && pursueEnabled)
        {
            fireAtTarget();
        }

        if (encircleEnabled)
        {
            transform.RotateAround(new Vector3(0, 0, 0), Vector3.forward, 5 * Time.deltaTime);
        }

        force = Vector3.ClampMagnitude(force, maxForce);
        acceleration = force / mass;
        velocity += acceleration * Time.deltaTime;

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        transform.position += velocity * Time.deltaTime;

        if (velocity.magnitude > float.Epsilon)
            transform.forward = velocity;

    }

    void fireAtTarget()
    {
        GameObject.Instantiate(projectile, shotPos.position, shotPos.rotation);     
    }


    void FindNewTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(idealTarget);

        foreach(GameObject t in targets)
        {
            if (!t.GetComponent<Pilot>().beingChased)
            {
                pursueTarget = t;
            }
        }
        pursueTarget = GameObject.FindGameObjectWithTag(idealTarget);//return first target as new target even if engaged
    }

    Vector3 Seek(Vector3 target)
    {
        Vector3 toTarget = target - transform.position;
        toTarget.Normalize();
        Vector3 desired = toTarget * maxSpeed;
        return desired - velocity;
    }

    Vector3 Arrive(Vector3 target)
    {
        Vector3 toTarget = target - transform.position;

        float distance = toTarget.magnitude;
        if (distance < 0.5f)
        {
            velocity = Vector3.zero;
        }
        float ramped = maxSpeed * (distance / slowingDistance);
        float clamped = Mathf.Min(ramped, maxSpeed);
        Vector3 desired = clamped * (toTarget / distance);

        return desired - velocity;
    }

    public Vector3 Pursue(GameObject target)
    {
        Vector3 toTarget = target.transform.position - transform.position;
        float lookAhead = toTarget.magnitude / maxSpeed;
        pursueTargetPos = target.transform.position
           + (target.GetComponent<Pilot>().velocity * lookAhead);
        target.GetComponent<Pilot>().beingChased = true;
        isChasing = true;
        return Seek(pursueTargetPos);
    }

    void OnCollisionEnter(Collision c)
    {
        print("FRACK");
    }


    //void OnDrawGizmos()
    //{
    //    if (seekEnabled)
    //    {
    //        Gizmos.color = Color.cyan;
    //        Gizmos.DrawRay(transform.position, seekTarget);
    //    }
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawRay(transform.position, transform.position + force);
    //}

}
