using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Unity.VisualScripting;

public class PassthroughColorLUTController : MonoBehaviour
{
    [SerializeField]
    private Texture2D _2dColorLUT;
    public Texture2D ColorLUT2D => _2dColorLUT;
    OVRPassthroughColorLut ovrpcl;
    OVRPassthroughLayer passthroughLayer;

    void Start()
    {
        GameObject ovrCameraRig = GameObject.Find("OVRCameraRig");
        if (ovrCameraRig == null)
        {
            Debug.LogError("Scene does not contain an OVRCameraRig");
            return;
        }

        passthroughLayer = ovrCameraRig.GetComponent<OVRPassthroughLayer>();
        if (passthroughLayer == null)
        {
            Debug.LogError("OVRCameraRig does not contain an OVRPassthroughLayer component");
            return;
        }

        ovrpcl = new OVRPassthroughColorLut(_2dColorLUT, false);
        passthroughLayer.SetColorLut(ovrpcl,1);
    }

    void Update()
    {
    }
}