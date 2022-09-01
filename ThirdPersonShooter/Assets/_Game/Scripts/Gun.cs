using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    [SerializeField] AmmoType ammoType;
    [SerializeField] float FireRange;
    [SerializeField] float Damage;
    
// Getters 
    public AmmoType GetGunAmmoType()
    {
        return ammoType;
    }
//Setters

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
