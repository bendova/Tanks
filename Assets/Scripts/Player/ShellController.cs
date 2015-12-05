using UnityEngine;
using System.Collections;

public class ShellController : MonoBehaviour
{
    public float m_LaunchSpeed = 10.0f;
    public GameObject m_Launcher;
    public GameObject m_Explosion;

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

    void Update()
    {
        m_TimeSinceLaunch += Time.fixedDeltaTime;
        Vector2 parabolicPos = GetProjectilePosition(m_TimeSinceLaunch);
        UpdatePosition(parabolicPos);
        UpdateOrientation(parabolicPos);
    }

    private void UpdatePosition(Vector2 parabolicPos)
    {
        Vector3 nextPos = m_ForwardDirection * parabolicPos.x + Vector3.up * parabolicPos.y;
        transform.position = m_LaunchPosition + nextPos;
    }

    private void UpdateOrientation(Vector2 parabolicPos)
    {
        Vector2 nextParabolicPos = GetProjectilePosition(m_TimeSinceLaunch + Time.fixedDeltaTime);
        Vector3 nextOrientation = m_ForwardDirection * (nextParabolicPos.x - parabolicPos.x) +
            Vector3.up * (nextParabolicPos.y - parabolicPos.y);
        float angle = Vector3.Angle(m_ForwardDirection, nextOrientation) * Mathf.Deg2Rad;
        if (nextOrientation.y < 0)
        {
            angle = -angle;
        }
        Quaternion newRotation = Quaternion.FromToRotation(m_ForwardDirection, nextOrientation);
        transform.rotation = newRotation * m_InitialRotation;
    }

    private Vector2 GetProjectilePosition(float timeSinceLaunch)
    {
        float x = m_LaunchSpeed * m_CosLaunchAngle * timeSinceLaunch;
        float y = m_LaunchSpeed * m_SinLaunchAngle * timeSinceLaunch - (Physics.gravity.magnitude * m_TimeSinceLaunch * timeSinceLaunch) / 2;
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
            SpawnExplosion(m_Explosion);
            Destroy(gameObject);
        }
    }

    private void DestroyTank(GameObject tank)
    {
        TankController controller = tank.GetComponent<TankController>();
        SpawnExplosion(controller.m_Explosion);
        Destroy(tank);
    }

    private void SpawnExplosion(GameObject explosionPrefab)
    {
        GameObject explosion = GameObject.Instantiate(explosionPrefab, transform.position, Quaternion.identity) as GameObject;
        float destroyTime = 1f;
        if (explosion)
        {
            ParticleSystem particles = explosion.GetComponent<ParticleSystem>();
            if (particles)
            {
                destroyTime = particles.duration;
            }
        }
        Destroy(explosion, destroyTime);
    }
}
