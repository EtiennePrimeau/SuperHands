using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PainPointTool : EditorWindow
{
    private enum PainPositionsHand
    { Left, Right }
    
    private enum PainPositionsFinger
    { Index, Middle, Ring, Pinky, Thumb }

    private EnumField _handEnumField;
    private EnumField _fingerEnumField;
    private Button _button;


    [MenuItem("Splendide/Pain Point Tool")]
    public static void OpenWindow()
    {
        GetWindow<PainPointTool>();
    }

    private void CreateGUI()
    {
        
        _handEnumField = new EnumField("Pain Position - Hand", PainPositionsHand.Left);
        rootVisualElement.Add(_handEnumField);

        _handEnumField.RegisterValueChangedCallback(evt =>
        {
            PainPositionsHand selectedValue = (PainPositionsHand)evt.newValue;
        });

        _fingerEnumField = new EnumField("Pain Position - Finger", PainPositionsFinger.Index);
        rootVisualElement.Add(_fingerEnumField);

        _fingerEnumField.RegisterValueChangedCallback(evt =>
        {
            PainPositionsFinger selectedValue = (PainPositionsFinger)evt.newValue;
        });

        _button = new Button();
        _button.text = "Add Pain Point";
        rootVisualElement.Add(_button);

        _button.clicked += AddPainPoint;
    }

    private void AddPainPoint()
    {
        OVRSkeleton.SkeletonType hand;

        switch (_handEnumField.value)
        {
            case PainPositionsHand.Left:
                hand = OVRSkeleton.SkeletonType.HandLeft;
                break;
            case PainPositionsHand.Right:
                hand = OVRSkeleton.SkeletonType.HandRight;
                break;
            default:
                hand = OVRSkeleton.SkeletonType.None;
                break;
        }


        OVRSkeleton.BoneId boneId;
        switch (_fingerEnumField.value)
        {
            case PainPositionsFinger.Index:
                boneId = OVRSkeleton.BoneId.Hand_IndexTip;
                break;
            case PainPositionsFinger.Middle:
                boneId = OVRSkeleton.BoneId.Hand_MiddleTip;
                break;
            case PainPositionsFinger.Ring:
                boneId = OVRSkeleton.BoneId.Hand_RingTip;
                break;
            case PainPositionsFinger.Pinky:
                boneId = OVRSkeleton.BoneId.Hand_PinkyTip;
                break;
            case PainPositionsFinger.Thumb:
                boneId = OVRSkeleton.BoneId.Hand_ThumbTip;
                break;
            default:
                boneId = OVRSkeleton.BoneId.Invalid;
                break;
        }

        //PainPointToolManager.Instance.SpawnPainPoint(hand, boneId);
        PainPointToolSpawner.Instance.SpawnPainPoint(hand, boneId);
        
    }
}
