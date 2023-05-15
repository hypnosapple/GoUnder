using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    [Header("Player Info")]
    public CharacterController myController;
    public Transform cam;

    [Header("Physics")]
    // Jump and walk speed
    private float speed;

    // Simulate gravity
    private Vector3 velocity;
    private float gravity = -30f;
    public bool isGrounded;

    // Ground check variables
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [SerializeField] private CinemachineVirtualCamera vCam;
    private CinemachineBasicMultiChannelPerlin noise;

    [Header("Camera Noise")]
    public NoiseSettings walkNoise;
    public NoiseSettings runNoise;

    [Header("Move check")]
    public bool moveDisabled;
    public bool directionDisabled;
    public bool cam6DShakeOn;
    public float raycastLength = 10f;

    public bool islocked;
    public bool inFirstFloor;


   public void Start()
    {
        Instance = this;

        noise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cam6DShakeOn = false;

        islocked = false;
    }

   public void Update()
    {
        // Check if grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(islocked)
        {
            return;
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (moveDisabled)
        {
            speed = 0f;
        }

        else if (Input.GetKey(KeyCode.LeftShift) && !inFirstFloor)
        {
            speed = 27f;
            noise.m_NoiseProfile = runNoise;
        }
        else
        {
            speed = 18f;
            noise.m_NoiseProfile = walkNoise;
        }

        // Player Input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Change movement direction according to camera direction
        if (direction.magnitude >= float.Epsilon && !moveDisabled)
        {
            float playerAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            Vector3 moveDirection = Quaternion.Euler(0f, playerAngle, 0f) * Vector3.forward;

            myController.Move(moveDirection.normalized * speed * Time.deltaTime);
        }

        // Keep track of gravity
        velocity.y += gravity * Time.deltaTime;

        if (!moveDisabled)
        {
            myController.Move(velocity * Time.deltaTime);
        }
        

        // Camera shake while walking
        if ((horizontal != 0 || vertical != 0) && !moveDisabled && !cam6DShakeOn)
        {
            CameraShake(1);
        }
        else if (cam6DShakeOn)
        {
            CameraShake(6f);
        }
        else
        {
            CameraShake(0);
        }


        

    }


    public void CameraShake(float amplitude)
    {
        
        noise.m_AmplitudeGain = amplitude;


    }
}
