namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Compares for less than 
/// </summary>
public class SetLessThanI32 : Instruction {
    
    public SetLessThanI32() {
        // Set opcode
        this.Opcode = 0x90; 
        
        // Arguments
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
public class SetLessThanU32 : Instruction {
    
    public SetLessThanU32() {
        // Set opcode
        this.Opcode = 0x91; 
        
        // Arguments
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
public class SetLessThanF32 : Instruction {
    
    public SetLessThanF32() {
        // Set opcode
        this.Opcode = 0x92; 
        
        // Arguments
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
public class SetGreaterThanI32 : Instruction {
    
    public SetGreaterThanI32() {
        // Set opcode
        this.Opcode = 0x93; 
        
        // Arguments
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
public class SetGreaterThanU32 : Instruction {
    
    public SetGreaterThanU32() {
        // Set opcode
        this.Opcode = 0x94; 
        
        // Arguments
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
public class SetGreaterThanF32 : Instruction {
    
    public SetGreaterThanF32() {
        // Set opcode
        this.Opcode = 0x95; 
        
        // Arguments
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
    }

    public override string Description => "Compares the top 2 elements on the stack and returns 1 if the inequality comparison holds, returns 0 otherwise.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = runtime.Stack.PopTop();
        var lhs = runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.Float32 != rhs.Float32 ? 1 : 0));
    }
}