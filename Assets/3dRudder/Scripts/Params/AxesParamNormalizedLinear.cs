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
    * \class    AxesParamNormalizedLinear
    *
    * \brief    The axes parameter used to calculate the axes in Normalized Linear.
    */
    //[CreateAssetMenu(fileName = "NormalizedLinear", menuName = "3dRudder/AxesParamNormalizedLinear", order = 1)]
    public class AxesParamNormalizedLinear : IAxesParam
    {
        /**
        *
        * \brief    Constructor
        *
        *     ¤compatible_plateforme Win, PS4¤
        *    ¤compatible_firm From x.4.x.x¤
        *    ¤compatible_sdk From 2.00¤
        *
        */
        public AxesParamNormalizedLinear() : base()
        {
            SetCurve(Axes.LeftRight, new Curve(1.0f, 0.0f, 1.0f));
            SetCurve(Axes.ForwardBackward, new Curve(1.0f, 0.0f, 1.0f));
            SetCurve(Axes.UpDown, new Curve(1.0f, 0.0f, 1.0f));
            SetCurve(Axes.Rotation, new Curve(1.0f, 0.0f, 1.0f));

            NonSymmetrical = false;
            Roll2YawCompensation = 0.0f;
        }

        /**
        *
        * \brief    Update curves data
        *
        *    ¤compatible_plateforme Win, PS4¤
        *    ¤compatible_firm From x.4.x.x¤
        *    ¤compatible_sdk From 2.00¤
        *
        * \param    nPortNumber The port number.
        *                       .
        * \return   The last error. The possible error code returned by this method. ErrorCode can also be a success. \n \n \n <b> Possible error codes : </b>
        *
        * \possibleret #Success
        * \possibleret #NotInitialized
        * \possibleret #FirmwareNeedToBeUpdated
        * \possibleret #DeviceNotSupported
        * \possibleret #ValidationError
        * \possibleret #DeviceInitError
        * \possibleret #NotConnected
        * \possibleret #NotReady
        * \possibleret #ValidationTimeOut
        * \possibleret #IncorrectCommand
        */
        public override ErrorCode UpdateParam(uint nPortNumber)
        {
            Sdk3dRudder.GetDeviceInformation(nPortNumber);
            return Sdk3dRudder.GetLastError();        
        }
    }
}