using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    bool isInDirectMode = false;

    [SerializeField]
    float walkMoveStopRadius = 0.2f;

    ThirdPersonCharacter m_Character;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;
        
    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        m_Character = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        // TODO Allow Player to map later
        if(Input.GetKeyDown(KeyCode.G)) // g for gamepad 
        {
            isInDirectMode = !isInDirectMode;
            currentClickTarget = transform.position; // clear the click target
        }

        if (isInDirectMode)
        {
            ProcessDirectMovement();
        }
        else
        {
            ProcessIndirectMovement(); // Mouse Movement    
        }
    }

    private void ProcessDirectMovement()
    {
        print("Direct Movement Enabled");
        // read inputs
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        print(h + v);
        // calculate camera relative direction to move:

        Vector3 m_CamForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 m_Move = v * m_CamForward + h * Camera.main.transform.right;

        m_Character.Move(m_Move, false, false);
    }

    private void ProcessIndirectMovement()
    {
        if (Input.GetMouseButton(0))
        {
            print("Cursor raycast hit: " + cameraRaycaster.LayerHit);

            switch (cameraRaycaster.LayerHit)
            {
                case Layer.Enemy:
                    print("Not Moving to enemy");
                    break;
                case Layer.Walkable:
                    currentClickTarget = cameraRaycaster.Hit.point;
                    break;
                default:
                    print("SHOULDNT BE HERE");
                    return;
            }
        }

        var playerToClickPoint = currentClickTarget - transform.position;
        if (playerToClickPoint.magnitude >= walkMoveStopRadius)
        {
            m_Character.Move(playerToClickPoint, false, false);
        }
        else
        {
            m_Character.Move(Vector3.zero, false, false);
        }
    }
}

