using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TargetScreenCoords : MonoBehaviour
{
    //private ImageTargetBehaviour mImageTargetBehaviour = null;
    private Vector3 velocity = new Vector3(0, 0, 0);
    private Vector3 previous = new Vector3(0, 0, 0);
    private float time=0.0f;
    private float v=0;
    // public string text = "sravya";
    void Start()
    {
        
    }
    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width - 200, 0, 200, Screen.height - 40), System.Convert.ToString(v));
    }

    // Use this for initialization

    // Update is called once per frame
    void Update()
    {

        // We define a point in the target local reference 
        // we take the bottom-left corner of the target, 
        // just as an example
        // Note: the target reference plane in Unity is X-Z, 
        // while Y is the normal direction to the target plane
        // time = Time.deltaTime;
        if (time < 1)
            time += Time.deltaTime;
        else
        {
            time = 0;
            Vector3 pointOnTarget = new Vector3(0.5f, 0, 0.5f);

            // We convert the local point to world coordinates
            Vector3 targetPointInWorldRef = this.transform.TransformPoint(pointOnTarget);


            // We project the world coordinates to screen coords (pixels)
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(targetPointInWorldRef);

            velocity.x = (float)(screenPoint.x - previous.x) / 1;
            velocity.y = (float)(screenPoint.y - previous.y) / 1;
            velocity.z = (float)(screenPoint.z - previous.z) / 1;
            // float v = Mathf.Sqrt(velocity.x*velocity.x+)
            v = velocity.magnitude;


            Debug.Log("target point in screen coords: " + screenPoint);
            Debug.Log("Distance moved is: " + (screenPoint - previous));
            Debug.Log("velocity is " + v);
            previous = screenPoint;
        }
    }
}
