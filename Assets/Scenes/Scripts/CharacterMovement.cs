using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float playerHeight;

    [HideInInspector] public float walkSpeed;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;

    [Header("Health")]
    public float minWet= 0;
    public float currentWet;

    // [Header("Ground Check")]
    // 
    // public LayerMask whatIsGround;
    // bool grounded;

    [Header("DoorAction")]
    [SerializeField] private Text UseText;
    [SerializeField] private Transform Player;
    [SerializeField] private float MaxUseDistance = 5f;
    [SerializeField] private LayerMask UseLayer;

    [Header("")]
    public Transform orientation;
    public RainController rain;
    bool isRain = false;
    public GameObject setRain;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        setRain.SetActive(false);
    }

    private void Update()
    {
        // ground check
        // grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();

        // handle drag
        rb.drag = groundDrag;

        if (Input.GetKeyDown(KeyCode.Q) && isRain == false)
        {
            setRain.SetActive(true);
            Debug.Log("hujan");
            isRain = true;
            StartCoroutine(Example());
            TakeWet(10);
        }
        else if(Input.GetKeyDown(KeyCode.Q) && isRain == true)
        {
            setRain.SetActive(false);
            Debug.Log("berhenti");
            isRain = false;
            StartCoroutine(Example());
            TakeWet(0);
        }

        //Door Open
        if(Physics.Raycast(Player.position, Player.forward, out RaycastHit hit, MaxUseDistance, UseLayer)
            && hit.collider.TryGetComponent<Doors>(out Doors doors)) 
        {
            if (!doors.isOpen)
            {
                UseText.text = "Open \"E\"";
            }
            UseText.gameObject.SetActive(true);
            UseText.transform.position = hit.point - (hit.point - Player.position).normalized * 0.01f;
            UseText.transform.rotation = Quaternion.LookRotation((hit.point - Player.position).normalized);
        }
        else
        {
            UseText.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 5f, ForceMode.Force);

        //on Slope
        if(OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 3f, ForceMode.Force);
        }

        // on ground
        // if(grounded)
            
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private bool OnSlope() 
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    void TakeWet(float wet)
    {
        currentWet += wet;
        rain.SetHealth(currentWet);
        //Mathf.SmoothDamp(rain.SetHealth(currentWet), minWet, ref currentWet, 100 * Time.deltaTime);
    }

    IEnumerator Example()
    {
        yield return new WaitForSeconds(5);
        print(Time.time);
    }

    public void OnUse()
    {
        if(Physics.Raycast(Player.position, Player.forward, out RaycastHit hit, MaxUseDistance, UseLayer))
        {
            if(hit.collider.TryGetComponent<Doors>(out Doors doors))
            {
                if(!doors.isOpen)
                {
                    doors.Open(transform.position);
                }
            }
        }
    }
}