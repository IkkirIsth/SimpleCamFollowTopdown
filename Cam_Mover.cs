using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_Mover : MonoBehaviour {

    public Transform focusTarget;
    float camSpeed = 100;
    public float followOffsetDistance = 20f;
    public float followOffsetHeight = 30f;
    public float currentRotation = 270f;

    public float offsetDistMax = 35f;
    public float offsetDistMin = 15f;
    public float offsetHeightMax = 50f;
    public float offsetHeightMin = 15f;
    float zoomSpeed = 2f;

    float currentDistHeightFraction = 0.8f;

	void Start ()
    {
        SetOffsetVarsFromZoom(currentDistHeightFraction);
		
	}
	

    public void DoCamMotion(int motionDirection) //change to float if you want to mod by joystick float value instead of base direction
    {
        //Debug.Log("Doing cam motion" + motionDirection);
        CamMotion((float)motionDirection * camSpeed * Time.unscaledDeltaTime);
    }

    public void DoCamZoom(float zoomDirection)
    {
        //Debug.Log("Zooming " + zoomDirection);
        currentDistHeightFraction = Mathf.Clamp(currentDistHeightFraction + (float)zoomDirection * zoomSpeed * Time.unscaledDeltaTime ,0, 1.0f);
        SetOffsetVarsFromZoom(currentDistHeightFraction);
    }

    void CamMotion(float motion)
    {
        currentRotation += motion;
        if (currentRotation >= 360f)
        {
            currentRotation = 0.0f;
        }
        else if (currentRotation < 0.0f)
        {
            currentRotation = 360f;
        }
        SetOffset();

    }

    void SetOffsetVarsFromZoom(float zoomAmt)
    {
        followOffsetDistance = (offsetDistMax - offsetDistMin)  * zoomAmt + offsetDistMin;
        followOffsetHeight = (offsetHeightMax - offsetHeightMin) * zoomAmt + offsetHeightMin;
        SetOffset();
    }

    public void SetOffset()
    {
        if (!focusTarget)
        {
            focusTarget = Game_Controller.instance.pManager.GetPartyLead(); //replace with how you get your players transform follow target
        }
        if (!focusTarget)
        {
            return;
        }
        this.transform.position = focusTarget.transform.position;
        Vector2 unitOffSet = GetUnitRotation();
        unitOffSet.x *= followOffsetDistance;
        unitOffSet.y *= followOffsetDistance;
        Vector3 totalOffset = new Vector3(unitOffSet.x, followOffsetHeight, unitOffSet.y);
        this.transform.position += totalOffset;
        focusTarget.transform.position += new Vector3(0, 10, 0); //adjustment for lookat
        this.transform.LookAt(focusTarget, Vector3.up);
        focusTarget.transform.position -= new Vector3(0, 10, 0);
    }

    public Vector2 GetUnitRotation()
    {
        Vector2 unitOffSet = new Vector2(Mathf.Cos(currentRotation * Mathf.Deg2Rad), Mathf.Sin(currentRotation * Mathf.Deg2Rad));
        return unitOffSet;
    }

    public float GetRotationVar()
    {
        return currentRotation;
    }

    public void SetNewFocusTarget(Transform newTarget)
    {
        focusTarget = newTarget;
    }

    public void ZoomCam(float camZoom)
    {

        SetOffsetVarsFromZoom(currentDistHeightFraction);
    }

}
