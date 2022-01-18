using Godot;

public class Actor : KinematicBody2D
{

	#region Nodes

	public Node2D node_center { get; private set; }

	public CollisionShape2D node_bodyCollisionShape { get; private set; }

	public AnimatedSprite node_animatedSprite { get; private set; }

	public Area2D node_interactArea { get; private set; }
	public CollisionShape2D node_interactAreaCollisionShape { get; private set; }

	public Area2D node_hitArea { get; private set; }
	public CollisionShape2D node_hitAreaCollisionShape { get; private set; }

	public Area2D node_hurtArea { get; private set; }
	public CollisionShape2D node_hurtAreaCollisionShape { get; private set; }

	#endregion // Nodes



	#region Signals

	[Signal] public delegate void OnHealthChangedSignal (Actor actor, int oldHealth, int newHealth);
	[Signal] public delegate void OnMaxHealthChangedSignal (Actor actor, int oldMaxHealth, int newMaxHealth);
	[Signal] public delegate void OnHealthReachedZeroSignal (Actor actor);


	#endregion // Signals



	#region Exports

	[Export] Vector2 SpriteOffsetOverride { get; set; } = Vector2.Zero;

	[Export] public float Acceleration { get; set; } = 500f;
	[Export] public float Deceleration { get; set; } = 500f;

	[Export] public Vector2 Velocity { get; set; } = Vector2.Zero;
	[Export] public Vector2 MaxVelocity { get; set; } = new Vector2(500f, 500f);

	[Export] public float Gravity { get; set; } = 300f;

	[Export] public Vector2 Snap { get; set; } = Vector2.Zero;

	[Export] public Vector2 InputDirection { get; set; } = Vector2.Zero;
	[Export] public Vector2 FacingDirection { get; set; } = Vector2.Right;

	private int _health = 5;
	[Export]
	public int Health
	{
		get => _health;
		set
		{
			int oldHealth = _health;
			//_health = Mathf.Clamp(value, 0, MaxHealth);
			_health = value;
			if (oldHealth != _health)
			{
				EmitSignal(nameof(OnHealthChangedSignal), this, oldHealth, _health);
			}
			if (_health == 0)
			{
				EmitSignal(nameof(OnHealthReachedZeroSignal), this);
			}
		}
	}
	private int _maxHealth = 5;
	[Export]
	public int MaxHealth
	{
		get => _maxHealth;
		set
		{
			int oldMaxHealth = _maxHealth;
			_maxHealth = Mathf.Max(1, value);
			if (Health > _maxHealth) Health = MaxHealth;
			if (oldMaxHealth != _maxHealth)
			{
				EmitSignal(nameof(OnMaxHealthChangedSignal), this, oldMaxHealth, _maxHealth);
			}
		}
	}

	[Export] public bool CanTakeDamage { get; set; } = true;
	[Export] public bool CanHeal { get; set; } = true;
	[Export] public bool CanDie { get; set; } = true;

	[Export] public bool IsApplyDeceleration { get; set; } = true;
	[Export] public bool IsApplyDecelerationX { get; set; } = true;
	[Export] public bool IsApplyDecelerationY { get; set; } = false;

	[Export] public bool IsApplyGravity { get; set; } = true;

	[Export] public bool IsMove { get; set; } = true;

	[Export] public bool IsPollInput { get; set; } = true;

	[Export] public bool IsUpdateFacingDirection { get; set; } = true;
	[Export] public bool IsUpdateFacingDirectionX { get; set; } = true;
	[Export] public bool IsUpdateFacingDirectionY { get; set; } = false;

	[Export] public bool TookDamageThisFrame { get; set; } = false;

	[Export] public bool IsInvincible { get; set; } = false;
	[Export] public bool WasInvincible { get; set; } = false;
	[Export] public float InvincibleTime { get; set; } = 1f;
	[Export] public float InvincibleTimer { get; set; } = 0f;

	[Export] public bool IsFlashing { get; set; } = false;
	[Export] public bool WasFlashing { get; set; } = false;
	[Export] public float FlashRate { get; set; } = 0.12f;
	[Export] public float FlashTimer { get; set; } = 0f;

	[Export] public int HurtAreaDamage { get; set; } = 1;

	[Export] public Vector2 DieKnockback { get; set; } = new Vector2(32f, 32f);
	[Export] public Vector2 HitKnockback { get; set; } = new Vector2(32f, 32f);

	#endregion // Exports



	#region Fields

	protected ActorState m_state;

	#endregion // Fields



	#region Constructors

	public Actor () : base()
	{
		m_state = new ActorState_Null(this);
	}

	#endregion // Constructors



	#region Godot methods

	public override void _Ready ()
	{
		node_center = GetNode<Node2D>("Center");

		node_bodyCollisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		node_animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");

		node_interactArea = GetNode<Area2D>("InteractArea");
		node_interactAreaCollisionShape = node_interactArea.GetNode<CollisionShape2D>("CollisionShape2D");

		node_hitArea = GetNode<Area2D>("HitArea");
		node_hitAreaCollisionShape = node_hitArea.GetNode<CollisionShape2D>("CollisionShape2D");

		node_hurtArea = GetNode<Area2D>("HurtArea");
		node_hurtAreaCollisionShape = node_hurtArea.GetNode<CollisionShape2D>("CollisionShape2D");
	}

