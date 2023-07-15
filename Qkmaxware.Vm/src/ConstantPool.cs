using System.Collections;

namespace Qkmaxware.Vm;

public class ConstantPool : IEnumerable<ConstantData> {

    private List<ConstantData> elements = new List<ConstantData>();

    public int Count => elements.Count;

    public ConstantData this[int index] => elements[index];
    public ConstantData this[StaticRef index] {
        get => elements[index.PoolIndex];
    }

    public void EnsureCapacity(int size) {
        this.elements.EnsureCapacity(size);
    }

    public void Add(ConstantData data) {
        data.PoolIndex = this.Count;
        elements.Add(data);
    }

    public void AddRange(IEnumerable<ConstantData> data) {
        foreach (var e in data) {
            Add(e);
        }
    }

    public void Clear() {
        this.elements.Clear();
    }

    public void Remove(ConstantData data) {
        this.elements.Remove(data);
        for (var i = 0; i < this.elements.Count; i++) {
            this.elements[i].PoolIndex = i;
        }
    }

    public IEnumerator<ConstantData> GetEnumerator() {
        return this.elements.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return this.elements.GetEnumerator();
    }
}