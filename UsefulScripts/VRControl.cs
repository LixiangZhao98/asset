using HTC.UnityPlugin.Vive;
using UnityEngine;


public class VRControl : MonoBehaviour
{
    float movey;
    public Camera cam;
    public float speed;
    void Update()
    {
        movey= ViveInput.GetAxis(HandRole.LeftHand, ControllerAxis.PadY);
        if(movey!=0f)
            this.transform.position+=cam.transform.forward *speed*movey*Time.deltaTime;
    }

}
