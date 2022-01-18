using Godot;

public class Projectile : Area2D
{

	#region Nodes

	public AnimatedSprite node_animatedSprite { get; private set; }
	public CollisionShape2D node_collisionShape { get; private set; }

	#endregion // Nodes



	#region Signals

	[Signal] public delegate void OnFiredSignal (Projectile projectile);

	[Signal] public delegate void OnAreaEnteredAreaSignal (Projectile projectile, Area area);
	[Signal] public delegate void OnBodyEnteredAreaSignal (Projectile projectile, PhysicsBody2D body);

	[Signal] public delegate void OnReachedLifeTimeSignal (Projectile projectile);

	[Signal] public delegate void OnDestroySignal (Projectile projectile);

	#endregion // Signals



	#region Exports

	[Export] public Vector2 Direction { get; set; } = Vector2.Right;
	[Export] public float Speed { get; set; } = 32f;
	[Export] public int Damage { get; set; } = 1;

	[Export] public PackedScene OneShotAnimationPackedScene { get; set; }
	[Export] public PackedScene OneShotAudioPackedScene { get; set; }

	[Export] public bool IsDestroyOnCollision { get; set; } = true;

	[Export] public float LifeTime { get; set; } = 10f;
	[Export] public float LifeTimeTimer { get; set; } = 0f;

	#endregion // Exports



	#region Fields

	private bool m_hasOneShotAnimationFinished = false;
	private bool m_hasOneShotAudioFinished = false;

	#endregion // Fields



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		node_animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		node_collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");

		Rotation = Direction.Angle();

		EmitSignal(nameof(OnFiredSignal), this);
	}

	public override void _PhysicsProcess (float delta)
	{
		base._PhysicsProcess(delta);

		if (m_hasOneShotAnimationFinished && m_hasOneShotAudioFinished)
		{
			QueueFree();
		}

		if (IsQueuedForDeletion()) return;

		Rotation = Direction.Angle();
		Position += Direction * Speed * delta;

		if (LifeTimeTimer < LifeTime)
		{
			LifeTimeTimer += delta;
			if (LifeTimeTimer >= LifeTime)
			{
				EmitSignal(nameof(OnReachedLifeTimeSignal), this);
			}
		}
	}

	#endregion // Godot methods



	#region Area2D callbacks

	protected virtual void OnAreaEntered (Area2D area)
	{
		EmitSignal(nameof(OnAreaEnteredAreaSignal), this, area);

		if (area is BasePlant) return;

		if (IsDestroyOnCollision) Destroy();
	}

	protected virtual void OnBodyEntered (PhysicsBody2D body)
	{
		EmitSignal(nameof(OnBodyEnteredAreaSignal), this, body);

		if (body is BreakableBlock breakableBlock) breakableBlock.Break();

		if (IsDestroyOnCollision) Destroy();
	}

	#endregion // Area2D callbacks



	#region Public methods

	public void Destroy ()
	{
		if (IsQueuedForDeletion()) return;

		EmitSignal(nameof(OnDestroySignal), this);

		OnDestroy();
	}

	#endregion // Public methods



	#region Private methods

	private void DisableCollision ()
	{
		CollisionLayer = 0;
		CollisionMask = 0;
		node_animatedSprite.Visible = false;
		node_collisionShape.Disabled = true;
	}

	private void OnDestroy ()
	{
		CallDeferred(nameof(DisableCollision));
		Speed = 0f;

		if (OneShotAnimationPackedScene != null)
		{
			OneShotAnimation oneShotAnimation = (OneShotAnimation)OneShotAnimationPackedScene.Instance();
			AddChild(oneShotAnimation);
			oneShotAnimation.GlobalPosition = GlobalPosition;
			oneShotAnimation.Connect(nameof(OneShotAnimation.OnDestroySignal), this, nameof(OnOneShotAnimationFinished));
		}
		else
		{
			m_hasOneShotAnimationFinished = true;
		}

		if (OneShotAudioPackedScene != null)
		{
			OneShotAudio oneShotAudio = (OneShotAudio)OneShotAudioPackedScene.Instance();
			AddChild(oneShotAudio);
			oneShotAudio.Connect(nameof(OneShotAudio.OnDestroySignal), this, nameof(OnOneShotAudioFinished));
		}
		else
		{
			m_hasOneShotAudioFinished = true;
		}
	}

	#endregion // Private methods



	#region Callback methods

	private void OnReachedLifeTime (Projectile projectile)
	{
		Destroy();
	}

	private void OnOneShotAnimationFinished (OneShotAnimation oneShotAnimation)
	{
		m_hasOneShotAnimationFinished = true;
	}
	private void OnOneShotAudioFinished (OneShotAudio oneShotAudio)
	{
		m_hasOneShotAudioFinished = true;
	}

	#endregion // Callback methods

}
