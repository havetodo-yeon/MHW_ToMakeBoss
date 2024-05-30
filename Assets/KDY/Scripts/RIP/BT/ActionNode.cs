using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode : Node
{
    public Action _action;
    private Func<bool> _isCompleted;
    private bool _actionStarted;

    public ActionNode(Action action, Func<bool> isCompleted)
    {
        _action = action;
        _isCompleted = isCompleted;
        _actionStarted = false;
    }


    public override NodeState Run()
    {
        if (!_actionStarted)
        {
            _action();
            _actionStarted = true;
            return NodeState.RUNNING;
        }

        if (_isCompleted())
        {
            _actionStarted = false;
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
    }

    public void Reset()
    {
        _actionStarted = false;
    }
}
