
using UnityEngine;

public class NewReaction : DelayedReaction
{
    public string debugMsg = "";
    protected override void SpecificInit ()
    {
        //triggerHash = Animator.StringToHash(trigger);
    }


    protected override void ImmediateReaction ()
    {
        Debug.Log(debugMsg);
    }
}
