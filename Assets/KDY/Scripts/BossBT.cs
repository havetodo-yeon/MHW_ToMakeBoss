using CleverCrow.Fluid.BTs.Trees;
using CleverCrow.Fluid.BTs.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossBT : MonoBehaviour
{
    public int BossHP;
    public float _movementSpeed;
    public Transform _detectedPlayer;

    public BehaviorTree _tree;
    public Transform _player;

    public bool sam;
    
    public Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();

        _tree = new BehaviorTreeBuilder(gameObject)
            .Selector()
                .Sequence()
                    .Condition("isAttacking", () => true)
                    .Condition("isPlayerInAttackRange", () => true)
                    .Do(() =>
                    {
                        _animator.Play("NomalAttack");
                        return TaskStatus.Success;
                    })
                .End()
                .Sequence()
                    .Condition("detectedPlayer", () => true)
                    .Do("TrackingPlayer", () =>
                    {
                        _animator.Play("BattleWalk");
                        transform.position = Vector3.MoveTowards(transform.position, _detectedPlayer.position, Time.deltaTime * _movementSpeed);
                        return TaskStatus.Success;
                    })
                .End()
                .Sequence()
                    .StateAction("Walk", => { sam = true; })
                .End()
            .End()
            .Build();
    }

    void Update()
    {
        
    }

    IEnumerator ActiveAi()
    {
        while(true)
        if(BossHP <= 0)
        {
            _animator.Play("Die");
        }
    }

}
