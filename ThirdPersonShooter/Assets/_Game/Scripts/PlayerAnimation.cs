using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
   
    [SerializeField] WeaponSelect weaponType;


    [SerializeField] float walkingAccelaration;
    [SerializeField] float deccelaration;
    [SerializeField] float runningAccelaration;
    [SerializeField] float rifleAccelaration;
    [SerializeField] float rifleRunningAccelaration;
    [SerializeField] float rifleDeccelaration;
    [SerializeField] float pistolAccelaration;
    [SerializeField] float pistolRunningAccelaration;
    [SerializeField] float pistolDeccelaration;
    [SerializeField] float maxWalkVelocity;
    [SerializeField] float maxRunVelocity;

    //Animator Parameter Variables hash
    int ForwardMovementSpeed;
    int RifleAimMoveX;
    int RifleAimMoveY;
    int PistolAimMoveX;
    int PistolAimMoveY;
    int IsPlayerAiming;
    int IsPistolSelected;
    int IsRifleSelected;
    int RifleMoveX;
    int RifleMoveY;

    // Axis Inputs
    bool leftPressed, rightPressed, forwardPressed, backwardPressed;
    bool isPistol, isRifle;

    Animator playerAnimController;
    float horizontalInput;
    float verticalInput;
    float currentMaxVelocity;
    float Velocity = 0f;
    float VelocityX = 0f;
    float VelocityZ = 0f;
    float rifleVelocityX = 0f;
    float rifleVelocityZ = 0f;
    float rifleVelocityAimX = 0f;
    float rifleVelocityAimZ = 0f;
    float pistolVelocityX = 0f;
    float pistolVelocityZ = 0f;
    bool runButtonPressed;
    bool aimButtonPressed;


    private void Start() 
    {
        playerAnimController = GetComponent<Animator>();  
        StringToHashAnimParameters();
    }

    private void Update() 
    {
        currentMaxVelocity = runButtonPressed ? maxRunVelocity : maxWalkVelocity;
        CheckInputs();
        HandleAxisInputs();
        if(isPistol)
        {
            playerAnimController.SetBool(IsPistolSelected, true);
            playerAnimController.SetBool(IsRifleSelected, false);
            HandlePistolLocomotion();
        }
        else if(isRifle)
        {
            playerAnimController.SetBool(IsRifleSelected, true);
            HandleTheRifleLocomotion();
            if(aimButtonPressed)
            {
                HandleRifleAimLocomotion();
            }
        }
        HandleTheBasicLocomotionBlend();
    }

    private void CheckInputs()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if(Input.GetKey(KeyCode.LeftShift)) { runButtonPressed = true; }
        else if(Input.GetKeyUp(KeyCode.LeftShift)) { runButtonPressed = false; }

        if(Input.GetMouseButton(1)) { aimButtonPressed = true; playerAnimController.SetBool("isPlayerAiming", true); }
        else if (Input.GetMouseButtonUp(1)) { aimButtonPressed = false; playerAnimController.SetBool("isPlayerAiming", false); }

        if(weaponType.GetCurrentWeaponType() == AmmoType.Pistol)
        {
            isPistol = true;
            isRifle = false;
        }
        if(weaponType.GetCurrentWeaponType() == AmmoType.Rifle)
        {
            isPistol = false;
            isRifle = true;
        }
        

    }

    private void StringToHashAnimParameters()
    {
        ForwardMovementSpeed = Animator.StringToHash("ForwardMovementSpeed");
        RifleAimMoveX = Animator.StringToHash("RifleAimMoveX");
        RifleAimMoveY = Animator.StringToHash("RifleAimMoveY");
        PistolAimMoveX = Animator.StringToHash("PistolAimMoveX");
        PistolAimMoveY = Animator.StringToHash("PistolAimMoveY");
        IsPlayerAiming = Animator.StringToHash("isPlayerAiming");
        IsPistolSelected = Animator.StringToHash("isPistolSelected");
        IsRifleSelected = Animator.StringToHash("isRifleSelected");
        
        RifleMoveX = Animator.StringToHash("RifleMoveX");
        RifleMoveY = Animator.StringToHash("RifleMoveY");
    }

    private void HandleTheBasicLocomotionBlend()
    {
        if((forwardPressed || rightPressed || leftPressed || backwardPressed) && Velocity < currentMaxVelocity)
        {
            Velocity += Time.deltaTime * walkingAccelaration;
        } 
        if((forwardPressed || rightPressed || leftPressed || backwardPressed) && runButtonPressed && Velocity < 8f)
        {
            Velocity += Time.deltaTime * runningAccelaration;
        }

        if((!forwardPressed && !rightPressed && !leftPressed && !backwardPressed) && Velocity > 0)
        {
            Velocity -= Time.deltaTime * deccelaration;
        }
        //Debug.Log(forwardPressed);
        playerAnimController.SetFloat(ForwardMovementSpeed, Velocity);
    }

    private void HandleTheRifleLocomotion()
    {
        
        if(rightPressed && rifleVelocityX < currentMaxVelocity)
        {
            rifleVelocityX += Time.deltaTime * rifleAccelaration;
        }
        if(!rightPressed && rifleVelocityX > 0)
        {
            rifleVelocityX -= Time.deltaTime * rifleDeccelaration;
        }
        if(leftPressed && rifleVelocityX > -currentMaxVelocity)
        {
            rifleVelocityX -= Time.deltaTime * rifleAccelaration;
        }
        if(!leftPressed && rifleVelocityX < 0)
        {
            rifleVelocityX += Time.deltaTime * rifleDeccelaration;
        }
        if(forwardPressed && rifleVelocityZ < currentMaxVelocity)
        {
            rifleVelocityZ += Time.deltaTime * rifleAccelaration;
        }
        if(!forwardPressed &&rifleVelocityZ > 0f)
        {
            rifleVelocityZ -= Time.deltaTime * rifleDeccelaration;
        }
        if(backwardPressed && rifleVelocityZ > -currentMaxVelocity)
        {
            rifleVelocityZ -= Time.deltaTime * rifleAccelaration;
        }
        if(!backwardPressed && rifleVelocityZ < 0f)
        {
            rifleVelocityZ += Time.deltaTime * rifleDeccelaration;
        }

        playerAnimController.SetFloat(RifleMoveX, rifleVelocityX);
        playerAnimController.SetFloat(RifleMoveY, rifleVelocityZ);
        
    }

    private void HandleRifleAimLocomotion()
    {
        
        if(rightPressed && rifleVelocityAimX < currentMaxVelocity)
        {
            rifleVelocityAimX += Time.deltaTime * rifleAccelaration;
        }
        if(!rightPressed && rifleVelocityAimX > 0)
        {
            rifleVelocityAimX -= Time.deltaTime * rifleDeccelaration;
        }
        if(leftPressed && rifleVelocityAimX > -currentMaxVelocity)
        {
            rifleVelocityAimX -= Time.deltaTime * rifleAccelaration;
        }
        if(!leftPressed && rifleVelocityAimX < 0)
        {
            rifleVelocityAimX += Time.deltaTime * rifleDeccelaration;
        }
        if(forwardPressed && rifleVelocityAimZ < currentMaxVelocity)
        {
            rifleVelocityAimZ += Time.deltaTime * rifleAccelaration;
        }
        if(!forwardPressed && rifleVelocityAimZ > 0f)
        {
            rifleVelocityAimZ -= Time.deltaTime * rifleDeccelaration;
        }
        if(backwardPressed && rifleVelocityAimZ > -currentMaxVelocity)
        {
            rifleVelocityAimZ -= Time.deltaTime * rifleAccelaration;
        }
        if(!backwardPressed && rifleVelocityAimZ < 0f)
        {
            rifleVelocityAimZ += Time.deltaTime * rifleDeccelaration;
        }

        playerAnimController.SetFloat(RifleAimMoveX, rifleVelocityAimX);
        playerAnimController.SetFloat(RifleAimMoveY, rifleVelocityAimZ);
    }

    private void HandlePistolLocomotion()
    {
        
        if(rightPressed && pistolVelocityX < 5)
        {
            pistolVelocityX += Time.deltaTime * pistolAccelaration;
        }
        if(!rightPressed && pistolVelocityX > 0)
        {
            pistolVelocityX -= Time.deltaTime * pistolDeccelaration;
        }
        if(leftPressed && pistolVelocityX > -5)
        {
            pistolVelocityX -= Time.deltaTime * pistolAccelaration;
        }
        if(!leftPressed && pistolVelocityX < 0)
        {
            pistolVelocityX += Time.deltaTime * rifleDeccelaration;
        }
        if(forwardPressed && pistolVelocityZ < currentMaxVelocity)
        {
            pistolVelocityZ += Time.deltaTime * pistolAccelaration;
        }
        if(!forwardPressed && pistolVelocityZ > 0f)
        {
            pistolVelocityZ -= Time.deltaTime * pistolDeccelaration;
        }
        if(backwardPressed && pistolVelocityZ > -currentMaxVelocity)
        {
            pistolVelocityZ -= Time.deltaTime * pistolAccelaration;
        }
        if(!backwardPressed && pistolVelocityZ < 0f)
        {
            pistolVelocityZ += Time.deltaTime * pistolDeccelaration;
        }

        playerAnimController.SetFloat(PistolAimMoveX, pistolVelocityX);
        playerAnimController.SetFloat(PistolAimMoveY, pistolVelocityZ);
    }

    private void HandleAxisInputs()
    {
        if(Input.GetKey(KeyCode.W))
        {
            forwardPressed = true;
        }
        else forwardPressed = false;

        if(Input.GetKey(KeyCode.A))
        {
            leftPressed = true;
        }
        else leftPressed = false;
        
        if(Input.GetKey(KeyCode.S))
        {
            backwardPressed = true;
        }
        else backwardPressed = false;

        if(Input.GetKey(KeyCode.D))
        {
            rightPressed = true;
        }
        else rightPressed = false;
    }

}
