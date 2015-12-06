using UnityEngine;
using System.Collections;

public class ShellController : MonoBehaviour
{
    public float m_LaunchSpeed = 10.0f;
    public float m_Damage = 50.0f;
    public GameObject m_Explosion;
    [HideInInspector] public GameObject m_Launcher;

    private Rigidbody m_RigidBody;
    private Vector3 m_ForwardDirection;
    private float m_LaunchAngle;
    private float m_SinLaunchAngle;
    private float m_CosLaunchAngle;
    private float m_TimeSinceLaunch = 0.0f;
    private Vector3 m_LaunchPosition;
    private Quaternion m_InitialRotation;

    void Start()
	{
	    m_RigidBody = GetComponent<Rigidbody>();
        m_RigidBody.useGravity = false;

        m_LaunchPosition = transform.position;
        m_LaunchAngle = (360f - transform.rotation.eulerAngles.x) * Mathf.Deg2Rad;
        m_InitialRotation = transform.rotation;
        m_InitialRotation.x = 0f;
        m_InitialRotation.z = 0f;
        m_ForwardDirection = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        m_CosLaunchAngle = Mathf.Cos(m_LaunchAngle);
        m_SinLaunchAngle = Mathf.Sin(m_LaunchAngle);
	}

    void FixedUpdate()
    {
        m_TimeSinceLaunch += Time.fixedDeltaTime;
        Vector2 parabolicPos = GetParabolicPosition(m_TimeSinceLaunch);
        UpdatePosition(parabolicPos);
        UpdateOrientation(parabolicPos);
    }

    private void UpdatePosition(Vector2 parabolicPos)
    {
        transform.position = GetWorldPositionFromParabolicPos(parabolicPos);
    }

    private Vector3 GetWorldPositionFromParabolicPos(Vector2 parabolicPos)
    {
        Vector3 nextPos = m_ForwardDirection * parabolicPos.x + Vector3.up * parabolicPos.y;
        return m_LaunchPosition + nextPos;
    }

    private void UpdateOrientation(Vector2 parabolicPos)
    {
        Vector2 nextParabolicPos = GetParabolicPosition(m_TimeSinceLaunch + Time.fixedDeltaTime);
        Vector3 nextOrientation = m_ForwardDirection * (nextParabolicPos.x - parabolicPos.x) +
            Vector3.up * (nextParabolicPos.y - parabolicPos.y);

        DrawDebugLine(parabolicPos, nextParabolicPos);

        Quaternion newRotation = Quaternion.FromToRotation(m_ForwardDirection, nextOrientation);
        transform.rotation = newRotation * m_InitialRotation;
    }

    private void DrawDebugLine(Vector3 parabolicPos, Vector3 nextParabolicPos)
    {
        Vector3 lineStart = GetWorldPositionFromParabolicPos(parabolicPos);
        Vector3 lineEnd = GetWorldPositionFromParabolicPos(nextParabolicPos);
        Debug.DrawLine(lineStart, lineEnd, Color.red, 5.0f);
    }

    private Vector2 GetParabolicPosition(float timeSinceLaunch)
    {
        float x = m_LaunchSpeed * m_CosLaunchAngle * timeSinceLaunch;
        float y = m_LaunchSpeed * m_SinLaunchAngle * timeSinceLaunch - (Physics.gravity.magnitude * timeSinceLaunch * timeSinceLaunch) / 2;
        return new Vector2(x, y);
    }

    void OnTriggerEnter(Collider other)
    {
        if (m_Launcher != other.gameObject)
        {
            if (other.gameObject.tag == Tags.Tank)
            {
                DestroyTank(other.gameObject);
            }
            Utils.DestroyWithExplosion(gameObject, m_Explosion);
        }
    }

    private void DestroyTank(GameObject tank)
    {
        HitPoints hitPoints = tank.GetComponent<HitPoints>();
        if (hitPoints)
        {
            hitPoints.TakeDamage(m_Damage);
        }
    }
}
