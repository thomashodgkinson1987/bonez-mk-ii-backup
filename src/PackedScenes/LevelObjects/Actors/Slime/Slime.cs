using Godot;

public class Slime : Actor
{

	#region Nodes

	public RayCast2D node_rayCast_leftWall { get; private set; }
	public RayCast2D node_rayCast_rightWall { get; private set; }
	public RayCast2D node_rayCast_leftFloor { get; private set; }
	public RayCast2D node_rayCast_rightFloor { get; private set; }

	#endregion // Nodes



	#region Exports

	[Export] public float WalkSpeed { get; set; } = 16f;
	[Export] public int WalkDirection { get; set; } = 1;

	[Export] public float IdleTime { get; set; } = 1f;
	[Export] public float IdleTimeTimer { get; set; } = 0f;

	#endregion // Exports



	#region States

	public SlimeState_Idle IdleState { get; private set; }
	public SlimeState_Walk WalkState { get; private set; }
	public SlimeState_Fall FallState { get; private set; }
	public SlimeState_Die DieState { get; private set; }

	#endregion // States



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		node_rayCast_leftWall = GetNode<RayCast2D>("RayCast2D_LeftWall");
		node_rayCast_rightWall = GetNode<RayCast2D>("RayCast2D_RightWall");
		node_rayCast_leftFloor = GetNode<RayCast2D>("RayCast2D_LeftFloor");
		node_rayCast_rightFloor = GetNode<RayCast2D>("RayCast2D_RightFloor");

		IdleState = new SlimeState_Idle(this, this);
		WalkState = new SlimeState_Walk(this, this);
		FallState = new SlimeState_Fall(this, this);
		DieState = new SlimeState_Die(this, this);

		m_state = IdleState;
		m_state.OnEnter();
	}

	#endregion // Godot methods



	#region Public methods

	public bool IsWallLeft () => node_rayCast_leftWall.IsColliding();
	public bool IsWallRight () => node_rayCast_rightWall.IsColliding();
	public bool IsFloorLeft () => node_rayCast_leftFloor.IsColliding();
	public bool IsFloorRight () => node_rayCast_rightFloor.IsColliding();

	#endregion // Public methods

}
