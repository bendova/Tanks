using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour
{
    public float m_LifeTime = 1.0f;

	void Start()
    {
	    Destroy(gameObject, m_LifeTime);
	}
}
