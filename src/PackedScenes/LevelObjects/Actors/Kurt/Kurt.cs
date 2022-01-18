using Godot;
using Godot.Collections;

public class Kurt : Actor
{

	#region Nodes

	public Node2D node_projectileSpawnPoint { get; private set; }

	public KinematicBody2D node_backBody { get; private set; }
	public CollisionShape2D node_backBodyCollisionShape2D { get; private set; }

	#endregion // Nodes



	#region Fields

	private int m_turn = 1;
	private int m_turns = 3;

	#endregion // Fields



	#region Properties

	public KurtState_Idle IdleState { get; private set; }
	public KurtState_Walk WalkState { get; private set; }
	public KurtState_Die DieState { get; private set; }

	#endregion // Properties



	#region Export

	[Export] public PackedScene ProjectilePackedScene { get; set; }

	[Export] public float IdleTime { get; set; } = 1f;
	[Export] public float IdleTimeTimer { get; set; } = 0f;

	[Export] public float WalkSpeed { get; set; } = 32f;
	[Export] public int WalkDirection { get; set; } = 1;
	[Export] public float WalkTime { get; set; } = 1f;
	[Export] public float WalkTimeTimer { get; set; } = 0f;

	[Export] public float AttackTime { get; set; } = 1f;
	[Export] public float AttackTimeTimer { get; set; } = 0f;

	#endregion // Exports



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		node_projectileSpawnPoint = GetNode<Node2D>("ProjectileSpawnPoint");
		node_backBody = GetNode<KinematicBody2D>("BackBody");
		node_backBodyCollisionShape2D = node_backBody.GetNode<CollisionShape2D>("CollisionShape2D");

		IsUpdateFacingDirection = false;
		FacingDirection = new Vector2(Scale.x, 0f);

		IdleState = new KurtState_Idle(this, this);
		WalkState = new KurtState_Walk(this, this);
		DieState = new KurtState_Die(this, this);

		m_state = IdleState;
		m_state.OnEnter();
	}

	#endregion // Godot methods



	#region Public methods

	public void Attack ()
	{
		Projectile projectile = (Projectile)ProjectilePackedScene.Instance();
		projectile.GlobalPosition = node_projectileSpawnPoint.GlobalPosition;
		projectile.Direction = new Vector2(FacingDirection.x, 0f);

		switch (m_turn)
		{
			case 1:
			projectile.Direction += new Vector2(0f, 0f);
			break;

			case 2:
			projectile.Direction += new Vector2(0f, -0.3f);
			break;

			case 3:
			projectile.Direction += new Vector2(0f, -0.9f);
			break;

			default:
			projectile.Direction += new Vector2(0f, 0f);
			break;
		}

		m_turn = Mathf.Wrap(m_turn + 1, 1, m_turns + 1);

		projectile.Rotation = projectile.Direction.Angle();

		Array nodesInGroup = GetTree().GetNodesInGroup("projectiles_holder");
		if (nodesInGroup.Count > 0)
		{
			Node2D node = (Node2D)nodesInGroup[0];
			node.AddChild(projectile);
		}
		else if (Owner != null)
		{
			Owner.AddChild(projectile);
		}
		else if (GetParentOrNull<Node2D>() != null)
		{
			GetParent<Node2D>().AddChild(projectile);
		}
		else
		{
			GetTree().Root.AddChild(projectile);
		}
	}

	#endregion // Public methods

}
