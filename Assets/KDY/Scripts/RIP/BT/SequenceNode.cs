using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : Node
{
    private List<Node> _children;
    private int _currentChild;

    public SequenceNode(List<Node> children)
    {
        _children = children;
        _currentChild = 0;
    }

    public List<Node> Children => _children;

    public override NodeState Run()
    {
        while (_currentChild < _children.Count)
        {
            NodeState result = _children[_currentChild].Run();
            if (result == NodeState.FAILURE)
            {
                _currentChild = 0;
                return NodeState.FAILURE;
            }
            if (result == NodeState.RUNNING)
            {
                return NodeState.RUNNING;
            }
            _currentChild++;
        }
        _currentChild = 0;
        return NodeState.SUCCESS;
    }
}