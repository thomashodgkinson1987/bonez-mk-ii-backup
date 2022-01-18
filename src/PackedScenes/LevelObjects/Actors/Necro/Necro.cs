using Godot;
using Godot.Collections;

public class Necro : Actor
{

	#region Nodes

	public Node2D node_necroBlastSpawnPosition { get; private set; }

	#endregion // Nodes



	#region Exports

	[Export] public bool IsShooting { get; set; } = false;
	[Export] public float FireRate { get; set; } = 1f;
	[Export] public float FireRateTimer { get; set; } = 0f;

	[Export] public PackedScene NecroBlastPackedScene { get; set; }
	[Export] public float NecroBlastSpeed { get; set; }
	[Export] public int NecroBlastDamage { get; set; }

	#endregion // Exports



	#region Properties

	public NecroState_Idle IdleState { get; private set; }
	public NecroState_Die DieState { get; private set; }

	public Node2D Target { get; set; }

	#endregion // Properties



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		node_necroBlastSpawnPosition = GetNode<Node2D>("NecroBlastSpawnPosition");

		IdleState = new NecroState_Idle(this, this);
		DieState = new NecroState_Die(this, this);

		m_state = IdleState;
		m_state.OnEnter();
	}

	public override void _PhysicsProcess (float delta)
	{
		base._PhysicsProcess(delta);
	}

	public override void _Process (float delta)
	{
		base._Process(delta);

		if (IsShooting)
		{
			FireRateTimer += delta;
			if (FireRateTimer >= FireRate)
			{
				FireRateTimer = 0f;
				ShootNecroBlast();
			}
		}
		else
		{
			FireRateTimer = 0f;
		}
	}

	#endregion // Godot methods



	#region Public methods

	public void ShootNecroBlast ()
	{
		if (Target != null)
		{
			Projectile necroBlast = (Projectile)NecroBlastPackedScene.Instance();

			Array nodesInGroup = GetTree().GetNodesInGroup("projectiles_holder");
			if (nodesInGroup.Count > 0)
			{
				Node2D node = (Node2D)nodesInGroup[0];
				node.AddChild(necroBlast);
			}
			else if (Owner != null)
			{
				Owner.AddChild(necroBlast);
			}
			else if (GetParentOrNull<Node2D>() != null)
			{
				GetParent<Node2D>().AddChild(necroBlast);
			}
			else
			{
				GetTree().Root.AddChild(necroBlast);
			}

			necroBlast.GlobalPosition = node_necroBlastSpawnPosition.GlobalPosition;

			necroBlast.Direction = (Target.GlobalPosition - necroBlast.GlobalPosition).Normalized();
			necroBlast.Speed = NecroBlastSpeed;
			necroBlast.Damage = NecroBlastDamage;
		}
	}

	#endregion // Public methods

}
