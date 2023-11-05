namespace Qkmaxware.Vm.Ir;

/// <summary>
/// A contiguous block of IR statements
/// </summary>
public class Block : IrNode {
    private List<Statement> statements = new List<Statement>();

    /// <summary>
    /// Create an empty block
    /// </summary>
    public Block () {}

    /// <summary>
    /// Create a block containing the given list of statements
    /// </summary>
    /// <param name="statements">statements</param>
    public Block(params Statement[] statements) {
        this.statements.EnsureCapacity(statements.Length);
        this.statements.AddRange(statements);
    }

    /// <summary>
    /// Create a block copying the statements of another block
    /// </summary>
    /// <param name="other">block to copy statements from</param>
    public Block(Block other) {
        this.statements.EnsureCapacity(other.statements.Count);
        this.statements.AddRange(other.statements);
    }

    /// <summary>
    /// Add a single statement to this block
    /// </summary>
    /// <param name="statement">statement to add</param>
    public void Add(Statement statement) {
        this.statements.Add(statement);
    }

    /// <summary>
    /// Add several statements to this block
    /// </summary>
    /// <param name="stmts">statements</param>
    public void AddAll(params Statement[] stmts) {
        this.statements.AddRange(stmts);
    }

    /// <summary>
    /// Add several statements to this block
    /// </summary>
    /// <param name="stmts">statements</param>
    public void AddRange(IEnumerable<Statement> stmts) {
        this.statements.AddRange(stmts);
    }

    public override void ToBytecode(ModuleBuilder builder) {
        foreach (var stmt in this.statements) {
            stmt.ToBytecode(builder);
        }
    }
}