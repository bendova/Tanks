using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform m_Target;

    private Vector3 m_Offset;

	void Start()
	{
	    m_Offset = transform.position - m_Target.position;
	}
	
	void LateUpdate()
	{
	    transform.position = m_Target.position + m_Offset;
	}
}
