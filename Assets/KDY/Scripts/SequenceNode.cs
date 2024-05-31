using System.Collections.Generic;

public sealed class SequenceNode : INode
{
    List<INode> _childs;

    public SequenceNode(List<INode> childs)
    {
        _childs = childs;
    }

    public INode.ENodeState Evaluate()
    {
        if (_childs == null || _childs.Count == 0)
            return INode.ENodeState.FAILURE;

        foreach (var child in _childs)
        {
            switch (child.Evaluate())
            {
                case INode.ENodeState.RUNNING:
                    return INode.ENodeState.RUNNING;
                case INode.ENodeState.SUCCESS:
                    continue;
                case INode.ENodeState.FAILURE:
                    return INode.ENodeState.FAILURE;
            }
        }

        return INode.ENodeState.SUCCESS;
    }
}