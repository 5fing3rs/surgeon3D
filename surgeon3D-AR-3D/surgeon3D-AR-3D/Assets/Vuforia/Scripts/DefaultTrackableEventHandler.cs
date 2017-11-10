/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;


namespace Vuforia
{
    /// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    
    public class DefaultTrackableEventHandler : MonoBehaviour,
                                                ITrackableEventHandler
    {
        #region PRIVATE_MEMBER_VARIABLES

        public TrackableBehaviour mTrackableBehaviour;
        public int flag;
        public int flag_calculat;
        public int flag_remote;
        private Vector3 velocity = new Vector3(0, 0, 0);
        private Vector3 previous = new Vector3(0, 0, 0);
        private float time;
        private float v;
        private float angle;
        private Vector3 orientation_remote = new Vector3(0, 0, 0);
        private Vector3 orientation_remote2 = new Vector3(0, 0, 0);
        private Vector3 orientation_remote3 = new Vector3(0, 0, 0);
        private Vector3 screenPoint = new Vector3(0, 0, 0);
        public DefaultTrackableEventHandler1 calculator;
        #endregion // PRIVATE_MEMBER_VARIABLES

        #region UNTIY_MONOBEHAVIOUR_METHODS

        void Start()
        {
            GameObject calc = GameObject.Find("Calculator");
            calculator = calc.GetComponent<DefaultTrackableEventHandler1>();
            flag_calculat = calculator.flag_calculator;
            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }
        }

        #endregion // UNTIY_MONOBEHAVIOUR_METHODS



        #region PUBLIC_METHODS

        /// <summary>
        /// Implementation of the ITrackableEventHandler function called when the
        /// tracking state changes.
        /// </summary>
        public void OnTrackableStateChanged(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                OnTrackingFound();
                flag= 1;
            }
            else
            {
                OnTrackingLost();
               flag = 0;
            }
        }

        #endregion // PUBLIC_METHODS



        #region PRIVATE_METHODS


        private void OnTrackingFound()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = true;
            }

            // Enable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = true;
            }
          //  if (mTrackableBehaviour.TrackableName == "Calculator")
            //    flag_calculator = 1;
            if (mTrackableBehaviour.TrackableName == "remote")
                flag_remote = 1;
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
        }


        private void OnTrackingLost()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Disable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = false;
            }

            // Disable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = false;
            }
            //if (mTrackableBehaviour.TrackableName == "Calculator")
              //  flag_calculator = 0;
            if (mTrackableBehaviour.TrackableName == "remote")
                flag_remote = 0;
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            //mTrackableBehaviour.transform()
        }

        void OnGUI()
        {
            //if (mTrackableBehaviour.TrackableName == "Calculator")
            //  GUI.Label(new Rect(Screen.width - 200, 0, 200, Screen.height - 40), "Velociy of " + mTrackableBehaviour.TrackableName + " is " + v);
            if (mTrackableBehaviour.TrackableName == "remote")
            {
                GUI.Label(new Rect(Screen.width - 500, 100, 300, Screen.height - 80), "Position of " + mTrackableBehaviour.TrackableName + " is " + screenPoint);
                GUI.Label(new Rect(Screen.width - 500, 120, 300, Screen.height - 90), "Velociy of " + mTrackableBehaviour.TrackableName + " is " + v);
                if (calculator.flag_calculator == 1 && flag_remote == 1)
                {
                    GUI.Label(new Rect(Screen.width - 500, 140, 300, Screen.height - 100), "Angle between calculator and remote is  " + angle);
                }
            }
        }

        void Update()
        {
            if (flag ==1)
            {
                if (time < 1)
                    time += Time.deltaTime;

                // We define a point in the target local reference 
                // we take the bottom-left corner of the target, 
                // just as an example
                // Note: the target reference plane in Unity is X-Z, 
                // while Y is the normal direction to the target plane
                // time = Time.deltaTime;
                else
                {
                    time = 0;
                    Vector3 pointOnTarget = new Vector3(0.5f, 0, 0.5f);
                    Vector3 pointOnTarget2 = new Vector3(-0.5f, 0, 0.5f);
                    Vector3 pointOnTarget3 = new Vector3(0.5f, 0, -0.5f);

                    // We convert the local point to world coordinates
                    Vector3 targetPointInWorldRef = mTrackableBehaviour.transform.TransformPoint(pointOnTarget);
                    Vector3 targetPointInWorldRef2 = mTrackableBehaviour.transform.TransformPoint(pointOnTarget2);
                    Vector3 targetPointInWorldRef3 = mTrackableBehaviour.transform.TransformPoint(pointOnTarget3);

                    // We project the world coordinates to screen coords (pixels)
                    screenPoint = Camera.main.WorldToScreenPoint(targetPointInWorldRef);
                    Vector3 screenPoint2 = Camera.main.WorldToScreenPoint(targetPointInWorldRef2);
                    Vector3 screenPoint3 = Camera.main.WorldToScreenPoint(targetPointInWorldRef3);

                    velocity.x = (float)(screenPoint.x - previous.x) / 1;
                    velocity.y = (float)(screenPoint.y - previous.y) / 1;
                    velocity.z = (float)(screenPoint.z - previous.z) / 1;
                    // float v = Mathf.Sqrt(velocity.x*velocity.x+)
                    v = velocity.magnitude;


                    Debug.Log("Target point in screen coords of : " + mTrackableBehaviour.TrackableName +screenPoint);
                    Debug.Log("Distance traversed by  " + mTrackableBehaviour.TrackableName+ " is  "  + (screenPoint - previous));
                    Debug.Log("Velocity of  " + mTrackableBehaviour.TrackableName + " is " + v);
                    previous = screenPoint;
                    if(mTrackableBehaviour.TrackableName == "remote")
                    {
                        orientation_remote = screenPoint;
                        orientation_remote2 = screenPoint2;
                        orientation_remote3 = screenPoint3;
                    }
                    Debug.Log("Flag of calculator is " + flag_calculat);
                    if(calculator.flag_calculator==1 && flag_remote==1)
                    {
                        //float val1 = orientation_remote.magnitude;
                        //float val2 = calculator.orientation_calculator.magnitude;
                        //float numerator = orientation_remote.x * calculator.orientation_calculator.x + orientation_remote.y * calculator.orientation_calculator.y + orientation_remote.z * calculator.orientation_calculator.z;
                        //float value = numerator / (val1 * val2);
                        //angle = Mathf.Acos(value);
                        //angle =  (float) (angle * 180) /  (float) Mathf.PI;
                        //Debug.Log("Angle is " + angle);
                        var dir = Vector3.Cross(orientation_remote2 - orientation_remote, orientation_remote3 - orientation_remote);
                        var norm1 = Vector3.Normalize(dir);
                        var dir2 = Vector3.Cross(calculator.orientation_calculator2 - calculator.orientation_calculator, calculator.orientation_calculator3 - calculator.orientation_calculator);
                        var norm2 = Vector3.Normalize(dir2);
                        angle = Mathf.Acos(Vector3.Dot(norm1, norm2));
                        angle = (float)(angle * 180) / (float)Mathf.PI;
                        Debug.Log("Angle is " + angle);
                    }

                }
            }
            else
            {
                // Debug.Log(mTrackableBehaviour.TrackableName + "Not being tracked");
                screenPoint.x = 0;
                screenPoint.y = 0;
                screenPoint.z = 0;
                v = 0;
            }
        }
    }

        #endregion // PRIVATE_METHODS
    }
