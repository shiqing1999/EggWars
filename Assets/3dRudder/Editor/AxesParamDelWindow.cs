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
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class AxesParamDelWindow : EditorWindow
{
    Controller3dRudderEditor m_controller3dRudder;
    private static Texture logo; 
    int selectedIndex;

    public void Init(Controller3dRudderEditor controller3dRudder)
    {
        m_controller3dRudder = controller3dRudder;       
        if (logo == null)
            logo = AssetDatabase.LoadAssetAtPath<Texture2D>(Controller3dRudderEditor.PATH_EDITOR + "3dRudderIcon.png");
        titleContent = new GUIContent("Delete config", logo);
        position = new Rect(Screen.width / 2, Screen.height / 2, 200, 150);
        Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Custom Axes Param name : ", EditorStyles.wordWrappedLabel);
        
        string[] listConfig = m_controller3dRudder.AxesParamList.Except(Controller3dRudderEditor.AxesDefaultParamList).ToArray();
        selectedIndex = EditorGUILayout.Popup(selectedIndex, listConfig);
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("OK"))
        {
            if (listConfig.Length != 0)
            {
                m_controller3dRudder.RemoveConfig(listConfig[selectedIndex]);
            }

            Close();
        }
        if (GUILayout.Button("Cancel"))
        {
            Close();
        }

        EditorGUILayout.EndHorizontal();
    }
}
