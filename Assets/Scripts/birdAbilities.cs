using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdAbilities : MonoBehaviour
{
    private Animator mAnimator;

    public bool abilityActive = false;   
    
    void Start(){
        //mRigidbody = GetComponent<Rigidbody>();
        mAnimator = GetComponent<Animator>();        
    }

    void Update(){
        if(mAnimator != null){
            if(Input.GetKeyDown(KeyCode.DownArrow) && abilityActive == false){
                SetActive();
                mAnimator.SetTrigger("Loop");
                SetInactive();
            }
            if(Input.GetKeyDown(KeyCode.UpArrow) && abilityActive == false){
                SetActive();
                mAnimator.SetTrigger("Corkscrew");
                SetInactive();
            }
            if(Input.GetKeyDown(KeyCode.LeftArrow) && abilityActive == false){
                SetActive();
                mAnimator.SetTrigger("BankLeft");
                SetInactive();
            }
            if(Input.GetKeyDown(KeyCode.RightArrow) && abilityActive == false){
                SetActive();
                mAnimator.SetTrigger("BankRight");
                SetInactive();
            }
        }           
    }

    void SetActive(){ // Enables ability check
        abilityActive = true;
    }

    void SetInactive(){ // Disables ablity check
        abilityActive = false;
    }
}
