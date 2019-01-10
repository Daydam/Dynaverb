using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 10;
    public float torque = 45;

	void Update () 
	{
        if (Input.GetKey(KeyCode.W)) transform.position += transform.forward * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S)) transform.position -= transform.forward * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) transform.Rotate(Vector3.up * -torque * Time.deltaTime);
        if (Input.GetKey(KeyCode.D)) transform.Rotate(Vector3.up * torque * Time.deltaTime);
    }
}
