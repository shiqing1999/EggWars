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
* \namespace	ns3dRudder
*
*/
namespace ns3dRudder
{
	/**
	 * \class	ILocomotion
	 *
	 * \brief	The interface for locomotion. This allow you to get the axes of the 3dRudder.
	 */
    public abstract class ILocomotion : MonoBehaviour
    {
    	/**
		*
		* \brief Update the axes with a factor (Speed)
		*
		* ¤compatible_plateforme Win, PS4¤
		* ¤compatible_firm From x.4.x.x¤
		* ¤compatible_sdk From 2.00¤
		*
		* \param	controller3dRudder	The access to controller.
		* \param	axesWithFactor	The Unity.Vector4 with (X = Left/Right, Y = Up/Down, Z = Forward/Backward, W = Rotation)
		*
		*/
        public abstract void UpdateAxes(Controller3dRudder controller3dRudder, Vector4 axesWithFactor);
    }
}