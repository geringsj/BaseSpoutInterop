using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using interop;

public class WhitelistManager : VisParamSender<List<string>>
{
    // "SphereRenderer", "RaycastVolumeRenderer", "BoundingBoxRenderer", "VrInteropView3D_2"
    public List<string> whitelist = new List<string>();
    Parameter<List<string>> whitelistParam = new Parameter<List<string>>();
    public WhitelistManager(string name, bool init) : base(name, init) { }

    // Start is called before the first frame update
    void Start()
    {
        //whitelist = new List<string> {"VrInteropView3D_2" };
    }

    void Update()
    {


        whitelistParam.name = "Whitelist";
        whitelistParam.modulFullName = "ModulWhitelist";
        whitelistParam.param = whitelist;
        //Debug.Log(whitelist);
        // send the whitelist
        Send(whitelistParam);
    }
}