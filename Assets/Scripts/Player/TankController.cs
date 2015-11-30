using UnityEngine;
using System.Collections;

public class TankController : MonoBehaviour
{
    public float m_SpeedFactor = 0.2f;
    public Transform m_ShellSpawner;
    public GameObject m_Projectile;
    public float m_ProjectileAcc = 1.0f;

	void Start()
	{
	}
	
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
        transform.position = transform.position + forwardDirection * vInput * m_SpeedFactor;
        transform.Rotate(Vector3.up, hInput, Space.World);
    }

    private void UpdateFiring()
    {
        bool fire = Input.GetButtonUp("Fire1");
        if (fire)
        {
            FireProjectile();
        }
    }

    private void FireProjectile()
    {
        GameObject projectile = GameObject.Instantiate(m_Projectile, m_ShellSpawner.position, m_ShellSpawner.rotation) as GameObject;
        if (projectile)
        {
            projectile.GetComponent<ShellController>().m_LaunchSpeed = m_ProjectileAcc;
        }
    }
}
