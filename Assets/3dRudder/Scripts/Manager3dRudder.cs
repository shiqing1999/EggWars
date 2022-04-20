/************************************************************************************

	Copyright © 2014-2019, 3dRudder SA, All rights reserved
    For terms of use: https://3drudder-dev.com/docs/introduction/sdk_licensing_agreement/

	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions are met :

	* Redistributions of source code must retain the above copyright notice, and
	this list of conditions.
	* Redistributions in binary form must reproduce the above copyright
	notice and this list of conditions.
	* The name of 3dRudder may not be used to endorse or promote products derived from
	this software without specific prior written permission.

    Copyright (c) Unity Technologies. For terms of use, see https://unity3d.com/legal/licenses/Unity_Reference_Only_License

************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ns3dRudder;

public class Manager3dRudder : MonoBehaviour
{
    private static Manager3dRudder instance;
    public static Manager3dRudder Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Manager3dRudder>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(Manager3dRudder).Name;
                    obj.hideFlags = HideFlags.HideInHierarchy;
                    instance = obj.AddComponent<Manager3dRudder>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }

    public bool IsInitialized = false;

    // Use this for initialization
    void Awake ()
    {
#if !UNITY_EDITOR
        Sdk3dRudder.Initialize();
#endif
        IsInitialized = true;
    }

    void OnApplicationQuit()
    {
#if !UNITY_EDITOR
        Sdk3dRudder.Stop();
#endif
    }
}
