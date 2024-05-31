﻿public interface INode
{
    public enum ENodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE,
    }

    public ENodeState Evaluate();
}