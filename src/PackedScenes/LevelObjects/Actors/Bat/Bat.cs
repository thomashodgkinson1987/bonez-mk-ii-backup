using Godot;
using Godot.Collections;

public class Bat : Actor
{

	#region Exports

	[Export] public float IdleTime { get; set; } = 1f;

	[Export] public float MoveSpeed { get; set; } = 32f;
	[Export] public float MoveTime { get; set; } = 1f;

	#endregion // Exports



	#region States

	public BatState_Idle IdleState { get; private set; }
	public BatState_Move MoveState { get; private set; }
	public BatState_Die DieState { get; private set; }

	#endregion // States



	#region Properties

	public float IdleTimer { get; set; } = 0f;
	public float MoveTimer { get; set; } = 0f;

	public Node2D Target { get; set; }

	public Vector2 LastTargetPosition { get; set; } = Vector2.Zero;

	#endregion // Properties



	#region Constructors

	public Bat () : base()
	{
		IdleState = new BatState_Idle(this, this);
		MoveState = new BatState_Move(this, this);
		DieState = new BatState_Die(this, this);
	}

	#endregion // Constructors



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		m_state = IdleState;
		m_state.OnEnter();
	}

	#endregion // Godot methods



	#region Private methods

	private void OnAreaEnteredScanArea (Area2D area)
	{
		Target = area;
	}

	private void OnAreaExitedScanArea (Area2D area)
	{
		Target = null;
	}

	private void OnBodyEnteredScanArea (PhysicsBody2D body)
	{
		Target = body;
	}

	private void OnBodyExitedScanArea (PhysicsBody2D body)
	{
		Target = null;
	}

	#endregion // Private methods

}
