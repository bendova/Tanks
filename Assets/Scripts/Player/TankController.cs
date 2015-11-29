using UnityEngine;
using System.Collections;

public class TankController : MonoBehaviour
{
    public float SpeedFactor = 10.0f;

    private static Vector3 s_UpVector = new Vector3(0.0f, 1.0f, 0.0f);

	void Start()
    {
	
	}
	
	void Update()
	{
	    UpdateMovement();
	}

    private void UpdateMovement()
    {
        float hInput = Input.GetAxis("Horizontal1");
        float vInput = Input.GetAxis("Vertical1");

        Vector3 forwardDirection = Vector3.ProjectOnPlane(transform.forward, s_UpVector).normalized;
        transform.position = transform.position + forwardDirection * vInput * SpeedFactor;
        transform.Rotate(s_UpVector, hInput, Space.World);
    }
}
