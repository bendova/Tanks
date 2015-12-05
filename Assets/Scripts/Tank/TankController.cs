using UnityEngine;
using System.Collections;

public class TankController : MonoBehaviour
{
    public enum PlayerIndex
    {
        None = 0,
        Player_01 = 1,
        Player_02 = 2,
    }

    public float m_MoveSpeedFactor = 0.2f;
    public float m_ProjectileAccMin = 5.0f;
    public float m_ProjectileAccMax = 20.0f;
    public float m_ShotPowerUpFactor = 1.0f;
    public PlayerIndex m_PlayerIndex = PlayerIndex.None;
    public Transform m_ShellSpawner;
    public GameObject m_Projectile;
    public GameObject m_LaunchFlash;

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
        float hInput = GetAxis("Horizontal");
        float vInput = GetAxis("Vertical");
        
        Vector3 forwardDirection = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        transform.position = transform.position + forwardDirection * vInput * m_MoveSpeedFactor;
        transform.Rotate(Vector3.up, hInput, Space.World);
    }

    private void UpdateFiring()
    {
        m_IsPoweringUp = m_IsPoweringUp || GetButtonDown("Fire");
        if (m_IsPoweringUp)
        {
            m_ProjectileAcc = Mathf.Lerp(m_ProjectileAccMin, m_ProjectileAccMax, m_TimeSincePowerup);
            m_TimeSincePowerup += m_ShotPowerUpFactor * Time.deltaTime;
        }

        bool fireUp = GetButtonUp("Fire");
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
            GameObject.Instantiate(m_LaunchFlash, m_ShellSpawner.position, m_ShellSpawner.rotation);
            ShellController controller = projectile.GetComponent<ShellController>();
            if (controller)
            {
                controller.m_Launcher = gameObject;
                controller.m_LaunchSpeed = projectileAcc;
            }
        }
    }

    private float GetAxis(string axis)
    {
        return Input.GetAxis(GetInputName(axis));
    }

    private bool GetButtonDown(string button)
    {
        return Input.GetButtonDown(GetInputName(button));
    }

    private bool GetButtonUp(string button)
    {
        return Input.GetButtonUp(GetInputName(button));
    }

    private string GetInputName(string input)
    {
        return input + (int) m_PlayerIndex;
    }
}