	public override void _PhysicsProcess (float delta)
	{
		base._PhysicsProcess(delta);

		m_state.OnPhysicsProcess(delta);

		if (IsInvincible)
		{
			if (!WasInvincible) OnBecameInvincible();
			OnWhileInvincible(delta);
			WasInvincible = true;
		}
		else if (WasInvincible)
		{
			OnBecameNotInvincible();
			WasInvincible = false;
		}

		if (IsFlashing)
		{
			FlashTimer += delta;
			if (FlashTimer >= FlashRate)
			{
				FlashTimer = 0f;
				Visible = !Visible;
			}
			WasFlashing = true;
		}
		else if (WasFlashing)
		{
			FlashTimer = 0f;
			Visible = true;
			WasFlashing = false;
		}
	}

	public override void _Process (float delta)
	{
		base._Process(delta);

		m_state.OnProcess(delta);

		TookDamageThisFrame = false;
	}

	#endregion // Godot methods



	#region Public methods

	public void SetState (ActorState state, bool forceApply = false)
	{
		if (m_state.StateName == state.StateName && !forceApply) return;

		m_state.OnExit();
		m_state = state;
		m_state.OnEnter();
	}

	public void PollInput ()
	{
		if (!IsPollInput) return;

		OnPollInput();
	}

	public void ApplyGravity (float delta)
	{
		if (!IsApplyGravity) return;

		OnApplyGravity(delta);
	}

	public void ApplyDeceleration (float delta)
	{
		if (!IsApplyDeceleration) return;

		OnApplyDeceleration(delta);
	}
	public void ApplyDecelerationX (float delta)
	{
		if (!IsApplyDeceleration || !IsApplyDecelerationX) return;

		OnApplyDecelerationX(delta);
	}
	public void ApplyDecelerationY (float delta)
	{
		if (!IsApplyDeceleration || !IsApplyDecelerationY) return;

		OnApplyDecelerationY(delta);
	}

	public void Move (Vector2 velocity)
	{
		if (!IsMove) return;

		OnMove(velocity);
	}
	public void Move () => Move(Velocity);

	public void UpdateFacingDirection ()
	{
		if (!IsUpdateFacingDirection) return;

		OnUpdateFacingDirection();
	}
	public void UpdateFacingDirectionX ()
	{
		if (!IsUpdateFacingDirection || !IsUpdateFacingDirectionX) return;

		OnUpdateFacingDirectionX();
	}
	public void UpdateFacingDirectionY ()
	{
		if (!IsUpdateFacingDirection || !IsUpdateFacingDirectionY) return;

		OnUpdateFacingDirectionY();
	}

	public void TakeDamage (int amount)
	{
		if (!CanTakeDamage || IsInvincible) return;

		OnTakeDamage(amount);
	}
	public void Heal (int amount)
	{
		if (!CanHeal) return;

		OnHeal(amount);
	}
	public void Die ()
	{
		if (!CanDie) return;

		OnDie();
	}

	public void FlipH ()
	{
		OnFlipH();
	}
	public void FlipV ()
	{
		OnFlipV();
	}

	#endregion // Public methods



	#region Protected methods

	protected virtual void OnBecameInvincible ()
	{
		IsFlashing = true;
		FlashTimer = 0f;
		InvincibleTimer = 0f;
		node_hitAreaCollisionShape.Disabled = true;
	}
	protected virtual void OnWhileInvincible (float delta)
	{
		InvincibleTimer += delta;
		if (InvincibleTimer >= InvincibleTime) IsInvincible = false;
	}
	protected virtual void OnBecameNotInvincible ()
	{
		IsFlashing = false;
		FlashTimer = 0f;
		InvincibleTimer = 0f;
		node_hitAreaCollisionShape.Disabled = false;
	}

	protected virtual void OnApplyGravity (float delta)
	{
		Vector2 velocity = Velocity;

		if (velocity.y >= -MaxVelocity.y && velocity.y <= MaxVelocity.y)
		{
			float vy = velocity.y + (Gravity * delta);
			velocity.y = Mathf.Clamp(vy, -MaxVelocity.y, MaxVelocity.y);
		}
		else if (velocity.y < -MaxVelocity.y)
		{
			velocity.y += Gravity * delta;
		}
		else
		{
			velocity.y = Mathf.Clamp(velocity.y, -MaxVelocity.y, MaxVelocity.y);
		}

		Velocity = velocity;
	}

