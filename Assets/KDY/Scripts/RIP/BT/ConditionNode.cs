using System;
using System.Collections.Generic;
using UnityEngine;

public class ConditionNode : Node
{
    public Func<bool> _condition;

    public ConditionNode(Func<bool> condition)
    {
        _condition = condition;
    }

    public override NodeState Run()
    {
        return _condition()? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
