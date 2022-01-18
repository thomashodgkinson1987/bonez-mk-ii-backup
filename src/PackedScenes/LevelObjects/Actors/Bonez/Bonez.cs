using Godot;
using Godot.Collections;

public class Bonez : Actor
{

	#region Nodes

	public Node2D node_projectileSpawnPoint { get; private set; }

	public AudioStreamPlayer node_audioPlayer_jump { get; private set; }
	public AudioStreamPlayer node_audioPlayer_die { get; private set; }
	public AudioStreamPlayer node_audioPlayer_shoot { get; private set; }
	public AudioStreamPlayer node_audioPlayer_collectItem { get; private set; }
	public AudioStreamPlayer node_audioPlayer_hit { get; private set; }

	#endregion // Nodes



	#region Signals

	[Signal] public delegate void OnCollectedBoneSignal (Bonez bonez, Bone bone);
	[Signal] public delegate void OnCollectedHeartSignal (Bonez bonez, Heart heart);
	[Signal] public delegate void OnFireRateTimerChangedSignal (Bonez bonez, float fireRate, float fireRateTimer);

	#endregion // Signals



	#region Backing fields

	private float _fireRateTimer = 0f;

	#endregion // Backing fields



	#region Exports

	[Export]
	public float WalkSpeed
	{
		get => WalkState.Speed;
		set => WalkState.Speed = value;
	}

	[Export]
	public int JumpCount
	{
		get => JumpState.JumpCount;
		set => JumpState.JumpCount = value;
	}
	[Export]
	public int JumpLimit
	{
		get => JumpState.JumpLimit;
		set => JumpState.JumpLimit = value;
	}
	[Export]
	public float JumpHeight
	{
		get => JumpState.JumpHeight;
		set => JumpState.JumpHeight = value;
	}

	[Export] public bool IsJump { get; set; } = false;
	[Export] public bool IsAttack { get; set; } = false;

	[Export] public float HitTime { get; set; } = 1f;
	[Export] public float HitTimer { get; set; } = 0f;

	[Export] public float FireRate { get; set; } = 0.5f;
	[Export]
	public float FireRateTimer
	{
		get => _fireRateTimer;
		set
		{
			_fireRateTimer = value;
			EmitSignal(nameof(OnFireRateTimerChangedSignal), this, FireRate, _fireRateTimer);
		}
	}

	[Export] public PackedScene ProjectilePackedScene { get; set; }

	#endregion // Exports



	#region States

	public BonezState_Idle IdleState { get; private set; }
	public BonezState_Walk WalkState { get; private set; }
	public BonezState_Fall FallState { get; private set; }
	public BonezState_Jump JumpState { get; private set; }
	public BonezState_Hit HitState { get; private set; }
	public BonezState_Die DieState { get; private set; }

	#endregion // States



	#region Constructors

	public Bonez () : base()
	{
		IdleState = new BonezState_Idle(this, this);
		WalkState = new BonezState_Walk(this, this);
		FallState = new BonezState_Fall(this, this);
		JumpState = new BonezState_Jump(this, this);
		HitState = new BonezState_Hit(this, this);
		DieState = new BonezState_Die(this, this);
	}

	#endregion // Constructors



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		node_projectileSpawnPoint = GetNode<Node2D>("ProjectileSpawnPoint");
		node_audioPlayer_collectItem = GetNode<AudioStreamPlayer>("AudioPlayer_CollectItem");
		node_audioPlayer_die = GetNode<AudioStreamPlayer>("AudioPlayer_Die");
		node_audioPlayer_hit = GetNode<AudioStreamPlayer>("AudioPlayer_Hit");
		node_audioPlayer_shoot = GetNode<AudioStreamPlayer>("AudioPlayer_Shoot");
		node_audioPlayer_jump = GetNode<AudioStreamPlayer>("AudioPlayer_Jump");

		FireRateTimer = FireRate;

		m_state = IdleState;
		m_state.OnEnter();
	}

	public override void _PhysicsProcess (float delta)
	{
		base._PhysicsProcess(delta);

		if (FireRateTimer < FireRate)
		{
			FireRateTimer += delta;

			if (FireRateTimer >= FireRate)
			{
				FireRateTimer = FireRate;
			}
		}
	}

	#endregion // Godot methods



	#region Public methods

	public void Jump ()
	{
		IsJump = false;

		if (JumpCount < JumpLimit)
		{
			node_audioPlayer_jump.Play();
			JumpCount++;
			Velocity = new Vector2()
			{
				x = Velocity.x,
				y = -Mathf.Sqrt(2 * Gravity * JumpHeight)
			};
		}
	}

	public void Attack ()
	{
		if (FireRateTimer >= FireRate)
		{
			FireRateTimer = 0f;

			node_audioPlayer_shoot.Play();

			Projectile projectile = (Projectile)ProjectilePackedScene.Instance();
			projectile.GlobalPosition = node_projectileSpawnPoint.GlobalPosition;
			projectile.Direction = new Vector2(FacingDirection.x, 0f);
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
	}

	public void ResolveInput (float delta)
	{
		Vector2 velocity = Velocity;

		if (velocity.x >= -MaxVelocity.x && velocity.x <= MaxVelocity.x)
		{
			if (InputDirection.x != 0)
			{
				float vx = velocity.x + ((Acceleration * InputDirection.x) * delta);
				vx = Mathf.Clamp(vx, -MaxVelocity.x, MaxVelocity.x);
				velocity.x = vx;
			}
		}

		Velocity = velocity;

		if (IsAttack)
		{
			IsAttack = false;
			Attack();
		}
	}

	#endregion // Public methods



	#region Protected methods

	protected override void OnPollInput ()
	{
		base.OnPollInput();

		Vector2 inputDirection = Vector2.Zero;

		if (Input.IsActionPressed("left")) inputDirection.x -= 1;
		if (Input.IsActionPressed("right")) inputDirection.x += 1;
		if (Input.IsActionPressed("up")) inputDirection.y -= 1;
		if (Input.IsActionPressed("down")) inputDirection.y += 1;

		InputDirection = inputDirection;

		if (Input.IsActionJustPressed("jump")) IsJump = true;
		if (Input.IsActionJustPressed("attack")) IsAttack = true;
	}

	protected override void OnUpdateFacingDirectionX ()
	{
		base.OnUpdateFacingDirectionX();

		if (node_animatedSprite.FlipH)
			node_projectileSpawnPoint.Position = new Vector2()
			{
				x = -Mathf.Abs(node_projectileSpawnPoint.Position.x),
				y = node_projectileSpawnPoint.Position.y
			};
		else
			node_projectileSpawnPoint.Position = new Vector2()
			{
				x = Mathf.Abs(node_projectileSpawnPoint.Position.x),
				y = node_projectileSpawnPoint.Position.y
			};
	}

	protected override void OnAreaEnteredInteractArea (Area2D area)
	{
		base.OnAreaEnteredInteractArea(area);

		// TODO: Move these to Actor.cs?

		if (area is Bone bone)
		{
			EmitSignal(nameof(OnCollectedBoneSignal), this, bone);
			node_audioPlayer_collectItem.Play();
		}
		else if (area is Heart heart)
		{
			EmitSignal(nameof(OnCollectedHeartSignal), this, heart);
			node_audioPlayer_collectItem.Play();
			Heal(1);
		}
	}

	#endregion // Protected methods

}
