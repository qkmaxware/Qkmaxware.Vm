namespace Qkmaxware.Vm;

/// <summary>
/// Builder to simplify the creation of bytecode modules programmatically
/// </summary>
public partial class ModuleBuilder {
    public void Nop() {
        this.AddInstruction("nop");
    }

    public void Exit(int status) {
        this.AddInstruction("exit", Operand.From(status));
    }

    public void SwapStackTop() {
        this.AddInstruction("swap");
    }

    public void DuplicateStackTop() {
        this.AddInstruction("dup");
    }

    public void DuplicateStackTop(int elements) {
        this.AddInstruction("dup_block", Operand.From(elements));
    }
    public void DuplicateStackElement(int position) {
        this.AddInstruction("dup_below", Operand.From(position));
    }

    public void PopStackTop() {
        this.AddInstruction("pop");
    }

    public void PopStackElements(int elements) {
        this.AddInstruction("pop_n", Operand.From(elements));
    }

    public void PushAddressOf(MemoryRef element) {
        this.AddInstruction("immediate_i32", Operand.From(element.Offset));
    }

    public void PushLocal(int localIndex) {
        this.AddInstruction("load_local", Operand.From(localIndex));
    }

    public void StoreLocal(int localIndex) {
        this.AddInstruction("store_local", Operand.From(localIndex));
    }

