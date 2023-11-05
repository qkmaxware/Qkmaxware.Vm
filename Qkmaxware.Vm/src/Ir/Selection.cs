namespace Qkmaxware.Vm.Ir;

/// <summary>
/// A branch which can be selected via a selection statement
/// </summary>
public class Branch {
    /// <summary>
    /// Branch condition
    /// </summary>
    public Expression Condition {get; private set;}
    /// <summary>
    /// Body to execute if condition holds
    /// </summary>
    public Block Body {get; private set;}

    public Branch(Expression condition, Block body) {
        this.Condition = condition;
        this.Body = body;
    }
}

/// <summary>
/// A selection statement
/// </summary>
public class Selection : Statement {
    private List<Branch> branches = new List<Branch>();

    private Selection() {}

    public Selection(Branch branch) {
        this.branches.Add(branch);
    }

    public Selection(Expression condition, Block body) {
        this.branches.Add(new Branch(condition, body));
    }

    #region Fluid API
    /// <summary>
    /// Create a selection statement for the given branch
    /// </summary>
    /// <param name="branch">branch to check</param>
    /// <returns>selection with 1 branch</returns>
    public static Selection If(Branch branch) {
        return new Selection(branch);
    }
    /// <summary>
    /// Create a selection statement for the given branch
    /// </summary>
    /// <param name="condition">condition to check</param>
    /// <param name="body">branch to execute if condition holds</param>
    /// <returns>selection with 1 branch</returns>
    public static Selection If(Expression condition, Block body) {
        return If(new Branch(condition, body));
    }
    /// <summary>
    /// Create a branch alternative
    /// </summary>
    /// <param name="branch">branch to check</param>
    /// <returns>selection with one more branch</returns>
    public Selection ElseIf(Branch branch) {
        var conditional = new Selection();
        conditional.branches.AddRange(this.branches);
        conditional.branches.Add(branch);
        return conditional;
    }
    /// <summary>
    /// Create a branch alternative
    /// </summary>
    /// <param name="condition">condition to check</param>
    /// <param name="body">branch to execute if condition holds</param>
    /// <returns>selection with one more branch</returns>
    public Selection ElseIf(Expression condition, Block body) {
        return this.ElseIf(new Branch(condition, body));
    }
    // TODO ELSE 
    #endregion

    public override void ToBytecode(ModuleBuilder builder) {
        /*
            .if_block
                .first_condition
                    ...
                    goto_if_zero .second_condition
                    ...
                    goto .if_end
                .second_condition
                    ...
                    goto_if_zero .third_condition
                    ...
                    goto .if_end
                ...
                .last_condition
                    goto_if_zero .if_end
                    ...
                    goto .if_end
            .if_end
        */
        var if_block = builder.Anchor();
        var if_end = if_block;
        var branch_anchors = new List<(long start, long false_jump, long true_jump, long end)>(this.branches.Count);
        foreach (var branch in this.branches) {
            var branch_anchor = builder.Anchor();

            // Evaluate condition
            branch.Condition.ToBytecode(builder);

            // Check condition and branch if false
            var jump_anchor = builder.Anchor();
            builder.GotoIfStackTopZero(0); // FIX 1: Temporarily set this to 0 to fix later

            // Do body
            branch.Body.ToBytecode(builder);
            var branch_end = builder.Anchor();
            builder.Goto(0);               // FIX 2: Temporarily set this to 0 to fix later
            branch_anchors.Add((start: branch_anchor, false_jump: jump_anchor, true_jump: branch_end, end: builder.Anchor()));
        }
        if_end = builder.Anchor();

        // Fix labels
        for (var i = 0; i < branch_anchors.Count; i++) {
            // Fix conditional jumps
            var now = builder.Anchor();
            var next_anchor = ((i + 1) < branch_anchors.Count) ? branch_anchors[i + 1].start : if_end;
            builder.RewindStream(branch_anchors[i].false_jump);
            builder.GotoIfStackTopZero(next_anchor); // FIX 1: Fill in jump to next branch

            // Fix end jump
            builder.RewindStream(branch_anchors[i].true_jump);
            builder.Goto(if_end); // FIX 2: Fill in jump to the end of the if statement
            builder.RewindStream(now);
        }
    }
}