using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorAffordance : MonoBehaviour 
{

    [SerializeField]
    Texture2D walkCursor = null;

    [SerializeField]
    Vector2 cursorHotspot = new Vector2(96, 96);

    CameraRaycaster cameraRayCaster;

	// Use this for initialization
	void Start () 
    {
        cameraRayCaster = GetComponent<CameraRaycaster>();	
	}
	
	// Update is called once per frame
	void Update () 
    {
        Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
    }
}
