using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    bool isInDirectMode = false;

    [SerializeField]
    float walkMoveStopRadius = 0.2f;

    [SerializeField]
    float attackMoveStopRadius = 5f;

    ThirdPersonCharacter thirdPersonCharacterCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentDestination, clickPoint;
        
    

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacterCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        // TODO Allow Player to map later
        if(Input.GetKeyDown(KeyCode.G)) // g for gamepad 
        {
            isInDirectMode = !isInDirectMode;
            currentDestination = transform.position; // clear the click target
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
            clickPoint = cameraRaycaster.Hit.point;
            switch (cameraRaycaster.currentLayerHit)
            {
                case Layer.Enemy:
                    currentDestination = ShortDestination(clickPoint, attackMoveStopRadius);
                    break;
                case Layer.Walkable:
                    currentDestination = ShortDestination(clickPoint, walkMoveStopRadius);
                    break;
                default:
                    print("SHOULDNT BE HERE");
                    return;
            }
        }

        WalkToDestination();
    }

    private void WalkToDestination()
    {
        var playerToClickPoint = currentDestination - transform.position;
        if (playerToClickPoint.magnitude >= 0)
        {
            thirdPersonCharacterCharacter.Move(playerToClickPoint, false, false);
        }
        else
        {
            thirdPersonCharacterCharacter.Move(Vector3.zero, false, false);
        }
    }

    Vector3 ShortDestination(Vector3 destination, float shortening)
    {
        var reductionVector = (destination - transform.position).normalized * shortening;
        return destination - reductionVector;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, currentDestination);
        Gizmos.DrawSphere(currentDestination, 0.1f);
        Gizmos.DrawSphere(clickPoint, 0.15f);

        Gizmos.color = new Color(255f, 0f, 0f, .5f);
        Gizmos.DrawSphere(transform.position, attackMoveStopRadius);
    }
}

