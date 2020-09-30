using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(NewReaction))]
public class NewReactionEditor : ReactionEditor
{
    protected override string GetFoldoutLabel() {
        return "Paska";
    }
}
