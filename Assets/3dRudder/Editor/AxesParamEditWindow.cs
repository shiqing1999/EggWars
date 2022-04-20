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
using System.Linq;
using UnityEngine;
using UnityEditor;

public class AxesParamEditWindow : EditorWindow
{
    Controller3dRudderEditor m_controller3dRudder;
    private static Texture logo;    
    int selectedIndex;
    string newCustomAxesParamName;

    public void Init(Controller3dRudderEditor controller3dRudder)
    {
        m_controller3dRudder = controller3dRudder;
        
        if (logo == null)
            logo = AssetDatabase.LoadAssetAtPath<Texture2D>(Controller3dRudderEditor.PATH_EDITOR + "3dRudderIcon.png");
        titleContent = new GUIContent("Edit config", logo);
        position = new Rect(Screen.width / 2, Screen.height / 2, 200, 150);
        Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Custom Axes Param name : ", EditorStyles.wordWrappedLabel);
        string[] listConfig = m_controller3dRudder.AxesParamList.Except(Controller3dRudderEditor.AxesDefaultParamList).ToArray();
        selectedIndex = EditorGUILayout.Popup(selectedIndex, listConfig.ToArray());

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("New name : ", EditorStyles.wordWrappedLabel);
        newCustomAxesParamName = EditorGUILayout.TextField("", newCustomAxesParamName);

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("OK"))
        {
            if (listConfig.Length != 0)
            {
                newCustomAxesParamName = newCustomAxesParamName.Trim();
                if (Controller3dRudderEditor.AxesDefaultParamList.Where(x => x.ToUpper() == newCustomAxesParamName.ToUpper()).ToArray().Length > 0)
                {
                    EditorUtility.DisplayDialog("Warning", "The name " + newCustomAxesParamName + " is forbidden (Default, Angle and Normalized), change the name please!", "Ok");
                }
                else
                {
                    if (listConfig[selectedIndex] != newCustomAxesParamName)
                    {
                        string path = string.Format(Controller3dRudderEditor.PATH_ASSET, listConfig[selectedIndex]);
                        string newasset = AssetDatabase.RenameAsset(path, newCustomAxesParamName);
                        if (newasset != "")
                        {
                            EditorUtility.DisplayDialog("Warning", newasset, "Ok");
                        }                        
                        else
                        {
                            m_controller3dRudder.EditConfig(listConfig[selectedIndex], newCustomAxesParamName, string.Format(Controller3dRudderEditor.PATH_ASSET, newCustomAxesParamName));
                            Close();
                        }
                    }
                }
            }
            else
            {
                Close();
            }
        }
        
        if (GUILayout.Button("Cancel"))
        {
            Close();
        }

        EditorGUILayout.EndHorizontal();
    }
}
