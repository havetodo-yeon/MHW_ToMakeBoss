using System.Collections.Generic;

public sealed class SelectorNode : INode
{
    List<INode> _childs;

    public SelectorNode(List<INode> childs)
    {
        _childs = childs;
    }

    public INode.ENodeState Evaluate()
    {
        if (_childs == null)
            return INode.ENodeState.FAILURE;

        foreach (var child in _childs)
        {
            switch (child.Evaluate())
            {
                case INode.ENodeState.RUNNING:
                    return INode.ENodeState.RUNNING;
                case INode.ENodeState.SUCCESS:
                    return INode.ENodeState.SUCCESS;
            }
        }

        return INode.ENodeState.FAILURE;
    }
}