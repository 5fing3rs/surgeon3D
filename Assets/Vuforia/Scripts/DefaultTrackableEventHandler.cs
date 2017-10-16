/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using System;

namespace Vuforia
{
    /// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    public class DefaultTrackableEventHandler : MonoBehaviour,
                                                ITrackableEventHandler
    {
        #region PRIVATE_MEMBER_VARIABLES
 
        private TrackableBehaviour mTrackableBehaviour;
        private DateTime startDate=DateTime.Now;
        private Vector3 oldPosn;
        private bool firstTime = false;
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
            }
            else
            {
                OnTrackingLost();
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
            // Update();
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
        }

        // Update is called once per frame
        void Update () {
            // Get the position of one of the corners of the image target
            // For instance, let's get the top-right corner (+X, +Z)
            Vector3 cornerInLocalRef = new Vector3(0.5f, 0, 0.5f);
             
            // Convert from local ref to world ref
            Vector3 cornerInWorldRef = this.transform.TransformPoint(cornerInLocalRef);
             
            // Convert from world ref to camera ref
            Vector3 cornerInCameraRef = Camera.main.transform.InverseTransformPoint(cornerInWorldRef);
             
            // Debug.Log ("Top-right target corner in world ref: " + cornerInWorldRef);
            Debug.Log ("Top-right target corner in camera ref: " + cornerInCameraRef);
            if(!firstTime){
                    float translation = Time.deltaTime;
                    float disp = (cornerInCameraRef-oldPosn).magnitude;
                    float velocity = (float)disp/(float)translation;
                    Debug.Log("Velocity is: " +  Convert.ToString(velocity));   
            }
            else{
                firstTime = false;
                Debug.Log("Velocity is: 0");
            }
            oldPosn = cornerInCameraRef;
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
            // Update();
            firstTime = true;
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            Debug.Log("Velocity is 0");
        }

        #endregion // PRIVATE_METHODS
    }
}
