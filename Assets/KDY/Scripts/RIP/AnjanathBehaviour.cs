using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class AnjanathBehaviour : MonoBehaviour
{
    public int AnjanathHP;
    public int AnjanathNomalAttack;
    public int AnjanathSpeed;
    public bool AnjanathAlive;

    public Node _behaviour;
    public Animator _animator;
    private ActionNode _currentActionNode;

    public bool isPlayerInRange;
    public bool isBreathAttack;
    public bool isDetectedPlayer;

    public string nowAnimation;
    public int c = 0;

    public void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _behaviour = BossTree();
        AnjanathAlive = true;
    }

    public Node BossTree()
    {
        return new SelectorNode(new List<Node>
        {
            // Left Subtree
/*            new SequenceNode(new List<Node>
            {
                new ConditionNode(canBreathAttack),
                new ActionNode(BreathAttack)
            }),
            new SequenceNode(new List<Node> 
            {
                new ConditionNode(isPlayerInAttackRange),
                new ActionNode(NomalAttack)
            }),

            // Right Subtree
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

*/        });
    }

    private void Update()
    {
        NodeState result = _behaviour.Run();

        if (result == NodeState.SUCCESS || result == NodeState.FAILURE)
        {
            ResetActionNode(_behaviour);
        }
    }

    public void ResetActionNode(Node node)
    {
        if(node is ActionNode actionNode)
        {
            actionNode.Reset();
        }
        else if(node is SelectorNode selectorNode)
        {
            foreach(var child in selectorNode.Children)
            {
                ResetActionNode(child);
            }
        }
        else if(node is SequenceNode sequenceNode)
        {
            foreach (var child in sequenceNode.Children)
            {
                ResetActionNode(child);
            }
        }
    }

    public void ChangeState(string stateName)
    {
        Debug.Log($"{c++} / {nowAnimation}");
        _animator.Play(stateName);
    }

    public bool canBreathAttack()
    {
        //Debug.Log(nameof(canBreathAttack));
        // AnjanathHP <= 50 && is30Chance
        return isBreathAttack;
    }

    // 이 아래부터는 임의로 만든 조건 및 값들입니다.
    public bool isPlayerInAttackRange()
    {
        return isPlayerInRange;
    }

    public bool DetectedPlayer()
    {
        //Debug.Log(nameof(DetectedPlayer));
        return isDetectedPlayer;
    }

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

    public void BattleIdle()
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

}
