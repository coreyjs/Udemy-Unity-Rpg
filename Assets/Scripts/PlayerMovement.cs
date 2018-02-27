using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    bool isInDirectMode = false;

    [SerializeField]
    float walkMoveStopRadius = 0.2f;

    ThirdPersonCharacter thirdPersonCharacterCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;
        
    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacterCharacter = GetComponent<ThirdPersonCharacter>();
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

        Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = v * camForward + h * Camera.main.transform.right;

        thirdPersonCharacterCharacter.Move(movement, false, false);
    }

    private void ProcessIndirectMovement()
    {
        if (Input.GetMouseButton(0))
        {
            print("Cursor raycast hit: " + cameraRaycaster.currentLayerHit);

            switch (cameraRaycaster.currentLayerHit)
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
            thirdPersonCharacterCharacter.Move(playerToClickPoint, false, false);
        }
        else
        {
            thirdPersonCharacterCharacter.Move(Vector3.zero, false, false);
        }
    }
}

