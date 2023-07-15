using System.Collections;

namespace Qkmaxware.Vm;

/// <summary>
/// A reference to a static value in the constant pool
/// </summary>
public class StaticRef {
    public int PoolIndex {get; private set;}

    internal StaticRef(int index) {
        this.PoolIndex = index;
    }
}

/// <summary>
/// A pool of static values
/// </summary>
public class StaticPool : IEnumerable<Operand> {

    private List<Operand> elements = new List<Operand>();

    public int Count => elements.Count;

    public Operand this[int index] {
        get => elements[index];
        set => elements[index] = value;
    }
    public Operand this[StaticRef index] {
        get => elements[index.PoolIndex];
        set => elements[index.PoolIndex] = value;
    }

    public void EnsureCapacity(int size) {
        this.elements.EnsureCapacity(size);
    }

    public void Add(Operand data) {
        elements.Add(data);
    }

    public void AddRange(IEnumerable<Operand> data) {
        foreach (var e in data) {
            Add(e);
        }
    }

    public void Clear() {
        this.elements.Clear();
    }

    public void Remove(Operand data) {
        this.elements.Remove(data);
    }

    public IEnumerator<Operand> GetEnumerator() {
        return this.elements.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return this.elements.GetEnumerator();
    }

    public StaticPool Clone() {
        var pool = new StaticPool();
        pool.EnsureCapacity(this.elements.Count);
        pool.AddRange(this.elements);
        return pool;
    }
}