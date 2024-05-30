public enum NodeState
{
    FAILURE,
    SUCCESS,
    RUNNING
}

public abstract class Node
{
    public abstract NodeState Run();
}