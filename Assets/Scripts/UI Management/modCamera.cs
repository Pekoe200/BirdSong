using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class modCamera : MonoBehaviour
{
    public static modCamera Instance { get; private set; }

    public GameObject bird;
    public float baseFOV = 60f;
    public float deltaFOV = 10f;
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    public float minFOV = 60f; // Minimum field of view
    public float midFOV = 80f; // Mid-level field of view
    public float maxFOV = 90f; // Maximum field of view

    public float lowSpeedThreshold = 5f; // Speed threshold for mid-level FOV
    public float highSpeedThreshold = 20f; // Speed threshold for maximum FOV

    public float smoothSpeed = 2f; // Smoothing factor for FOV transition
    private float targetFOV; // Target field of view for smooth transition

    private void Awake()
    {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        targetFOV = cinemachineVirtualCamera.m_Lens.FieldOfView;
    }

    private void Update()
    {
        // Update the camera's FOV smoothly towards the target FOV
        cinemachineVirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.FieldOfView, targetFOV, smoothSpeed * Time.deltaTime);
        
        // Update the camera position and FOV based on bird's velocity
        if (bird != null)
        {
            UpdateCameraFOVBasedOnSpeed();
        }
    }

    public void ShakeCamera(float intensity)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
    }

    public void ZoomCamera(float force)
    {
        targetFOV = baseFOV - (force * deltaFOV);
        Debug.Log("Shaking with force: " + targetFOV);
    }

    private void UpdateCameraFOVBasedOnSpeed()
    {
        float birdSpeed = bird.GetComponent<Rigidbody2D>().velocity.magnitude;
        bool birdCharge = bird.GetComponent<birdController>().isCharging;

        if(!birdCharge){
            if (birdSpeed > highSpeedThreshold)
            {
                targetFOV = maxFOV;
            }
            else if (birdSpeed > lowSpeedThreshold)
            {
                targetFOV = midFOV;
            }
            else
            {
                targetFOV = minFOV;
            }
        }

        
    }
}
