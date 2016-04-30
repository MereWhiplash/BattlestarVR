using UnityEngine;
using System.Collections;

public class XController : MonoBehaviour {

    public float speed = 20.0f;
    // Use this for initialization
    void Start()
    {
    }

    void Yaw(float angle)
    {
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.up);
        transform.rotation = rot * transform.rotation;
    }

    void Roll(float angle)
    {
        Quaternion rot = Quaternion.AngleAxis(angle, transform.forward);
        transform.rotation = rot * transform.rotation;
    }

    void Pitch(float angle)
    {
        float invcosTheta1 = Vector3.Dot(transform.forward, Vector3.up);
        float threshold = 0.95f;
        if ((angle > 0 && invcosTheta1 < (-threshold)) || (angle < 0 && invcosTheta1 > (threshold)))
        {
            return;
        }

        // A pitch is a rotation around the right vector
        Quaternion rot = Quaternion.AngleAxis(angle, transform.right);

        transform.rotation = rot * transform.rotation;
    }

    void Walk(float units)
    {
        transform.position += transform.forward * units;
    }

    void Strafe(float units)
    {
        transform.position += transform.right * units;
    }

    // Update is called once per frame
    void Update()
    {
        float speed = this.speed;

        if (Input.GetKey(KeyCode.Joystick1Button1))
        {
            speed *= 5.0f;
        }

        if (Input.GetAxis("R2") == 1)
        {
            Walk(Time.deltaTime * speed);
        }

        if (Input.GetAxis("LeftStickY") == 1)
        {
            Pitch(Time.deltaTime * speed);
        }

        if (Input.GetAxis("LeftStickY") == -1)
        {
            Pitch(-Time.deltaTime * speed);
        }

        if (Input.GetAxis("LeftStickX") == 1)
        {
            Yaw(Time.deltaTime * speed);
        }

        if (Input.GetAxis("LeftStickX") == -1)
        {
            Yaw(-Time.deltaTime * speed);
        }

        if (Input.GetAxis("RightStickX") == -1)
        {
            Roll(-Time.deltaTime * speed);
        }

        if (Input.GetAxis("RightStickX") == 1)
        {
            Roll(Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.Joystick1Button4))
        {
            Strafe(-Time.deltaTime * 5);
        }
        if (Input.GetKey(KeyCode.Joystick1Button5))
        {
            Strafe(Time.deltaTime * 5);
        }

    }
    }
