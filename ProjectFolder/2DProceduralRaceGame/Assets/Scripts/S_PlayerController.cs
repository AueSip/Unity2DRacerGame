using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_PlayerController : MonoBehaviour
{
    private float Speed = 0;
    private float CurrentAcceleration = 0;

    public Transform mini;
    int inputDirection = 1;
    int rotDirection = 1;
    float maxSpeed = 0.0085f;

    float speedScale = 1;

    float driftSpeedScale = 0.67f;
    float CarRotation = 0;
    float reverseSpeed = -0.0035f;
    float Acceleration = 0.00015f;
    float minAcceleration = -1f;
    float maxAcceleration = 1f;
    //S = D/T
    float timeToAccelerate = 5f;

    float timeToSlow = 250f;
    float fric_Handling = 0.4f;
    float turn_Rate = 40000;

    float currentMiniRotZ = 0f;
    float driftRotationRate = 150f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(Acceleration);
        Debug.Log(fric_Handling);
        Debug.Log(turn_Rate);
        mini = this.gameObject.transform.GetChild(0);
    }

    float SpeedCalculation()
    {
        CurrentAcceleration += (Acceleration / timeToAccelerate * Time.deltaTime) * inputDirection;
        return CurrentAcceleration = Mathf.Clamp(CurrentAcceleration, minAcceleration, maxAcceleration);
    }

    float BreakCalculation()
    {
        CurrentAcceleration = 0;
        Speed = Mathf.Clamp(Speed + (fric_Handling / timeToSlow * Time.deltaTime) * -1, 0, maxSpeed);
        return Speed;
    }
    float SetSpeed(float spd)
    {
        Speed = Mathf.Clamp(spd, reverseSpeed, maxSpeed);
        return Speed;
    }

    float GetSpeed()
    {
        return Speed;
    }

    float RotationRate()
    {
        return Speed * (rotDirection * (turn_Rate * (fric_Handling)));
    }

    float IsDrifting()
    {

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentMiniRotZ = (currentMiniRotZ + (driftRotationRate * rotDirection) * Time.deltaTime);
            currentMiniRotZ = Mathf.Clamp(currentMiniRotZ, -45, 45);
            mini.transform.localRotation = Quaternion.Euler(0, 0, currentMiniRotZ);
            speedScale = driftSpeedScale;
            return 1.05f;
        }
        mini.transform.localRotation = Quaternion.Euler(0, 0, 0);
        speedScale = 1;
        currentMiniRotZ = 0;
        return 1;
        
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKey(KeyCode.W))
        {
            inputDirection = 1;
            SpeedCalculation();
            Debug.Log(Speed);
        }
        if (Input.GetKey(KeyCode.S))
        {

            inputDirection = -1;
            SpeedCalculation();

        }

        if (!Input.GetKey(KeyCode.W) && (!Input.GetKey(KeyCode.S)))
        {
            BreakCalculation();
        }

        if (Input.GetKey(KeyCode.A))
        {

            rotDirection = 1;

            Vector3 newVec = new Vector3(0f, 0f, RotationRate() *  IsDrifting());
            Debug.Log(RotationRate());
            IsDrifting();
            transform.Rotate(newVec * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.D))
        {

            rotDirection = -1;
            Vector3 newVec = new Vector3(0f, 0f, RotationRate() * IsDrifting());
            Debug.Log(RotationRate());
           
            transform.Rotate(newVec * Time.deltaTime);



        }

        

        transform.Translate(Vector3.up * SetSpeed((GetSpeed() + CurrentAcceleration)) * speedScale);


        
       
    }
}
