using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cainos.PixelArtTopDown_Basic;
using UnityEditor;
using JetBrains.Annotations;
using UnityEditor.TerrainTools;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.Port;
using System.Linq;
using UnityEditorInternal;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(TopDownCharacterController))]
class TopDownCharacterControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();

        TopDownCharacterController tdcc = (TopDownCharacterController)target;


        if (GUILayout.Button("Bake"))
        {
            //roomManagement.SendMessage("Bake", null, SendMessageOptions.DontRequireReceiver);
            tdcc.Bake();
            EditorUtility.SetDirty(tdcc);
        }
    }
}

[CustomEditor(typeof(RoomManagement))]
public class RoomManagementEditor : Editor
{
    public override void OnInspectorGUI()
    {


        base.OnInspectorGUI();

        RoomManagement roomManagement = (RoomManagement)target;

        EditorGUI.BeginChangeCheck();


        if (GUILayout.Button("Bake"))
        {
            //roomManagement.SendMessage("Bake", null, SendMessageOptions.DontRequireReceiver);
            roomManagement.Bake();
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(roomManagement);
        }
    }
}

[CustomEditor(typeof(ItemObjectInfo))]
public class ItemObjectInfoEditor : Editor
{
    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();

        ItemObjectInfo itemObjectInfo = (ItemObjectInfo)target;

        if (GUILayout.Button("Bake"))
        {
            //roomManagement.SendMessage("Bake", null, SendMessageOptions.DontRequireReceiver);
            itemObjectInfo.Bake();
        }
    }
}


[CustomEditor(typeof(BoxInfo))]
public class BoxInfoEditor : Editor
{
    bool _foldout = false;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BoxInfo _target = (BoxInfo)target;

        EditorGUI.BeginChangeCheck();

        switch (_target.boxType)
        {
        
            case BoxInfo.BoxType.item :
                _foldout = EditorGUILayout.Foldout(_foldout,"ItemInBox");
                if (_foldout)
                {

                    _target.itemInBox.ItemStorageGet.It = (Item)EditorGUILayout.ObjectField("Item", _target.itemInBox.ItemStorageGet.It, typeof(Item),true);
                    _target.itemInBox.ItemStorageGet.ItemNum = EditorGUILayout.IntField("ItemNum",(int)_target.itemInBox.ItemStorageGet.ItemNum);
                    
                }
                break;
            case BoxInfo.BoxType.tool:
                _foldout = EditorGUILayout.Foldout(_foldout, "ToolInBox");
                if (_foldout)
                {
                    _target.toolInBox.ToolGet = (Tool)EditorGUILayout.ObjectField("Tool", _target.toolInBox.ToolGet, typeof(Tool), true);
                }
                break;
            case BoxInfo.BoxType.heal:
                _foldout = EditorGUILayout.Foldout(_foldout, "HealInBox");
                if (_foldout)
                {
                    _target.healNum = EditorGUILayout.IntField("HealNum", _target.healNum);
                    _target.healSprite = (Sprite)EditorGUILayout.ObjectField("HealSprite",_target.healSprite,typeof(Sprite),true);
                }
                break;
        }

        if (GUILayout.Button("Bake"))
        {
            _target.Bake();
        }



        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(_target);
        }

    }
}
/*
if (GUILayout.Button("Bake"))
        {
            EditorUtility.SetDirty(_target);
        }
*/

[CustomEditor (typeof(ToolInfo))]
public class ToolInfoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        base.OnInspectorGUI();

        ToolInfo _target = (ToolInfo)target;


        if ( GUILayout.Button("Bake"))
        {
            _target.Bake();
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(_target);
        }
    }
}


[CustomEditor(typeof(HandrailInfo))]
public class HandrailInfoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        base.OnInspectorGUI();

        HandrailInfo _target = (HandrailInfo)target;


        if (GUILayout.Button("Bake"))
        {
            _target.Bake();
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(_target);
            
        }
    }
}

[CustomEditor(typeof(HandrailTriggerInfo))]
public class HandrailTriggerInfoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        base.OnInspectorGUI();

        HandrailTriggerInfo _target = (HandrailTriggerInfo)target;


        /*if (GUILayout.Button("Bake"))
        {
            _target.Bake();
        }*/

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(_target);

        }
    }
}