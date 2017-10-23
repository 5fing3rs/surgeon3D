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
        private Vector3 velocity = new Vector3(0, 0, 0);
        private Vector3 previous = new Vector3(0, 0, 0);
        private float time;
        private float v;
        #endregion // PRIVATE_MEMBER_VARIABLES



        #region UNTIY_MONOBEHAVIOUR_METHODS

        void Start()
        {
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
                flag = 1;
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

            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            //mTrackableBehaviour.transform()
        }

        void OnGUI()
        {
            if(mTrackableBehaviour.TrackableName == "calculator")
                GUI.Label(new Rect(Screen.width - 200, 0, 200, Screen.height - 40), "Velociy of " + mTrackableBehaviour.TrackableName + " is "+ v );
            else if(mTrackableBehaviour.TrackableName == "remote")
                GUI.Label(new Rect(Screen.width - 400, 50, 100, Screen.height - 80), "Velociy of " + mTrackableBehaviour.TrackableName + " is " + v);

        }

        void Update()
        {
            if (flag == 1)
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

                    // We convert the local point to world coordinates
                    Vector3 targetPointInWorldRef = mTrackableBehaviour.transform.TransformPoint(pointOnTarget);


                    // We project the world coordinates to screen coords (pixels)
                    Vector3 screenPoint = Camera.main.WorldToScreenPoint(targetPointInWorldRef);

                    velocity.x = (float)(screenPoint.x - previous.x) / 1;
                    velocity.y = (float)(screenPoint.y - previous.y) / 1;
                    velocity.z = (float)(screenPoint.z - previous.z) / 1;
                    // float v = Mathf.Sqrt(velocity.x*velocity.x+)
                    v = velocity.magnitude;


                    Debug.Log("target point in screen coords of : " + mTrackableBehaviour.TrackableName + screenPoint);
                    Debug.Log("Distance traversed by  " + mTrackableBehaviour.TrackableName+ " is  "  + (screenPoint - previous));
                    Debug.Log("velocity of  " + mTrackableBehaviour.TrackableName  + " is "+ v);
                    previous = screenPoint;
                }
            }
            else
            {
                 Debug.Log(mTrackableBehaviour.TrackableName + "Not being tracked");
            }
        }
    }

        #endregion // PRIVATE_METHODS
    }
