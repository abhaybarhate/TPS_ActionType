using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{

    [Header("References")]
    [SerializeField] Transform orientation;
    [SerializeField] WeaponSelect WeaponHolder;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 3;
    [SerializeField] float runningSpeed = 8;
    [SerializeField] float crouchingSpeed = 1.5f ;
    [SerializeField] float jumpForce = 7f ;
    [SerializeField] float jumpCooldown = 0.25f;
    [SerializeField] float airMultiplier;
    [SerializeField] float airDrag = 2f;
    [SerializeField] float strafingSpeed = 6;

    [Header("Rifle Movement")]
    [SerializeField] float maxRifleSpeed = 8f;
    [SerializeField] float minRifleSpeed = 3f;
    [SerializeField] float rifleWalkSpeed = 3f;
    [SerializeField] float rifleRunSpeed = 5f;
    [SerializeField] float rifleSprintSpeed = 8f;
    [SerializeField] float rifleCrouchWalkSpeed = 2f;
    
    [Header("Handgun Movement")]
    [SerializeField] float maxHandgunSpeed = 8f;
    [SerializeField] float minHandgunSpeed = 3f;
    [SerializeField] float handgunWalkSpeed = 3f;
    [SerializeField] float handgunRunSpeed = 5f;
    [SerializeField] float handgunCrouchSpeed = 2f;

    bool canPlayerJump = true;
    bool isPlayerRunning = false;
    bool canPlayerRun = true;
    bool isPlayerHavingRifle = false;
    bool isPlayerHavingPistol = false;
    bool isPlayerAiming = false;
    bool isPlayerCrouching = false;

    [Header("Ground Check")]
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask GroundMask;
    [SerializeField] float groundDrag;
    bool grounded;

    float horizontalInput;
    float verticalInput;
     
    Vector3 movementDirection;
    PlayerState playerState;
    [SerializeField] Animator playerAnimatorController;
    Rigidbody playerRb;

    private void Start() 
    {

        playerState = PlayerState.Normal;
        playerRb = GetComponent<Rigidbody>();
        playerRb.freezeRotation = true;     
        // /Cursor cursor;
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    private void Update() 
    {
        MyInputs();
        SpeedControl();
        //CheckCurrentWeapon();
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, GroundMask);
        if(grounded) { 
            playerRb.drag = groundDrag;
        }
        else
            playerRb.drag = airDrag;
        
        //Debug.Log(playerRb.velocity);
    }

    private void FixedUpdate() 
    {
        MovePlayer();
        if(isPlayerRunning)
            PlayerRun();

        if(playerState == PlayerState.Combat)
        {
            //PlayerStrafe();
        }
        //Debug.Log(playerAnimatorController.GetBool("isStrafe"));
    }

    private void MyInputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(KeyCode.Space) && canPlayerJump && grounded)
        {
            canPlayerJump = false;
            Debug.Log("Pressed Space");
            Jump();
            Debug.Log("Jumped");
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if(Input.GetKey(KeyCode.LeftShift) && canPlayerRun && grounded)
        {
            isPlayerRunning = true;
        }
        else 
            isPlayerRunning = false;
        if(Input.GetKeyDown(KeyCode.Alpha2)) { playerState = PlayerState.Combat; }

        if(Input.GetMouseButton(1))
        {
            isPlayerAiming = true;
        }
        else isPlayerAiming = false;
    }

    private void MovePlayer()
    {
        movementDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        // if grounded
        if(grounded)
            playerRb.AddForce(movementDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        
        // in air
        else if(!grounded)
            playerRb.AddForce(movementDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Impulse);
        
        
        // playerAnimatorController.SetFloat("ForwardMovementSpeed", playerRb.velocity.magnitude);
        
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);
        //limit velocity if needed
        if(flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            playerRb.velocity = new Vector3(limitedVelocity.x, playerRb.velocity.y, limitedVelocity.z);
        }
        if(isPlayerRunning || flatVelocity.magnitude > runningSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * runningSpeed;
            playerRb.velocity = new Vector3(limitedVelocity.x, playerRb.velocity.y, limitedVelocity.z);
        }
        // if(playerState == PlayerState.Combat || flatVelocity.magnitude > strafingSpeed)
        // {
        //     Vector3 limitedVelocity = flatVelocity.normalized * strafingSpeed;
        //     playerRb.velocity = new Vector3(limitedVelocity.x, playerRb.velocity.y, limitedVelocity.z);
        // }
    }

    private void Jump()
    {
        //reset y
        playerRb.velocity = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);
        playerRb.AddForce(transform.up*jumpForce,ForceMode.Impulse);     // Impulse mode because giving force only once
        
    }

    private void ResetJump()
    {
        canPlayerJump = true;
    }

    private void PlayerRun()
    {
        movementDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        playerRb.AddForce(movementDirection.normalized * runningSpeed * 10f, ForceMode.Force);
        //playerAnimatorController.SetFloat("ForwardMovementSpeed", playerRb.velocity.magnitude);
    }

    // private void PlayerStrafe()
    // {
    //     movementDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
    //     playerRb.AddForce(movementDirection.normalized * strafingSpeed * 10f, ForceMode.Force);
    //     playerAnimatorController.SetFloat("PlayerCombatMoveX", Input.GetAxis("Horizontal"));
    //     playerAnimatorController.SetFloat("PlayerCombatMoveY", Input.GetAxis("Vertical"));
    // }

    private void HandlePlayerState()
    {
        
    }

    // private void CheckCurrentWeapon()
    // {
    //     if(WeaponHolder.GetCurrentWeaponType() == AmmoType.Pistol)
    //     {
    //         isPlayerHavingPistol = true;
    //         isPlayerHavingRifle = false;
    //         HandlePistolMovement();
    //         //playerAnimatorController.SetBool("isPistolSelected", true);
    //         //playerAnimatorController.SetBool("isRifleSelected", false);
    //     }
    //     if(WeaponHolder.GetCurrentWeaponType() == AmmoType.Rifle)
    //     {
    //         isPlayerHavingPistol = false;
    //         isPlayerHavingRifle = true;
    //         HandleRifleMovement();
    //         playerAnimatorController.SetBool("isPistolSelected", false);
    //         playerAnimatorController.SetBool("isRifleSelected", true);
    //     }
    // }

    // private void HandlePistolMovement()
    // {
    //     movementDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
    //     if(!isPlayerRunning)
    //     {
    //         playerRb.AddForce(movementDirection.normalized * handgunWalkSpeed * 10f, ForceMode.Force);
    //     }
    //     else if(isPlayerRunning)
    //     {
    //         playerRb.AddForce(movementDirection.normalized * handgunRunSpeed * 10f, ForceMode.Force);
    //     }
    //     playerAnimatorController.SetFloat("PistolAimMoveX", playerRb.velocity.magnitude);
    //     playerAnimatorController.SetFloat("PistolAimMoveY", playerRb.velocity.magnitude);
    // }

    // private void HandleRifleMovement()
    // {
    //     movementDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
    //     if(!isPlayerRunning)
    //     {
    //         playerRb.AddForce(movementDirection.normalized * rifleWalkSpeed * 10f, ForceMode.Force);
           
    //     }
    //     else if(isPlayerRunning)
    //     {
    //         playerRb.AddForce(movementDirection.normalized * rifleRunSpeed * 10f, ForceMode.Force);
    //     }
    //     playerAnimatorController.SetFloat("RifleAimMoveX", playerRb.velocity.x);
    //     playerAnimatorController.SetFloat("RifleAimMoveY", playerRb.velocity.z);
    // }

}
