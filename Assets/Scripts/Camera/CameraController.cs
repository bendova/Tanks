using System;
using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform[] m_Targets;
    public float m_OffsetY = 1f;
    public float m_MinOffsetX = -10.0f;
    public float m_MinY = 10.0f;
    public float m_MaxY = 60.0f;

	void LateUpdate()
	{
	    Vector3 averagePos = GetAveragePosition();
        Vector3 position = new Vector3(averagePos.x, averagePos.y, averagePos.z);
	    position.x = m_MinOffsetX + GetTargetsMinX();
	    position.y = Mathf.Clamp(m_OffsetY * GetTargetsDistance(), m_MinY, m_MaxY);
        transform.position = position;
        transform.LookAt(GetAveragePosition());
	}

    private Vector3 GetAveragePosition()
    {
        Vector3 average = new Vector3();
        int targetsCount = 0;
        for (int i = 0; i < m_Targets.Length; ++i)
        {
            if (m_Targets[i] != null)
            {
                average += m_Targets[i].position;
                ++targetsCount;
            }
        }
        if (targetsCount > 0)
        {
            average /= targetsCount;
        }
        else
        {
            // FIXME we'll have to do something here   
        }
        return average;
    }


    private float GetTargetsDistance()
    {
        float distance = 0f;
        if ((m_Targets[0] != null) && (m_Targets[1] != null))
        {
            distance = (m_Targets[0].position - m_Targets[1].position).magnitude;
        }
        return distance;
    }

    private float GetTargetsMinX()
    {
        float minX = Single.MaxValue;
        for (int i = 0; i < m_Targets.Length; ++i)
        {
            if (m_Targets[i] != null)
            {
                if (m_Targets[i].position.x < minX)
                {
                    minX = m_Targets[i].position.x;
                }
            }
        }
        return minX;
    }
}
