using UnityEngine;
using System.Collections.Generic;

public class Boid : MonoBehaviour
{
    public string preferedEnemy;
    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 force;
    public float mass = 1.0f;
    public float straighteningTendancy = 0.2f;
    public float damping = 0.0f;


    public bool isChasing;
    public bool beingChased;

    public float maxSpeed = 20.0f;
    public float maxForce = 10.0f;

    public bool seekEnabled;
    public Vector3 seekTargetPosition;

    public bool arriveEnabled;
    public Vector3 arriveTargetPosition;
    public float slowingDistance = 15;

    public bool fleeEnabled;
    public float fleeRange = 15.0f;
    public Vector3 fleeTargetPosition;

    public bool pursueEnabled;
    public GameObject pursueTarget;
    Vector3 pursueTargetPos;

    public bool offsetPursueEnabled = false;
    public GameObject offsetPursueTarget = null;
    Vector3 offset;
    Vector3 offsetPursueTargetPos;

    public int current = 0;

    public bool wanderEnabled = false;
    public float wanderRadius = 10.0f;
    public float wanderJitter = 1.0f;
    public float wanderDistance = 15.0f;
    private Vector3 wanderTargetPos;

    public bool planeAvoidanceEnabled;
    public float feelerDistance = 20.0f;
    private bool planeAvoidanceActive = false;
    List<Vector3> planeAvoidanceFeelers = new List<Vector3>();
    public List<Plane> planes = new List<Plane>();

    public void MakeFeelers()
    {
        planeAvoidanceFeelers.Clear();
        Vector3 newFeeler = Vector3.forward * feelerDistance;
        newFeeler = transform.TransformPoint(newFeeler);
        planeAvoidanceFeelers.Add(newFeeler);

        newFeeler = Vector3.forward * feelerDistance;
        newFeeler = Quaternion.AngleAxis(45, Vector3.up) * newFeeler;
        newFeeler = transform.TransformPoint(newFeeler);
        planeAvoidanceFeelers.Add(newFeeler);

        newFeeler = Vector3.forward * feelerDistance;
        newFeeler = Quaternion.AngleAxis(-45, Vector3.up) * newFeeler;
        newFeeler = transform.TransformPoint(newFeeler);
        planeAvoidanceFeelers.Add(newFeeler);

        newFeeler = Vector3.forward * feelerDistance;
        newFeeler = Quaternion.AngleAxis(45, Vector3.right) * newFeeler;
        newFeeler = transform.TransformPoint(newFeeler);
        planeAvoidanceFeelers.Add(newFeeler);

        newFeeler = Vector3.forward * feelerDistance;
        newFeeler = Quaternion.AngleAxis(-45, Vector3.right) * newFeeler;
        newFeeler = transform.TransformPoint(newFeeler);
        planeAvoidanceFeelers.Add(newFeeler);
    }

    public Vector3 PlaneAvoidance()
    {
        MakeFeelers();
        planeAvoidanceActive = false;
        Vector3 force = Vector3.zero;
        foreach (Vector3 feeler in planeAvoidanceFeelers)
        {
            foreach (Plane plane in planes)
            {
                if (!plane.GetSide(feeler))
                {
                    planeAvoidanceActive = true;
                    float distance = Mathf.Abs(plane.GetDistanceToPoint(feeler));
                    force += plane.normal * distance;
                }
            }
        }
        return force;
    }

    public void TurnOffAll()
    {
        seekEnabled = arriveEnabled = fleeEnabled = pursueEnabled = offsetPursueEnabled = wanderEnabled = planeAvoidanceEnabled = false;
    }

    public Vector3 Wander()
    {
        float jitterTimeSlice = wanderJitter * Time.deltaTime;

        Vector3 toAdd = Random.insideUnitSphere * jitterTimeSlice;
        wanderTargetPos += toAdd;
        wanderTargetPos.Normalize();
        wanderTargetPos *= wanderRadius;

        /*Quaternion jitter = Quaternion.AngleAxis(jitterTimeSlice, Random.insideUnitSphere);
        wanderTargetPos = jitter * wanderTargetPos;
        */
        Vector3 localTarget = wanderTargetPos + Vector3.forward * wanderDistance;
        Vector3 worldTarget = transform.TransformPoint(localTarget);
        return (worldTarget - transform.position);
    }

    public Vector3 Pursue(GameObject target)
    {
        Vector3 toTarget = target.transform.position - transform.position;
        float lookAhead = toTarget.magnitude / maxSpeed;
        pursueTargetPos = target.transform.position
           + (target.GetComponent<Boid>().velocity * lookAhead);

        return Seek(pursueTargetPos);
    }

    GameObject FindNearestTarget()
    {
        GameObject[] potientialTargets = GameObject.FindGameObjectsWithTag(preferedEnemy);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        foreach (var t in potientialTargets)
        {
            if (!t.GetComponent<Boid>().isChasing && !t.GetComponent<Boid>().beingChased)
            {
                Vector3 diff = t.transform.position - transform.position;
                float curDistance = diff.magnitude;
                if (curDistance < distance)
                {
                    closest = t;
                    distance = curDistance;
                }
            }
        }
        return closest;
    }

