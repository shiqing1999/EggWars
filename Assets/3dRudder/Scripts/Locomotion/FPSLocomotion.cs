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

/**
* \namespace    ns3dRudder
*
*/
namespace ns3dRudder
{
    /**
     * \class   FPSLocomotion
     *
     * \brief   The FPS locomotion. This allow you to move as an FPS game.
     */
    public class FPSLocomotion : ILocomotion
    {
        // Angle per second for the rotation
        public float AnglePerSecond = 90.0f;
        // If translate in local or world direction
        public bool LocalTranslation = true;
        public bool UsePhysics = false;

        protected Vector3 translation;
        protected float angle;
        protected Transform trans;

        // Use this for initialization
        void Start()
        {
            translation = Vector3.zero;
            angle = 0.0f;
            trans = transform;
        }

        // Vector4 X = Left/Right, Y = Up/Down, Z = Forward/Backward, W = Rotation
        public override void UpdateAxes(Controller3dRudder controller3dRudder, Vector4 axesWithFactor)
        {
            // Translation XYZ           
            translation = axesWithFactor;
            // Rotation
            angle = axesWithFactor.w; ;
        }

        // Update is called once per frame
        void Update()
        {
            // Translate
            if (!UsePhysics)
                trans.Translate(translation * Time.deltaTime, LocalTranslation ? Space.Self : Space.World);
            // Rotate
            trans.Rotate(0, angle * AnglePerSecond * Time.deltaTime, 0);
        }

        // Update is called once per fixed frame
        void FixedUpdate()
        {
            // Translate
            if(UsePhysics)
                trans.Translate(translation * Time.deltaTime, LocalTranslation ? Space.Self : Space.World);
        }
    }
}