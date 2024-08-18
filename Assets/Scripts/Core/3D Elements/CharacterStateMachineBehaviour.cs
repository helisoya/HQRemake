using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachineBehaviour : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Character character = animator.transform.parent.GetComponent<Character>();
        if (character && character.nextBodyRuntimeAnimatorController != null)
        {
            character.ChangeBodyAnimationSetImmediate(character.nextBodyRuntimeAnimatorController);
        }
    }
}