	protected virtual void OnApplyDeceleration (float delta)
	{
		ApplyDecelerationX(delta);
		ApplyDecelerationY(delta);
	}
	protected virtual void OnApplyDecelerationX (float delta)
	{
		Vector2 velocity = Velocity;

		if (velocity.x < 0)
		{
			float vx = velocity.x + (Deceleration * delta);
			velocity.x = Mathf.Clamp(vx, -MaxVelocity.x, 0f);
		}
		else if (velocity.x > 0)
		{
			float vx = velocity.x - (Deceleration * delta);
			velocity.x = Mathf.Clamp(vx, 0f, MaxVelocity.x);
		}

		Velocity = velocity;
	}
	protected virtual void OnApplyDecelerationY (float delta)
	{
		Vector2 velocity = Velocity;

		if (velocity.y < 0)
		{
			float vy = velocity.y + (Deceleration * delta);
			velocity.y = Mathf.Clamp(vy, -MaxVelocity.y, 0f);
		}
		else if (velocity.y > 0)
		{
			float vy = velocity.y - (Deceleration * delta);
			velocity.y = Mathf.Clamp(vy, 0f, MaxVelocity.y);
		}

		Velocity = velocity;
	}

	protected virtual void OnUpdateFacingDirection ()
	{
		UpdateFacingDirectionX();
		UpdateFacingDirectionY();
	}
	protected virtual void OnUpdateFacingDirectionX ()
	{
		float originalFacingDirectionX = FacingDirection.x;

		FacingDirection = new Vector2()
		{
			x = Velocity.x < 0 ? -1 : Velocity.x > 0 ? 1 : FacingDirection.x,
			y = FacingDirection.y
		};

		if (FacingDirection.x != originalFacingDirectionX)
		{
			node_animatedSprite.FlipH = !node_animatedSprite.FlipH;

			float offsetX = FacingDirection.x == -1 ? -SpriteOffsetOverride.x : 0;
			node_animatedSprite.Offset = new Vector2(offsetX, node_animatedSprite.Offset.y);
		}
	}
	protected virtual void OnUpdateFacingDirectionY ()
	{
		Vector2 originalFacingDirection = FacingDirection;

		FacingDirection = new Vector2()
		{
			x = FacingDirection.x,
			y = Velocity.y < 0 ? -1 : Velocity.y > 0 ? 1 : FacingDirection.y
		};

		if (FacingDirection.y != originalFacingDirection.y)
		{
			node_animatedSprite.FlipV = !node_animatedSprite.FlipV;

			float offsetY = FacingDirection.y == -1 ? -SpriteOffsetOverride.y : 0;
			node_animatedSprite.Offset = new Vector2(node_animatedSprite.Offset.x, offsetY);
		}
	}

	protected virtual void OnPollInput () { }

	protected virtual void OnMove (Vector2 velocity)
	{
		Velocity = MoveAndSlideWithSnap(velocity, Snap, Vector2.Up);
	}

	protected virtual void OnTakeDamage (int amount)
	{
		Health -= amount;
		Health = Mathf.Clamp(Health, 0, MaxHealth);
		TookDamageThisFrame = true;
	}

	protected virtual void OnHeal (int amount)
	{
		Health += amount;
		Health = Mathf.Clamp(Health, 0, MaxHealth);
	}

	protected virtual void OnDie ()
	{
		QueueFree();
	}

	protected virtual void OnFlipH ()
	{
		node_animatedSprite.FlipH = !node_animatedSprite.FlipH;
	}
	protected virtual void OnFlipV ()
	{
		node_animatedSprite.FlipV = !node_animatedSprite.FlipV;

		if (node_animatedSprite.FlipV)
			node_animatedSprite.Offset = new Vector2(node_animatedSprite.Offset.x, SpriteOffsetOverride.y);
		else
			node_animatedSprite.Offset = new Vector2(node_animatedSprite.Offset.x, 0f);
	}

	#endregion // Protected methods



	#region Interact/Hit/Hurt area callbacks

	// Interact Area

	protected virtual void OnAreaEnteredInteractArea (Area2D area) { }
	protected virtual void OnAreaExitedInteractArea (Area2D area) { }
	protected virtual void OnBodyEnteredInteractArea (Area2D area) { }
	protected virtual void OnBodyExitedInteractArea (Area2D area) { }

	// Hit Area

	protected virtual void OnAreaEnteredHitArea (Area2D area)
	{
		if (area is Projectile projectile)
		{
			TakeDamage(projectile.Damage);
		}
		else if (area is Spike || area is Water)
		{
			TakeDamage(MaxHealth);
		}
		else if (area.Name == "HurtArea") // TODO: Make HitArea/HurtArea scenes
		{
			Actor actor = area.GetParent<Actor>();
			TakeDamage(actor.HurtAreaDamage);
		}
	}
	protected virtual void OnAreaExitedHitArea (Area2D area) { }
	protected virtual void OnBodyEnteredHitArea (PhysicsBody2D body) { }
	protected virtual void OnBodyExitedHitArea (PhysicsBody2D body) { }

	// Hurt Area

	protected virtual void OnAreaEnteredHurtArea (Area2D area) { }
	protected virtual void OnAreaExitedHurtArea (Area2D area) { }
	protected virtual void OnBodyEnteredHurtArea (PhysicsBody2D body) { }
	protected virtual void OnBodyExitedHurtArea (PhysicsBody2D body) { }

	#endregion // Interact/Hit/Hurt area callbacks

}
