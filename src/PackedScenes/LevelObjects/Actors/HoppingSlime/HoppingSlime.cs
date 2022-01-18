using Godot;
using Godot.Collections;

public class HoppingSlime : Actor
{

	#region Nodes

	public RayCast2D node_rayCast2D_leftWall { get; private set; }
	public RayCast2D node_rayCast2D_rightWall { get; private set; }
	public RayCast2D node_rayCast2D_target { get; private set; }

	#endregion // Nodes



	#region States

	public HoppingSlimeState_Walk_Slow WalkSlowState { get; private set; }
	public HoppingSlimeState_Walk_Fast WalkFastState { get; private set; }

	public HoppingSlimeState_Hop_Mini HopMiniState { get; private set; }
	public HoppingSlimeState_Hop_Small HopSmallState { get; private set; }
	public HoppingSlimeState_Hop_Big HopBigState { get; private set; }
	public HoppingSlimeState_Hopping HoppingState { get; private set; }

	public HoppingSlimeState_Fall FallState { get; private set; }
	public HoppingSlimeState_Die DieState { get; private set; }

	public HoppingSlimeState_Random RandomState { get; set; }

	#endregion // States



	#region Exports

	[Export] public float SmallHopHeight { get; set; } = 64f;
	[Export] public float SmallHopDistance { get; set; } = 64f;

	[Export] public float MiniHopHeight { get; set; } = 32f;
	[Export] public float MiniHopDistance { get; set; } = 0f;

	[Export] public float BigHopHeight { get; set; } = 128f;
	[Export] public float BigHopDistance { get; set; } = 128f;

	[Export] public float SlowWalkSpeed { get; set; } = 32f;
	[Export] public float FastWalkSpeed { get; set; } = 64f;

	#endregion // Exports



	#region Properties

	public Actor Target { get; set; }

	public bool CanSeeTarget { get; set; } = false;

	#endregion // Properties



	#region Constructors

	public HoppingSlime () : base()
	{
		WalkSlowState = new HoppingSlimeState_Walk_Slow(this, this);
		WalkFastState = new HoppingSlimeState_Walk_Fast(this, this);

		HopMiniState = new HoppingSlimeState_Hop_Mini(this, this);
		HopSmallState = new HoppingSlimeState_Hop_Small(this, this);
		HopBigState = new HoppingSlimeState_Hop_Big(this, this);
		HoppingState = new HoppingSlimeState_Hopping(this, this);

		FallState = new HoppingSlimeState_Fall(this, this);
		DieState = new HoppingSlimeState_Die(this, this);
		RandomState = new HoppingSlimeState_Random(this, this);
	}

	#endregion // Constructors



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		node_rayCast2D_leftWall = GetNode<RayCast2D>("RayCast2D_LeftWall");
		node_rayCast2D_rightWall = GetNode<RayCast2D>("RayCast2D_RightWall");
		node_rayCast2D_target = GetNode<RayCast2D>("RayCast2D_Target");

		m_state = WalkSlowState;
		m_state.OnEnter();
	}

	public override void _PhysicsProcess (float delta)
	{
		CanSeeTarget = false;

		if (Target == null)
		{
			// TODO: This needs to be generalised around Actors and not Bonez
			Array nodesInGroup = GetTree().GetNodesInGroup("bonez");
			if (nodesInGroup.Count > 0) Target = (Actor)nodesInGroup[0];
			node_rayCast2D_target.Enabled = Target != null;
		}

		if (Target != null)
		{
			node_rayCast2D_target.CastTo = Target.node_center.GlobalPosition - node_rayCast2D_target.GlobalPosition;
			node_rayCast2D_target.ForceRaycastUpdate();
			CanSeeTarget = !node_rayCast2D_target.IsColliding();
		}

		base._PhysicsProcess(delta);
	}

	#endregion // Godot methods

}
