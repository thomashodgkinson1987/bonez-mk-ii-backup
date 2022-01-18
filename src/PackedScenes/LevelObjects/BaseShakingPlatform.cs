using Godot;

public class BaseShakingPlatform : StaticBody2D
{

	public Sprite node_sprite;
	public CPUParticles2D node_particles;

	[Export] public float ShakeStrengthMin { get; set; } = 0.2f;
	[Export] public float ShakeStrengthMax { get; set; } = 1f;

	[Export] public float ShakeTimeMin { get; set; } = 0.2f;
	[Export] public float ShakeTimeMax { get; set; } = 0.5f;

	[Export] public float ShakeTimer { get; set; } = 0f;

	[Export] public bool IsShaking { get; set; } = false;
	[Export] public bool IsShakeQueued { get; set; } = false;

	private readonly RandomNumberGenerator m_rng = new RandomNumberGenerator();

	public override void _Ready ()
	{
		base._Ready();

		node_sprite = GetNode<Sprite>("Sprite");
		node_particles = GetNode<CPUParticles2D>("CPUParticles2D");

		m_rng.Randomize();
	}

	public override void _Process (float delta)
	{
		base._Process(delta);

		if (IsShakeQueued)
		{
			IsShakeQueued = false;
			IsShaking = true;
			ShakeTimer = m_rng.RandfRange(ShakeTimeMin, ShakeTimeMax);
			node_particles.Emitting = true;
		}

		if (IsShaking)
		{
			ShakeTimer -= delta;

			float offsetX = m_rng.RandfRange(ShakeStrengthMin, ShakeStrengthMax);
			float offsetY = m_rng.RandfRange(ShakeStrengthMin, ShakeStrengthMax);
			node_sprite.Offset = new Vector2(offsetX, offsetY);

			if (ShakeTimer <= 0)
			{
				IsShaking = false;
				node_sprite.Offset = Vector2.Zero;
			}
		}
	}

	private void OnBodyEntered (PhysicsBody2D body)
	{
		if (body is Actor actor)
		{
			if (actor.Velocity.y >= 0)
			{
				IsShakeQueued = true;
			}
		}
	}

	private void OnAreaEntered (Area2D area)
	{
		if (area is Projectile projectile)
		{
			IsShakeQueued = true;
		}
	}

}
