﻿using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour
{
    [Header("Range")]
    [SerializeField] float _detectRange = 10f;
    [SerializeField] float _meleeAttackRange = 5f;

    [Header("Movement")]
    [SerializeField] float _movementSpeed = 10f;

    Vector3 _originPos = default;
    BehaviorTreeRunner _BTRunner = null;
    Transform _detectedPlayer = null;
    Animator _animator = null;

    const string _ATTACK_ANIM_STATE_NAME = "NomalAttack";
    const string _ATTACK_ANIM_TIRGGER_NAME = "attack";

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _BTRunner = new BehaviorTreeRunner(SettingBT());

        _originPos = transform.position;
    }

    private void Update()
    {
        _BTRunner.Operate();
    }

    INode SettingBT()
    {
        return new SelectorNode
            (
                new List<INode>()
                {
                    new SequenceNode
                    (
                        new List<INode>()
                        {
                            new ActionNode(CheckMeleeAttacking),
                            new ActionNode(CheckEnemyWithinMeleeAttackRange),
                            new ActionNode(DoMeleeAttack),
                        }
                    ),
                    new SequenceNode
                    (
                        new List<INode>()
                        {
                            new ActionNode(CheckDetectEnemy),
                            new ActionNode(MoveToDetectEnemy),
                        }
                    ),
                    new ActionNode(MoveToOriginPosition)
                }
            );
    }

    bool IsAnimationRunning(string stateName)
    {
        if (_animator != null)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            {
                var normalizedTime = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                return normalizedTime != 0 && normalizedTime < 1f;
            }
        }

        return false;
    }

    #region Attack
    INode.ENodeState CheckMeleeAttacking()
    {
        if (IsAnimationRunning(_ATTACK_ANIM_STATE_NAME))
        {
            return INode.ENodeState.RUNNING;
        }

        return INode.ENodeState.SUCCESS;
    }

    INode.ENodeState CheckEnemyWithinMeleeAttackRange()
    {
        if (_detectedPlayer != null)
        {
            if (Vector3.SqrMagnitude(_detectedPlayer.position - transform.position) < (_meleeAttackRange * _meleeAttackRange))
            {
                return INode.ENodeState.SUCCESS;
            }
        }

        return INode.ENodeState.FAILURE;
    }

    INode.ENodeState DoMeleeAttack()
    {
        if (_detectedPlayer != null)
        {
            _animator.SetTrigger(_ATTACK_ANIM_TIRGGER_NAME);
            return INode.ENodeState.SUCCESS;
        }

        return INode.ENodeState.FAILURE;
    }
    #endregion

    #region Detect & Move
    INode.ENodeState CheckDetectEnemy()
    {
        var overlapColliders = Physics.OverlapSphere(transform.position, _detectRange, LayerMask.GetMask("Player"));

        if (overlapColliders != null && overlapColliders.Length > 0)
        {
            _detectedPlayer = overlapColliders[0].transform;

            return INode.ENodeState.SUCCESS;
        }

        _detectedPlayer = null;

        return INode.ENodeState.FAILURE;
    }

    INode.ENodeState MoveToDetectEnemy()
    {
        if (_detectedPlayer != null)
        {
            if (Vector3.SqrMagnitude(_detectedPlayer.position - transform.position) < (_meleeAttackRange * _meleeAttackRange))
            {
                return INode.ENodeState.SUCCESS;
            }

            transform.position = Vector3.MoveTowards(transform.position, _detectedPlayer.position, Time.deltaTime * _movementSpeed);

            return INode.ENodeState.RUNNING;
        }

        return INode.ENodeState.FAILURE;
    }
    #endregion

    #region  Move Origin Position
    INode.ENodeState MoveToOriginPosition()
    {
        if (Vector3.SqrMagnitude(_originPos - transform.position) < float.Epsilon * float.Epsilon)
        {
            return INode.ENodeState.SUCCESS;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _originPos, Time.deltaTime * _movementSpeed);
            return INode.ENodeState.RUNNING;
        }
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, _detectRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, _meleeAttackRange);
    }
}