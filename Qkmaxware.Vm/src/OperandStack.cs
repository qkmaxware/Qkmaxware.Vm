namespace Qkmaxware.Vm;

/// <summary>
/// Operand stack of the virtual machine
/// </summary>
public class OperandStack {

    private List<Operand> stack = new List<Operand>();

    /// <summary>
    /// Operand stack pointer to the top
    /// </summary>
    public int SP => stack.Count;

    /// <summary>
    /// Check if the operand stack is empty
    /// </summary>
    public bool IsEmpty => stack.Count == 0;

    /// <summary>
    /// Peek at the operand on the top of the stack
    /// </summary>
    /// <returns>operand at the stack top if it exists</returns>
    public Operand? PeekTop() => IsEmpty ? null : stack[stack.Count - 1];

    /// <summary>
    /// Pop the operand off the top of the stack and return it
    /// </summary>
    /// <returns>operand at the stack top if it exists or an exception is thrown</returns>
    public Operand PopTop() {
        if (IsEmpty) {
            throw new IndexOutOfRangeException("Stack is empty");
        }
        var op = stack[stack.Count - 1];
        stack.RemoveAt(stack.Count - 1);
        return op;
    }

    /// <summary>
    /// Add an operand to the top of the stack
    /// </summary>
    /// <param name="operand">operand to add</param>
    public void PushTop(Operand operand) => stack.Add(operand);

    private int _fp;

    /// <summary>
    /// Operand stack pointer to the start of the current stack frame
    /// </summary>
    public int FP {
        get {
            return _fp;
        }
        set {
            if (value < 0)
                value = 0;
            if (value > SP)
                value = SP;
            _fp = value;
        }
    }

    /// <summary>
    /// Get the value of the operand at FP + index
    /// </summary>
    /// <param name="index">local index offset</param>
    public Operand GetFrameRelative(int index) {
        var abs_index = FP + index;
        if (abs_index < 0 || abs_index >= SP)
            throw new IndexOutOfRangeException();
        return stack[abs_index];
    }

    /// <summary>
    /// Set the value of the operand at FP + index
    /// </summary>
    /// <param name="index">local index offset</param>
    /// <param name="operand">new operand value</param>
    public void SetFrameRelative(int index, Operand operand) {
        var abs_index = FP + index;
        if (abs_index < 0 || abs_index >= SP)
            throw new IndexOutOfRangeException();
        stack[abs_index] = operand;
    }
}