using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class AnjanathTree : MonoBehaviour
{
    private Node _behaviourTree;
    private Animator _animator;
    private ActionNode _currentActionNode;

    public bool isPlayerInRange;
    public bool isBreathAttack;
    public bool isDetectedPlayer;

    public string nowAnimation;


    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _behaviourTree = CreateBehaviourTree();
    }

    private Node CreateBehaviourTree()
    {
        return new SelectorNode(new List<Node>
        {
            // Left Subtree
            new SequenceNode(new List<Node>
            {
                new ConditionNode(canBreathAttack),
                new ActionNode(BreathAttack, isBreathAttackAnimation)
            }),
            new SequenceNode(new List<Node> 
            {
                new ConditionNode(isPlayerInAttackRange),
                new ActionNode(NomalAttack, isBreathAttackAnimation)
            }),

/*            // Right Subtree
            new SelectorNode (new List<Node>
            {
                new SequenceNode (new List<Node>
                {
                    new ConditionNode(DetectedPlayer),
                    new ActionNode(BattleIdle),
                    new ActionNode(AttackTracking)
                }),
                new SequenceNode (new List<Node>
                {
                    new ActionNode(NomalIdle),
                    new ActionNode(NomalWalking)
                })
            })
*/
        });
    }


    public bool canBreathAttack()
    {
        //Debug.Log(nameof(canBreathAttack));
        return isBreathAttack;
    }

    public bool isPlayerInAttackRange()
    {
        return isPlayerInRange;
    }

/*    public bool DetectedPlayer()
    {
        //Debug.Log(nameof(DetectedPlayer));
        return isDetectedPlayer;
    }
*/
    public void BreathAttack()
    {
        //Debug.Log(nameof(BreathAttack));
        ChangeState("BreathAttack");
        nowAnimation = "BreathAttack";
    }

    public void NomalAttack()
    {
        //Debug.Log(nameof(NomalAttack));
        ChangeState("NomalAttack");
        nowAnimation = "NomalAttack";
    }

/*    public void BattleIdle()
    {
        //Debug.Log(nameof(BattleIdle));
        ChangeState("BattleIdle");
        nowAnimation = "BattleIdle";
    }
    public void AttackTracking()
    {
        //Debug.Log(nameof(AttackTracking));
        ChangeState("AttackTracking");
        nowAnimation = "AttackTracking";
    }

    public void NomalIdle()
    {
        //Debug.Log(nameof(NomalIdle));
        ChangeState("NomalIdle");
        nowAnimation = "NomalIdle";
    }

    public void NomalWalking()
    {
        //Debug.Log(nameof(NomalWalking));
        ChangeState("NomalWalking");
        nowAnimation = "NomalWalking";
    }
*/
    private bool isBreathAttackAnimation()
    {
        return IsAnimationCompleted("BreathAttack");
    }

    private bool isNomalAttackAnimation()
    {
        return IsAnimationCompleted("NomalAttack");
    }

    private bool IsAnimationCompleted(string stateName)
    {
        var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime >= 1.0f;
    }

    private void ChangeState(string newState)
    {
        _animator.Play(newState);
        Debug.Log($"State changed to: {newState}");
    }

    private void Update()
    {
        NodeState result = _behaviourTree.Run();

        if (result == NodeState.SUCCESS || result == NodeState.FAILURE)
        {
            ResetActionNodes(_behaviourTree);
        }
    }

    private void ResetActionNodes(Node node)
    {
        if (node is ActionNode actionNode)
        {
            actionNode.Reset();
        }
        else if (node is SelectorNode selector)
        {
            foreach (var child in selector.Children)
            {
                ResetActionNodes(child);
            }
        }
        else if (node is SequenceNode sequence)
        {
            foreach (var child in sequence.Children)
            {
                ResetActionNodes(child);
            }
        }
    }
}
