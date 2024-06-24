using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdScript : MonoBehaviour
{
    private Animator mAnimator;

    void Start()
    {
        mAnimator = GetComponent<Animator>(); 
    }

    public void StartDive(){
        mAnimator.SetTrigger("StartDive");
    }

    public void Diving(){
        mAnimator.SetTrigger("Diving");
    }

    public void Swoop(){
        mAnimator.SetTrigger("Swoop"); 

    }

    public void Ascend(){
        mAnimator.SetTrigger("Ascend"); 

    }

    public void EndSwoop(){
        mAnimator.SetTrigger("EndSwoop"); 

    }

    public void EndFlight(){  
        mAnimator.SetTrigger("New State");     
    }
}
