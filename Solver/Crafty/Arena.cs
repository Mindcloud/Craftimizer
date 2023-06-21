using System.Runtime.CompilerServices;

namespace Craftimizer.Solver.Crafty;

public class Arena<T> where T : struct
{
    public readonly record struct Node
    {
        public int? Parent { get; init; }
        public int Index { get; init; }
        public List<int> Children { get; init; }
        public T State { get; init; }
    }

    private readonly List<Node> nodes = new();

    public Arena(T initialState = default)
    {
        nodes.Add(new() { Parent = null, Index = 0, Children = new(), State = initialState });
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Insert(int parentIndex, T state)
    {
        var index = nodes.Count;
        nodes.Add(new() { Parent = parentIndex, Index = index, Children = new(), State = state });
        nodes[parentIndex].Children.Add(index);
        return index;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Node Get(int index) => nodes[index];
}