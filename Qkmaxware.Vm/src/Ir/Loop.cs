using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Qkmaxware.Vm.Ir;

/// <summary>
/// Base class for all loop statement types
/// </summary>
public abstract class Loop : Statement {
    #region Fluid API
    /// <summary>
    /// Create an unconditional loop which will repeat forever (eg: while (true) { ... })
    /// </summary>
    /// <param name="body">body to repeat</param>
    /// <returns>unconditional loop</returns>
    public static UnconditionalLoop Forever(Block body) {
        return new UnconditionalLoop(body);
    }
    /// <summary>
    /// Create a loop which iterates over a variable until a condition is met (eg: for (var x = 0; x < y; x++) { ... })
    /// </summary>
    /// <param name="init">variable initialization</param>
    /// <param name="condition">repeat condition</param>
    /// <param name="increment">variable increment expression</param>
    /// <param name="body">body to repeat</param>
    /// <returns>iteration loop</returns>
    public static IterationLoop For(Statement init, Expression condition, Statement increment, Block body) {
        return new IterationLoop(init, condition, increment, body);
    }
    /// <summary>
    /// Create a conditional loop which runs while a condition holds (eg: while (expr) { ... })
    /// </summary>
    /// <param name="condition">repeat condition</param>
    /// <param name="body">body to repeat</param>
    /// <returns>conditional loop</returns>
    public static ConditionalLoop While(Expression condition, Block body) {
        return new ConditionalLoop(condition, body);
    }
    #endregion
}

public class UnconditionalLoop : Loop {
    public Block Body {get; private set;}
    public UnconditionalLoop(Block body) {
        this.Body = body;
    }

    public override void ToBytecode(ModuleBuilder builder) {
        /*
            .loop_start
                condition
                goto_if_zero .loop_end
                ...
                goto .loop_start
            .loop_end
        */

        // Loop condition
        var loop_start_anchor = builder.Anchor();

        // Loop body
        this.Body.ToBytecode(builder);

        // Loop end
        builder.Goto(loop_start_anchor);
        var end_anchor = builder.Anchor();

        // TODO repair any inner break or continue statements
    }
}

public class ConditionalLoop : Loop {
    public Expression Condition {get; private set;}
    public Block Body {get; private set;}

    public ConditionalLoop(Expression condition, Block body) {
        this.Condition = condition;
        this.Body = body;
    }

    public override void ToBytecode(ModuleBuilder builder) {
        /*
            .loop_start
                condition
                goto_if_zero .loop_end
                ...
                goto .loop_start
            .loop_end
        */

        // Loop condition
        var loop_start_anchor = builder.Anchor();
        this.Condition.ToBytecode(builder);
        var condition_exit_anchor = builder.Anchor();
        builder.GotoIfStackTopZero(loop_start_anchor);

        // Loop body
        this.Body.ToBytecode(builder);
        builder.Goto(loop_start_anchor);

        // Loop end
        var end_anchor = builder.Anchor();
        builder.RewindStream(condition_exit_anchor);
        builder.GotoIfStackTopZero(end_anchor);
        builder.RewindStream(end_anchor);

        // TODO repair any inner break or continue statements
    }
}

public class IterationLoop : Loop {
    public Statement Initializer {get; private set;}
    public Expression Condition {get; private set;}
    public Statement Incrementor {get; private set;}
    public Block Body {get; private set;}

    public IterationLoop(Statement init, Expression condition, Statement increment, Block body) {
        this.Initializer = init;
        this.Condition = condition;
        this.Incrementor = increment;
        this.Body = body;
    }

    public override void ToBytecode(ModuleBuilder builder) {
        /*
            init
            .loop_start
                condition
                goto_if_zero .loop_end
                ...
                increment
                goto .loop_start
            .loop_end
        */
        // Initializer
        this.Initializer.ToBytecode(builder);

        // Loop condition
        var loop_start_anchor = builder.Anchor();
        this.Condition.ToBytecode(builder);
        var condition_exit_anchor = builder.Anchor();
        builder.GotoIfStackTopZero(loop_start_anchor);

        // Loop body
        this.Body.ToBytecode(builder);

        // Increment
        this.Incrementor.ToBytecode(builder);
        builder.Goto(loop_start_anchor);

        // Loop end
        var end_anchor = builder.Anchor();
        builder.RewindStream(condition_exit_anchor);
        builder.GotoIfStackTopZero(end_anchor);
        builder.RewindStream(end_anchor);

        // TODO repair any inner break or continue statements
    }
}