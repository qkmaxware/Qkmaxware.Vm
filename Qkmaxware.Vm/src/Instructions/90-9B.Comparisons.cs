namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Compares for less than 
/// </summary>
public class SetLtI32 : Instruction {
    
    public SetLtI32() {
        // Set opcode
        this.Opcode = 0x90; 
        
        // Arguments

        // Stack
        this.AddStackOperand("lhs");
        this.AddStackOperand("rhs");
        this.AddStackReturn("boolean");
    }

    public override string Description => "Compares the top 2 elements on the stack and returns 1 if the less than comparison holds, returns 0 otherwise.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = runtime.Stack.PopTop();
        var lhs = runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.Int32 < rhs.Int32 ? 1 : 0));
    }
}

/// <summary>
/// Compares for less than 
/// </summary>
public class SetLtU32 : Instruction {
    
    public SetLtU32() {
        // Set opcode
        this.Opcode = 0x91; 
        
        // Arguments

        // Stack
        this.AddStackOperand("lhs");
        this.AddStackOperand("rhs");
        this.AddStackReturn("boolean");
    }

    public override string Description => "Compares the top 2 elements on the stack and returns 1 if the less than comparison holds, returns 0 otherwise.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = runtime.Stack.PopTop();
        var lhs = runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.UInt32 < rhs.UInt32 ? 1 : 0));
    }
}

/// <summary>
/// Compares for less than 
/// </summary>
public class SetLtF32 : Instruction {
    
    public SetLtF32() {
        // Set opcode
        this.Opcode = 0x92; 
        
        // Arguments

        // Stack
        this.AddStackOperand("lhs");
        this.AddStackOperand("rhs");
        this.AddStackReturn("boolean");
    }

    public override string Description => "Compares the top 2 elements on the stack and returns 1 if the less than comparison holds, returns 0 otherwise.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = runtime.Stack.PopTop();
        var lhs = runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.Float32 < rhs.Float32 ? 1 : 0));
    }
}

/// <summary>
/// Compares for greater than 
/// </summary>
public class SetGtI32 : Instruction {
    
    public SetGtI32() {
        // Set opcode
        this.Opcode = 0x93; 
        
        // Arguments

        // Stack
        this.AddStackOperand("lhs");
        this.AddStackOperand("rhs");
        this.AddStackReturn("boolean");
    }

    public override string Description => "Compares the top 2 elements on the stack and returns 1 if the greater than comparison holds, returns 0 otherwise.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = runtime.Stack.PopTop();
        var lhs = runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.Int32 > rhs.Int32 ? 1 : 0));
    }
}

/// <summary>
/// Compares for greater than 
/// </summary>
public class SetGtU32 : Instruction {
    
    public SetGtU32() {
        // Set opcode
        this.Opcode = 0x94; 
        
        // Arguments

        // Stack
        this.AddStackOperand("lhs");
        this.AddStackOperand("rhs");
        this.AddStackReturn("boolean");
    }

    public override string Description => "Compares the top 2 elements on the stack and returns 1 if the greater than comparison holds, returns 0 otherwise.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = runtime.Stack.PopTop();
        var lhs = runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.UInt32 > rhs.UInt32 ? 1 : 0));
    }
}

/// <summary>
/// Compares for greater than 
/// </summary>
public class SetGtF32 : Instruction {
    
    public SetGtF32() {
        // Set opcode
        this.Opcode = 0x95; 
        
        // Arguments

        // Stack
        this.AddStackOperand("lhs");
        this.AddStackOperand("rhs");
        this.AddStackReturn("boolean");
    }

    public override string Description => "Compares the top 2 elements on the stack and returns 1 if the greater than comparison holds, returns 0 otherwise.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = runtime.Stack.PopTop();
        var lhs = runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.Float32 > rhs.Float32 ? 1 : 0));
    }
}

/// <summary>
/// Compares for equality 
/// </summary>
public class SetEqI32 : Instruction {
    
    public SetEqI32() {
        // Set opcode
        this.Opcode = 0x96; 
        
        // Arguments

        // Stack
        this.AddStackOperand("lhs");
        this.AddStackOperand("rhs");
        this.AddStackReturn("boolean");
    }

    public override string Description => "Compares the top 2 elements on the stack and returns 1 if the equality comparison holds, returns 0 otherwise.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = runtime.Stack.PopTop();
        var lhs = runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.Int32 == rhs.Int32 ? 1 : 0));
    }
}

/// <summary>
/// Compares for equality 
/// </summary>
public class SetEqU32 : Instruction {
    
    public SetEqU32() {
        // Set opcode
        this.Opcode = 0x97; 
        
        // Arguments

        // Stack
        this.AddStackOperand("lhs");
        this.AddStackOperand("rhs");
        this.AddStackReturn("boolean");
    }

    public override string Description => "Compares the top 2 elements on the stack and returns 1 if the equality comparison holds, returns 0 otherwise.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = runtime.Stack.PopTop();
        var lhs = runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.UInt32 == rhs.UInt32 ? 1 : 0));
    }
}

/// <summary>
/// Compares for equality 
/// </summary>
public class SetEqF32 : Instruction {
    
    public SetEqF32() {
        // Set opcode
        this.Opcode = 0x98; 
        
        // Arguments

        // Stack
        this.AddStackOperand("lhs");
        this.AddStackOperand("rhs");
        this.AddStackReturn("boolean");
    }

    public override string Description => "Compares the top 2 elements on the stack and returns 1 if the equality comparison holds, returns 0 otherwise.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = runtime.Stack.PopTop();
        var lhs = runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.Float32 == rhs.Float32 ? 1 : 0));
    }
}


/// <summary>
/// Compares for inequality 
/// </summary>
public class SetNeqI32 : Instruction {
    
    public SetNeqI32() {
        // Set opcode
        this.Opcode = 0x99; 
        
        // Arguments

        // Stack
        this.AddStackOperand("lhs");
        this.AddStackOperand("rhs");
        this.AddStackReturn("boolean");
    }

    public override string Description => "Compares the top 2 elements on the stack and returns 1 if the inequality comparison holds, returns 0 otherwise.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = runtime.Stack.PopTop();
        var lhs = runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.Int32 != rhs.Int32 ? 1 : 0));
    }
}

/// <summary>
/// Compares for inequality 
/// </summary>
public class SetNeqU32 : Instruction {
    
    public SetNeqU32() {
        // Set opcode
        this.Opcode = 0x9A; 
        
        // Arguments

        // Stack
        this.AddStackOperand("lhs");
        this.AddStackOperand("rhs");
        this.AddStackReturn("boolean");
    }

    public override string Description => "Compares the top 2 elements on the stack and returns 1 if the inequality comparison holds, returns 0 otherwise.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = runtime.Stack.PopTop();
        var lhs = runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.UInt32 != rhs.UInt32 ? 1 : 0));
    }
}

/// <summary>
/// Compares for inequality 
/// </summary>
public class SetNeqF32 : Instruction {
    
    public SetNeqF32() {
        // Set opcode
        this.Opcode = 0x9B; 
        
        // Arguments

        // Stack
        this.AddStackOperand("lhs");
        this.AddStackOperand("rhs");
        this.AddStackReturn("boolean");
    }

    public override string Description => "Compares the top 2 elements on the stack and returns 1 if the inequality comparison holds, returns 0 otherwise.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = runtime.Stack.PopTop();
        var lhs = runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.Float32 != rhs.Float32 ? 1 : 0));
    }
}