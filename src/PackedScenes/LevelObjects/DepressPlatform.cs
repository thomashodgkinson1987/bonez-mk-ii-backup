using Godot;

public class DepressPlatform : StaticBody2D
{

	#region Nodes

	public Sprite node_sprite { get; private set; }
	public CPUParticles2D node_particles { get; private set; }

	#endregion // Nodes



	#region Exports

	[Export] public Vector2 Offset { get; set; } = Vector2.Down;

	#endregion // Exports



	#region Fields

	private int m_overlappingCount = 0;

	#endregion // Fields



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		node_sprite = GetNode<Sprite>("Sprite");
		node_particles = GetNode<CPUParticles2D>("CPUParticles2D");
	}

	#endregion // Godot methods



	#region Area2D callbacks

	private void OnBodyEnteredTriggerArea (PhysicsBody2D body)
	{
		m_overlappingCount++;

		if (m_overlappingCount == 1) Depress();
	}

	private void OnBodyExitedTriggerArea (PhysicsBody2D body)
	{
		m_overlappingCount--;

		if (m_overlappingCount == 0) Unpress();
	}

	private void OnAreaEnteredTriggerArea (Area2D area)
	{
		m_overlappingCount++;

		if (m_overlappingCount == 1) Depress();
	}

	private void OnAreaExitedTriggerArea (Area2D area)
	{
		m_overlappingCount--;

		if (m_overlappingCount == 0) Unpress();
	}

	#endregion // Area2D callbacks



	#region Private methods

	private void Depress ()
	{
		node_sprite.Offset = Offset;
		node_particles.Emitting = true;
	}

	private void Unpress ()
	{
		node_sprite.Offset = Vector2.Zero;
		node_particles.Emitting = false;
	}

	#endregion // Private methods

}