    public void PushInt32(int value) {
        this.AddInstruction("immediate_i32", Operand.From(value));
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

    public void PushUInt32(uint value) {
        this.AddInstruction("immediate_u32", Operand.From(value));
    }
    
    public void AddUInt32() => this.AddInstruction("add_u32");
    public void SubtractUInt32() => this.AddInstruction("sub_u32");
    public void MultiplyUInt32() => this.AddInstruction("mul_u32");
    public void DivideUInt32() => this.AddInstruction("div_u32");
    public void RemainderUInt32() => this.AddInstruction("rem_u32");

    public void AndUInt32() => this.AddInstruction("and_u32");
    public void OrUInt32() => this.AddInstruction("or_u32");
    public void XorUInt32() => this.AddInstruction("xor_u32");

    public void PushFloat32(float value) {
        this.AddInstruction("immediate_f32", Operand.From(value));
    }

    public void AddFloat32() => this.AddInstruction("add_f32");
    public void SubtractFloat32() => this.AddInstruction("sub_f32");
    public void MultiplyFloat32() => this.AddInstruction("mul_f32");
    public void DivideFloat32() => this.AddInstruction("div_f32");
    public void RemainderFloat32() => this.AddInstruction("rem_f32");
    public void PowerFloat32() => this.AddInstruction("pow_f32");
    public void NegateFloat32() => this.AddInstruction("neg_f32");

    public void SetLessThanInt32() => this.AddInstruction("set_lt_i32");
    public void SetLessThanUInt32() => this.AddInstruction("set_lt_u32");
    public void SetLessThanFloat32() => this.AddInstruction("set_lt_f2");
    public void SetGreaterThanInt32() => this.AddInstruction("set_gt_i32");
    public void SetGreaterThanUInt32() => this.AddInstruction("set_gt_u32");
    public void SetGreaterThanFloat32() => this.AddInstruction("set_gt_f32");
    public void SetEqualThanInt32() => this.AddInstruction("set_eq_i32");
    public void SetEqualThanUInt32() => this.AddInstruction("set_eq_u32");
    public void SetEqualThanFloat32() => this.AddInstruction("set_eq_f32");
    public void SetNotEqualThanInt32() => this.AddInstruction("set_neq_i32");
    public void SetNotEqualThanUInt32() => this.AddInstruction("set_neq_u32");
    public void SetNotEqualThanFloat32() => this.AddInstruction("set_neq_f32");

    public void ObjectSize(int fromMemory) => this.AddInstruction("sizeof", Operand.From(fromMemory));

    public void Goto(long anchor) {
        this.AddInstruction(
            "goto", 
            Operand.From((int)(anchor - (this.Anchor() + 5)))
        );
    }
    public void GotoIfStackTopZero(long anchor) {
        this.AddInstruction(
            "goto_if_zero", 
            Operand.From((int)(anchor - (this.Anchor() + 5)))
        );
    }
    public void GotoIfStackTopNotZero(long anchor) {
        this.AddInstruction(
            "goto_if_nzero", 
            Operand.From((int)(anchor - (this.Anchor() + 5)))
        );
    }

    public void Goto(Label l) {
        this.AddInstruction(
            "goto", 
            Operand.From((int)(l.CodePosition - (this.Anchor() + 5)))
        );
    }
    public void GotoIfStackTopZero(Label l) {
        this.AddInstruction(
            "goto_if_zero", 
            Operand.From((int)(l.CodePosition - (this.Anchor() + 5)))
        );
    }
    public void GotoIfStackTopNotZero(Label l) {
        this.AddInstruction(
            "goto_if_nzero", 
            Operand.From((int)(l.CodePosition - (this.Anchor() + 5)))
        );
    }

    public void Call(Label l, int argc) {
        this.AddInstruction("call", new VmValue[]{Operand.From((int)(l.CodePosition - (this.Anchor() + 9))), Operand.From(argc)});
    }
    public void Call(int offset, int argc) {
        this.AddInstruction("call", new VmValue[]{Operand.From(offset), Operand.From(argc)});
    }

    public void CallExternal(Import import, int argc) {
        this.AddInstruction("call_external", new VmValue[]{Operand.From(this.imports.IndexOf(import)), Operand.From(argc)});
    }
    public void CallExternal(int importIndex, int argc) {
        this.AddInstruction("call_external", new VmValue[]{Operand.From(importIndex), Operand.From(argc)});
    }

    public void PushArgument(int argIndex) {
        this.AddInstruction("load_arg", Operand.From(argIndex));
    }

    public void Return() {
        this.AddInstruction("return_procedure");
    }
    public void ReturnResult() {
        this.AddInstruction("return_function");
    }

    public void PrintChar() => this.AddInstruction("putchar");
    public void ReadChar() => this.AddInstruction("readchar");

    public void Load8(int fromMemory, Extend extend) {
        switch (extend) {
            case Extend.Zero:
                this.AddInstruction("load8_u", Operand.From(fromMemory));
                break;
            case Extend.Sign:
                this.AddInstruction("load8_s", Operand.From(fromMemory));
                break;
        }
    }

    public void Load16(int fromMemory, Extend extend) {
        switch (extend) {
            case Extend.Zero:
                this.AddInstruction("load16_u", Operand.From(fromMemory));
                break;
            case Extend.Sign:
                this.AddInstruction("load16_s", Operand.From(fromMemory));
                break;
        }
    }

    public void Load32(int fromMemory) {
        this.AddInstruction("load32", Operand.From(fromMemory));
    }

    public void Allocate() => this.AddInstruction("alloc");
    public void Allocate(int bytes) {
        this.PushInt32(bytes);
        this.AddInstruction("alloc");
    }
    public void Free() {
        this.AddInstruction("free");
    }

    public void LessThanI32() {
        this.AddInstruction("set_lt_i32");
    }
    public void LessThanU32() {
        this.AddInstruction("set_lt_u32");
    }
    public void LessThanF32() {
        this.AddInstruction("set_lt_f32");
    }

    public void GreaterThanI32() {
        this.AddInstruction("set_gt_i32");
    }
    public void GreaterThanU32() {
        this.AddInstruction("set_gt_u32");
    }
    public void GreaterThanF32() {
        this.AddInstruction("set_gt_f32");
    }

    public void EqualsI32() {
        this.AddInstruction("set_eq_i32");
    }
    public void EqualsU32() {
        this.AddInstruction("set_eq_u32");
    }
    public void EqualsF32() {
        this.AddInstruction("set_eq_f32");
    }

    public void NotEqualsI32() {
        this.AddInstruction("set_neq_i32");
    }
    public void NotEqualsU32() {
        this.AddInstruction("set_neq_u32");
    }
    public void NotEqualsF32() {
        this.AddInstruction("set_neq_f32");
    }
}