namespace Qkmaxware.Vm;

/// <summary>
/// Builder to simplify the creation of bytecode modules programmatically
/// </summary>
public partial class ModuleBuilder {
    public void Nop() {
        this.AddInstruction("nop");
    }

    public void Exit(int status) {
        this.AddInstruction("exit", new Int32Operand(status));
    }

    public void SwapStackTop() {
        this.AddInstruction("swap");
    }

    public void DuplicateStackTop() {
        this.AddInstruction("dup");
    }

    public void DuplicateStackTop(int elements) {
        this.AddInstruction("dup_block", new Int32Operand(elements));
    }
    public void DuplicateStackElement(int position) {
        this.AddInstruction("dup_below", new Int32Operand(position));
    }

    public void PopStackTop() {
        this.AddInstruction("pop");
    }

    public void PopStackElements(int elements) {
        this.AddInstruction("pop_n", new Int32Operand(elements));
    }

    public void PushConstant(int constantPoolIndex) {
        this.AddInstruction("load_const", new Int32Operand(constantPoolIndex));
    }

    public void PushLocal(int localIndex) {
        this.AddInstruction("load_local", new Int32Operand(localIndex));
    }

    public void StoreLocal(int localIndex) {
        this.AddInstruction("store_local", new Int32Operand(localIndex));
    }

    public void PushInt32(int value) {
        this.AddInstruction("immediate_i32", new Int32Operand(value));
    }

    public void AddInt32() => this.AddInstruction("add_i32");
    public void SubtractInt32() => this.AddInstruction("sub_i32");
    public void MultiplyInt32() => this.AddInstruction("mul_i32");
    public void DivideInt32() => this.AddInstruction("div_i32");
    public void ModulusInt32() => this.AddInstruction("mod_i32");
    public void RemainderInt32() => this.AddInstruction("rem_i32");
    public void NegateInt32() => this.AddInstruction("neg_i32");

    public void AndInt32() => this.AddInstruction("and_i32");
    public void OrInt32() => this.AddInstruction("or_i32");
    public void XorInt32() => this.AddInstruction("xor_i32");
    public void ComplementInt32() => this.AddInstruction("complement_i32");

    public void PushUInt32(int value) {
        this.AddInstruction("immediate_u32", new Int32Operand(value));
    }
    
    public void AddUInt32() => this.AddInstruction("add_u32");
    public void SubtractUInt32() => this.AddInstruction("sub_u32");
    public void MultiplyUInt32() => this.AddInstruction("mul_u32");
    public void DivideUInt32() => this.AddInstruction("div_u32");
    public void RemainderUInt32() => this.AddInstruction("rem_u32");

    public void AndUInt32() => this.AddInstruction("and_u32");
    public void OrUInt32() => this.AddInstruction("or_u32");
    public void XorUInt32() => this.AddInstruction("xor_u32");

    public void PushFloat32(int value) {
        this.AddInstruction("immediate_f32", new Int32Operand(value));
    }

    public void AddFloat32() => this.AddInstruction("add_f32");
    public void SubtractFloat32() => this.AddInstruction("sub_f32");
    public void MultiplyFloat32() => this.AddInstruction("mul_f32");
    public void DivideFloat32() => this.AddInstruction("div_f32");
    public void RemainderFloat32() => this.AddInstruction("rem_f32");
    public void PowerFloat32() => this.AddInstruction("pow_f32");
    public void NegateFloat32() => this.AddInstruction("neg_f32");

    public void ObjectSize() => this.AddInstruction("sizeof");
    public void ArrayLength() => this.AddInstruction("len");
    public void GetArrayElement(int index) => this.AddInstruction("get_element", new Int32Operand(index));


    public void Goto(long anchor) {
        this.AddInstruction(
            "goto", 
            new Int32Operand((int)(anchor - (this.Anchor() + 5)))
        );
    }
    public void GotoIfStackTopZero(long anchor) {
        this.AddInstruction(
            "goto_if_zero", 
            new Int32Operand((int)(anchor - (this.Anchor() + 5)))
        );
    }
    public void GotoIfStackTopNotZero(long anchor) {
        this.AddInstruction(
            "goto_if_nzero", 
            new Int32Operand((int)(anchor - (this.Anchor() + 5)))
        );
    }

    public void Goto(Label l) {
        this.AddInstruction(
            "goto", 
            new Int32Operand((int)(l.CodePosition - (this.Anchor() + 5)))
        );
    }
    public void GotoIfStackTopZero(Label l) {
        this.AddInstruction(
            "goto_if_zero", 
            new Int32Operand((int)(l.CodePosition - (this.Anchor() + 5)))
        );
    }
    public void GotoIfStackTopNotZero(Label l) {
        this.AddInstruction(
            "goto_if_nzero", 
            new Int32Operand((int)(l.CodePosition - (this.Anchor() + 5)))
        );
    }

    public void Call(Label l, int argc) {
        this.AddInstruction("call", new VmValue[]{new Int32Operand((int)(l.CodePosition - (this.Anchor() + 9))), new Int32Operand(argc)});
    }
    public void Call(int offset, int argc) {
        this.AddInstruction("call", new VmValue[]{new Int32Operand(offset), new Int32Operand(argc)});
    }

    public void PushArgument(int argIndex) {
        this.AddInstruction("load_arg", new Int32Operand(argIndex));
    }

    public void Return() {
        this.AddInstruction("return_procedure");
    }
    public void ReturnResult() {
        this.AddInstruction("return_function");
    }

    public void PrintChar() => this.AddInstruction("putchar");
}