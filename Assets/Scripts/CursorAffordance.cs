using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraRaycaster))]
public class CursorAffordance : MonoBehaviour 
{

    [SerializeField]
    Texture2D walkCursor = null;

    [SerializeField]
    Texture2D targetCursor = null;

    [SerializeField]
    Texture2D unknownCursor = null;

    [SerializeField]
    Vector2 cursorHotspot = new Vector2(0, 0);

    [SerializeField] const int walkableLayerNumber = 8;
    [SerializeField] const int enemyLayerNumber = 9;


    CameraRaycaster cameraRayCaster;

	// Use this for initialization
	void Start () 
    {
        cameraRayCaster = GetComponent<CameraRaycaster>();
        if (cameraRayCaster != null)
        {
            cameraRayCaster.notifyLayerChangeObservers += OnLayerChanged;
        }
        
    }

    // Update is called once per frame
    void OnLayerChanged(int newLayer)
    {
        print("Cursor over new layer");

        if (cameraRayCaster == null) return;
        if (cameraRayCaster != null)
        {
            switch (newLayer)
            {
                case walkableLayerNumber:
                    Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                    break;
                case enemyLayerNumber:
                    Cursor.SetCursor(targetCursor, cursorHotspot, CursorMode.Auto);
                    break;
                default:
                    Cursor.SetCursor(unknownCursor, cursorHotspot, CursorMode.Auto);
                    break;
            }
        }
    }

}