    // Use this for initialization
    void Start()
    {
        if (offsetPursueEnabled)
        {
            offset = transform.position - offsetPursueTarget.transform.position;
            offset = Quaternion.Inverse(
                   offsetPursueTarget.transform.rotation) * offset;
        }

        wanderTargetPos = Random.insideUnitSphere * wanderRadius;
    }


    public Vector3 OffsetPursue(GameObject leader, Vector3 offset)
    {
        Vector3 target = leader.transform.TransformPoint(offset);
        Vector3 toTarget = transform.position - target;
        float dist = toTarget.magnitude;
        float lookAhead = dist / maxSpeed;

        offsetPursueTargetPos = target + (lookAhead * leader.GetComponent<Boid>().velocity);
        return Arrive(offsetPursueTargetPos);
    }

    public Vector3 Flee(Vector3 targetPos, float range)
    {
        Vector3 desiredVelocity;
        desiredVelocity = transform.position - targetPos;
        if (desiredVelocity.magnitude > range)
        {
            return Vector3.zero;
        }
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;
        Debug.Log("Flee");
        return desiredVelocity - velocity;
    }
    public Vector3 Arrive(Vector3 targetPos)
    {
        Vector3 toTarget = targetPos - transform.position;

        float slowingDistance = 15.0f;
        float distance = toTarget.magnitude;
        if (distance < 0.5f)
        {
            velocity = Vector3.zero;
            return Vector3.zero;
        }
        float ramped = maxSpeed * (distance / slowingDistance);

        float clamped = Mathf.Min(ramped, maxSpeed);
        Vector3 desired = clamped * (toTarget / distance);

        return desired - velocity;
    }

    void OnDrawGizmos()
    {
        if (seekEnabled)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, seekTargetPosition);
        }
        if (arriveEnabled)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, arriveTargetPosition);
        }
        if (pursueEnabled)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, pursueTargetPos);
        }
        if (offsetPursueEnabled)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, offsetPursueTargetPos);
        }

        if (wanderEnabled)
        {
            Gizmos.color = Color.blue;
            Vector3 wanderCircleCenter = transform.TransformPoint(Vector3.forward * wanderDistance);
            Gizmos.DrawWireSphere(wanderCircleCenter, wanderRadius);
            Gizmos.color = Color.green;
            Vector3 worldTarget = transform.TransformPoint(wanderTargetPos + Vector3.forward * wanderDistance);
            Gizmos.DrawLine(transform.position, worldTarget);
        }

        if (planeAvoidanceEnabled && planeAvoidanceActive)
        {
            foreach (Vector3 feeler in planeAvoidanceFeelers)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawLine(transform.position, feeler);
            }
        }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + force);
    }

    Vector3 Seek(Vector3 target)
    {
        Vector3 toTarget = target - transform.position;
        toTarget.Normalize();
        Vector3 desired = toTarget * maxSpeed;
        return desired - velocity;
    }

    // Update is called once per frame
    void Update()
    {
        force = Vector3.zero;

        if (seekEnabled)
        {
            force += Seek(seekTargetPosition);
        }

        if (arriveEnabled)
        {
            force += Arrive(arriveTargetPosition);
        }

        if (fleeEnabled && beingChased)
        {
            force += Flee(fleeTargetPosition, fleeRange);
            arriveEnabled = false;
            seekEnabled = false;
        }

        if (pursueEnabled && pursueTarget != null)
        {
            seekEnabled = false;
            arriveEnabled = false;
            force += Pursue(pursueTarget);
            isChasing = true;
            //pursueTarget.GetComponent<Boid>().beingChased = true;
            //pursueTarget.GetComponent<Boid>().fleeTargetPosition = transform.position;
        }
        else
        {
            wanderEnabled = true;
        }

        //if(beingChased && pursueTarget != null)
        //{
        //    pursueTarget = null;
        //    pursueEnabled = false;
        //}

        if (offsetPursueEnabled)
        {
            force += OffsetPursue(offsetPursueTarget, offset);
        }

        if (wanderEnabled)
        {
            force += Wander();
            pursueTarget = FindNearestTarget();
            if (pursueTarget != null)
            {
                pursueEnabled = true;
                wanderEnabled = false;
            }
        }

        if (planeAvoidanceEnabled)
        {
            force += PlaneAvoidance();
        }

        force = Vector3.ClampMagnitude(force, maxForce);
        Vector3 acceleration = force / mass;
        velocity += acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        if (velocity.magnitude > float.Epsilon)
        {
            transform.forward = velocity;
        }

        transform.position += velocity * Time.deltaTime;


        // Apply damping
        velocity *= (1.0f - damping * Time.deltaTime);

        /*
        force = Vector3.ClampMagnitude(force, maxForce);
        acceleration = force / mass;
        velocity += acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        transform.position += velocity * Time.deltaTime;
        if (velocity.magnitude > float.Epsilon)
        {
            transform.forward = velocity;
        }     
        */
    }

}
