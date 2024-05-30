using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : Node
{
    public List<Node> _children;
    public int _currentChild;

    public SelectorNode(List<Node> children)
    {
        _children = children;
        _currentChild = 0;
    }

    public List<Node> Children => _children;

    public override NodeState Run()
    {
        while(_currentChild < _children.Count) 
        {
            NodeState result = _children[_currentChild].Run();
            if (result == NodeState.SUCCESS)
            {
                _currentChild = 0;
                return NodeState.SUCCESS;
            }
            if (result == NodeState.RUNNING)
            {
                return NodeState.RUNNING;
            }
            _currentChild++;
        }
        _currentChild = 0;
        return NodeState.FAILURE;
    }
}