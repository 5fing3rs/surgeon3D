using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Intel.RealSense;
using Intel.RealSense.Hand;

public class GesturalManager : Publisher {

	public Text gestureText;
	public Text debugText;
	public bool isEnableAllGesture;
    public string[] gestureActions;

    HandManager handManager;

    string TAG = "Gestural Manager : ";

	// Update is called once per frame
	void Update () {
        UpdateHandGesture();
    }

    // Enable Gesture
    public void EnableGestures()
    {
        handManager = GetComponent<HandManager>();
        if (isEnableAllGesture)
        {
            Debug.Log(TAG + "All Gesture Enabled!");
            handManager.handConfiguration.EnableAllGestures();
        }else
        {
            foreach (string gesture in gestureActions)
            {
                Debug.Log(TAG + gesture + " Enabled!");
                handManager.handConfiguration.EnableGesture(gesture, true);
            }
        }
        handManager.handConfiguration.ApplyChanges();
        GetComponent<DepthCameraManger>().StartDevice();
    }

    // Update Gesture
    public void UpdateHandGesture()
    {
        if (GetComponent<DepthCameraManger>().isStart)
            if (handManager.handData != null)
                if (handManager.handData.FiredGestureData != null)
                {
                    foreach (GestureData gesture in handManager.handData.FiredGestureData)
                    {
                        string camelCaseGestureName = CreateCamelCase(gesture.name);
						debugText.text =gesture.ToString();
						if (GetHandSide(gesture).ToString() == "BODY_SIDE_LEFT")
                        {
                            gestureText.text = "Left Hand " + camelCaseGestureName;
                        }
                        else if (GetHandSide(gesture).ToString() == "BODY_SIDE_RIGHT")
                        {
                            gestureText.text = "Right Hand" + camelCaseGestureName;
						}

                        gestureText.text = "Hand " + camelCaseGestureName;
                        Boardcast("OnGesture", gesture);
                    }
                }
    }

    // Create Camel Case String
    string CreateCamelCase(string normalString)
    {
        string[] arrStr = normalString.Split('_');
        for (int i = 0;i<arrStr.Length;i++)
            arrStr[i] = (arrStr[i][0] + "").ToUpper() + arrStr[i].Substring(1);
        return string.Join("", arrStr);
    }

    // Get Hand Side from gesture
    public BodySideType GetHandSide(GestureData gesture)
    {
        IHand hand;
        if (GetComponent<HandManager>().handData.QueryHandDataById(gesture.handId, out hand) == Status.STATUS_NO_ERROR)
        {
            return hand.BodySide;
        }
        return BodySideType.BODY_SIDE_UNKNOWN;
    }

}