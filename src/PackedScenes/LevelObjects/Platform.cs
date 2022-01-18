using Godot;

public class Platform : KinematicBody2D
{

	public Sprite node_sprite;

	[Export] public Vector2 Offset { get; set; } = Vector2.Down;

	private int m_overlappingCount = 0;

	public override void _Ready ()
	{
		base._Ready();

		node_sprite = GetNode<Sprite>("Sprite");
	}

	private void OnBodyEnteredTriggerArea (PhysicsBody2D body)
	{
		m_overlappingCount++;

		if (m_overlappingCount == 1) node_sprite.Offset = Offset;
	}

	private void OnBodyExitedTriggerArea (PhysicsBody2D body)
	{
		m_overlappingCount--;

		if (m_overlappingCount == 0) node_sprite.Offset = Vector2.Zero;
	}

	private void OnAreaEnteredTriggerArea (Area2D area)
	{
		m_overlappingCount++;

		if (m_overlappingCount == 1) node_sprite.Offset = Offset;
	}

	private void OnAreaExitedTriggerArea (Area2D area)
	{
		m_overlappingCount--;

		if (m_overlappingCount == 0) node_sprite.Offset = Vector2.Zero;
	}

}
