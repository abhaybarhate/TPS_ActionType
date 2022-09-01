using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TPCamController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform Orientation;
    [SerializeField] Transform Player;
    [SerializeField] Transform PlayerObj;  
    [SerializeField] Rigidbody PlayerRb; 
    [Header("Variables")]
    [SerializeField] float rotationSpeed;
    PlayerState playerState;

    [SerializeField] CinemachineFreeLook BasicCinemachineFreeLook;
    [SerializeField] CinemachineFreeLook CombatCinemachineFreeLook;
    [SerializeField] CinemachineFreeLook GunCinemachineFreeLook;
     
    public enum CameraMode {
        Basic,
        Combat,
        Gun
    }

    private void Awake() 
    {
        
    }

    private void Start()
    {
        CombatCinemachineFreeLook.gameObject.SetActive(false);
        GunCinemachineFreeLook.gameObject.SetActive(false);
    }

    private void Update() 
    {
        CameraCheck();
        UpdateState();
    
    }

    private void UpdateState()
    {
        if(playerState == PlayerState.Normal) { 
            SetBasicCameraMode();
        }
        else if(playerState == PlayerState.Gun){
            SetGunCameraMode();
        }
        else if(playerState == PlayerState.Combat)
        {
            SetCombatCameraMode();
        }
    }

    private void CameraCheck()
    {
        if(Input.GetKeyDown(KeyCode.Alpha7))
        {
            playerState = PlayerState.Normal;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha8))
        {
            playerState = PlayerState.Combat;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            playerState = PlayerState.Gun;
        }
    }

    private void SetBasicCameraMode()
    {
        BasicCinemachineFreeLook.gameObject.SetActive(true);
        CombatCinemachineFreeLook.gameObject.SetActive(false);
        GunCinemachineFreeLook.gameObject.SetActive(false);
        //rotate orientation
        Vector3 ViewDirection = Player.position - new Vector3(transform.position.x, Player.position.y, transform.position.z);
        Orientation.forward = ViewDirection.normalized;
        // rotate the Player body
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 InputDirection = Orientation.forward * verticalInput + Orientation.right * horizontalInput;
        if(InputDirection != Vector3.zero)
        {
            Debug.Log("Reached here");
            PlayerObj.forward = Vector3.Slerp(PlayerObj.forward, InputDirection.normalized, Time.deltaTime * rotationSpeed);
        }
    }

    private void SetCombatCameraMode()
    {
        BasicCinemachineFreeLook.gameObject.SetActive(false);
        CombatCinemachineFreeLook.gameObject.SetActive(true);
        GunCinemachineFreeLook.gameObject.SetActive(false);
        Vector3 LookAtDirection = Player.position - new Vector3(transform.position.x, Player.position.y, transform.position.z);
        Orientation.forward = LookAtDirection.normalized;
        
        PlayerObj.forward = LookAtDirection.normalized;
    }

    private void SetGunCameraMode()
    {
        BasicCinemachineFreeLook.gameObject.SetActive(false);
        CombatCinemachineFreeLook.gameObject.SetActive(false);
        GunCinemachineFreeLook.gameObject.SetActive(true);
        Vector3 LookAtDirection = Player.position - new Vector3(transform.position.x, Player.position.y, transform.position.z);
        Orientation.forward = LookAtDirection.normalized;
        PlayerObj.forward = LookAtDirection.normalized;
    }

}
