﻿
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ADBRuntime
{
    [CustomEditor(typeof(ADBRuntimeController))]
    public class ADBRuntimeControllerEditor : Editor
    //OYM：它的编辑器，我觉得我有必要把一部分方法写到里面去
    {

        ADBRuntimeController controller;
        public void OnEnable()
        {
            controller = target as ADBRuntimeController;
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //OYM：更新表现形式

            ADBRuntimeController controller = target as ADBRuntimeController;
            GUILayout.Space(8);
            EditorGUILayout.TextField("Name", controller.transform.name);

            Titlebar("Generate setting", new Color(0.7f, 1.0f, 0.7f));
            EditorGUILayout.LabelField("=============== Point");
            controller.generateTransform = (Transform)EditorGUILayout.ObjectField(new GUIContent("parent Transform"), controller.generateTransform, typeof(Transform), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("generateKeyWordWhiteList"), new GUIContent("Name KeyWord"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("blackListOfGenerateTransform"), new GUIContent("Transform BlackList"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("generateKeyWordBlackList"), new GUIContent("Name Key Word BlackList"), true);

            if (GUILayout.Button("Generate Point", GUILayout.Height(22.0f)))
            {
                controller.initializePoint();
            }
            if (GUILayout.Button("Reset All Point", GUILayout.Height(22.0f)))
            {
                controller.RestorePoint();
            }
            if (controller.allPointTrans != null)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("allPointTrans"), new GUIContent("Root point list"), true);
                GUILayout.Space(5);
            }
            EditorGUILayout.LabelField("=============== Collider");
            if (GUILayout.Button("Generate Collider", GUILayout.Height(22.0f)))
            {
                controller.initializeCollider(true, controller.allPointTrans);
            }
            if (GUILayout.Button("Remove All Collider", GUILayout.Height(22.0f)))
            {
                if (controller.editorColliderList == null) return;
                for (int i = 0; i < controller.editorColliderList.Count; i++)
                {
                    DestroyImmediate(controller.editorColliderList[i]);
                }
                controller.editorColliderList = null;
            }

            controller.isGenerateColliderAutomaitc = EditorGUILayout.Toggle("Is Generate Body Collider Automatic ", controller.isGenerateColliderAutomaitc);

            if(controller.editorColliderList!=null)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("editorColliderList"), new GUIContent("Collider"), true);

            Titlebar("physical setting", new Color(0.7f, 1.0f, 0.7f));
            controller. isDebug = EditorGUILayout.Toggle("isDebug", controller.isDebug);
            if (controller.isDebug)
            {
                controller.iteration = EditorGUILayout.IntSlider("Iterations number", controller.iteration, 1, 1024);
            }
            else
            {
                controller.iteration = EditorGUILayout.IntSlider("Iterations number", controller.iteration, 4, 256);
            }
            controller.delayTime = EditorGUILayout.FloatField("delayTime", controller.delayTime);
            controller.windScale=EditorGUILayout.Slider("windForcePower",controller.windScale, 0, 1); 
            controller.colliderCollisionType= (ColliderCollisionType)EditorGUILayout.EnumPopup("Collision Quantity",controller.colliderCollisionType);
           serializedObject.ApplyModifiedProperties();
        }

        void Titlebar(string text, Color color)
        {
            GUILayout.Space(12);

            var backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = color;

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label(text);
            EditorGUILayout.EndHorizontal();

            GUI.backgroundColor = backgroundColor;

            GUILayout.Space(3);
        }
    }
}