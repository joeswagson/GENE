namespace GENE.Nodes.Primitive;

public abstract class AsyncNode : INode
{
    public event Action<Task>? Ran;

    void INode.Initialize()
    {
        Initialize();
        Ran?.Invoke(Task.Run(Run));
    }

    protected virtual void Initialize() { }

    public abstract string Name { get; }
    protected abstract Task Run();
}
