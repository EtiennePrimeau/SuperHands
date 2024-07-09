using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class PainPointInteractions : MonoBehaviour
{
    [SerializeField] private HandGrabInteractable _handGrabInteractable;
    [SerializeField] private TagSetFilter _tagSetFilter;

    private OVRSkeleton.SkeletonType _attachedHand;
    private HandGrabInteractor _validInteractor;

    public void Init(OVRSkeleton.SkeletonType hand, HandGrabInteractor oppositeSideHandInteractor)
    {
        _attachedHand = hand;
        _validInteractor = oppositeSideHandInteractor;

        //makes the painpoint not grabbable by the same hand
        //Adds the filter corresponding to the opposite hand that it is attached to
        string[] str = new string[1];
        if (hand == OVRSkeleton.SkeletonType.HandRight)
        {
            str[0] = "LeftHandInteractor";
        }
        else
        {
            str[0] = "RightHandInteractor";
        }
        _tagSetFilter.InjectOptionalRequireTags(str);
    }

    public virtual void OnSelect()
    {
        //Debug.Log("OnSelect");

        //Makes the pain point grabbable by both hands again
        //just resets the filter assigned to _handGrabInteractable
        _handGrabInteractable.InjectOptionalInteractorFilters(new List<IGameObjectFilter>());


        transform.SetParent(null);
    }


}
