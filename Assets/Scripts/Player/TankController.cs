using UnityEngine;
using System.Collections;

public class TankController : MonoBehaviour
{
    public float m_MoveSpeedFactor = 0.2f;
    public float m_ProjectileAccMin = 5.0f;
    public float m_ProjectileAccMax = 20.0f;
    public float m_ShotPowerUpFactor = 1.0f;
    public Transform m_ShellSpawner;
    public GameObject m_Projectile;
    public GameObject m_Explosion;

    private float m_ProjectileAcc = 0;
    private float m_TimeSincePowerup = 0;
    private bool m_IsPoweringUp = false;

	void Update()
	{
	    UpdateMovement();
	    UpdateFiring();
	}
    
    private void UpdateMovement()
    {
        float hInput = Input.GetAxis("Horizontal1");
        float vInput = Input.GetAxis("Vertical1");
        
        Vector3 forwardDirection = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        transform.position = transform.position + forwardDirection * vInput * m_MoveSpeedFactor;
        transform.Rotate(Vector3.up, hInput, Space.World);
    }

    private void UpdateFiring()
    {
        m_IsPoweringUp = m_IsPoweringUp || Input.GetButtonDown("Fire1");
        if (m_IsPoweringUp)
        {
            m_ProjectileAcc = Mathf.Lerp(m_ProjectileAccMin, m_ProjectileAccMax, m_TimeSincePowerup);
            m_TimeSincePowerup += m_ShotPowerUpFactor * Time.deltaTime;
        }

        bool fireUp = Input.GetButtonUp("Fire1");
        if (fireUp)
        {
            FireProjectile(m_ProjectileAcc);
            m_IsPoweringUp = false;
            m_ProjectileAcc = 0f;
            m_TimeSincePowerup = 0f;
        }
    }

    private void FireProjectile(float projectileAcc)
    {
        GameObject projectile = GameObject.Instantiate(m_Projectile, m_ShellSpawner.position, m_ShellSpawner.rotation) as GameObject;
        if (projectile)
        {
            ShellController controller = projectile.GetComponent<ShellController>();
            if (controller)
            {
                controller.m_Launcher = gameObject;
                controller.m_LaunchSpeed = projectileAcc;
            }
        }
    }
}
