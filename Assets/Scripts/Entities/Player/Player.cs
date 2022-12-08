using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player Character
/// </summary>
public class Player : MonoBehaviour
{
    #region static 
    static public Player Instance { get; private set; }
    #endregion

    //[Header("Prefab")]
    //public Bullet m_prefab_player_bullet;

    [Header("Components")]
    [SerializeField] Rigidbody m_rigidbody;
    [SerializeField] ParticleSystem m_particle_system;

    [Header("Parameter")]
    public float m_move_speed = 1;
    public float m_max_speed = 1;

    [Header("Variables")]
    Camera m_camera;
    ParticleSystem.EmissionModule m_emission;

    //------------------------------------------------------------------------------

    void Awake()
    {
        Instance = this;
    }

    void OnDestroy()
    {
        Instance = null;
    }

    public void StartRunning()
    {
    }

    public void Win()
    {
        StageLoop.Instance.Win();
    }

    public void GameOver()
    {
        StageLoop.Instance.GameOver();
    }

    void Start()
    {
        m_camera = Camera.main;
        m_emission = m_particle_system.emission;
    }

    void Update()
    {
        RotateShip();
    }

    void FixedUpdate()
    {
        // Slow down the ship as we don't want it to fly to the moon
        m_rigidbody.velocity /= 1.01f;
        // Stop emission
        m_emission.enabled = false;

        // Add force depending of direction
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q))
        {
            AddForceToShip(new Vector3(-1, 0, 0));
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            AddForceToShip(new Vector3(1, 0, 0));
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Z))
        {
            AddForceToShip(new Vector3(0, 1, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            AddForceToShip(new Vector3(0, -1, 0));
        }

        // Set max speed of rigidbody
        m_rigidbody.velocity = Vector3.ClampMagnitude(m_rigidbody.velocity, m_max_speed);
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.transform.tag)
        {
            case "EnemyBullet":
                GetComponent<PlayerLife>().HitPlayer(1);
                break;
            default:
                // Nothing to do with that object
                break;
        }
    }

    // Rotate ship to face mouse position
    void RotateShip()
    {
        float AngleRad = 0.0f;
        float AngleDeg = 0.0f;
        Vector3 mousePosition = GetMousePosition();

        // Calculation rotation needed for the ship to face mousePosition
        AngleRad = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x);
        AngleDeg = (180 / Mathf.PI) * AngleRad;

        // Only rotate on the z axis
        transform.rotation = Quaternion.Euler(0, 0, AngleDeg - 90f);
    }

    // Set the way we add force inside only one function
    private void AddForceToShip(Vector3 direction)
    {
        //direction = GetDirectionWithRotation(direction);

        m_rigidbody.AddForce(direction * m_move_speed);

        // Play emission
        m_emission.enabled = true;
    }

    // Calculate the direction with the rotation of the ship
    private Vector3 GetDirectionWithRotation(Vector3 direction)
    {
        return Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z) * direction;
    }

    // Get mouse world positions set with the correct z
    // Can be used later to find directions for bullets or objects
    public Vector3 GetMousePosition()
    {
        Vector3 mousePosition = m_camera.ScreenToWorldPoint(Input.mousePosition);

        // Set values Z of mouse to ship's z position in case we change it later
        // It needs to be the same as the ship otherwise the rotation will not be in only one direction
        mousePosition.z = transform.position.z;

        return mousePosition;
    }
}
