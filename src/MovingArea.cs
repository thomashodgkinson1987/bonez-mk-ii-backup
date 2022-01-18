using Godot;
using Godot.Collections;

public class MovingArea : Area2D
{

	[Export] public Vector2 Velocity { get; set; } = Vector2.Zero;

	private Array<Area2D> m_areas = new Array<Area2D>();
	private Array<PhysicsBody2D> m_bodies = new Array<PhysicsBody2D>();

	public override void _PhysicsProcess (float delta)
	{
		base._PhysicsProcess(delta);

		for (int i = 0; i < m_areas.Count; i++)
			OnAreaContinue(m_areas[i]);

		for (int i = 0; i < m_bodies.Count; i++)
			OnBodyContinue(m_bodies[i]);

		Translate(Velocity * delta);
	}

	private void OnAreaEntered (Area2D area)
	{
		GD.Print($"{Name}: {nameof(OnAreaEntered)} -> {area.Name} (IsQueuedFree={area.IsQueuedForDeletion()})");
		m_areas.Add(area);
	}
	private void OnAreaContinue (Area2D area)
	{
		GD.Print($"{Name}: {nameof(OnAreaContinue)} -> {area.Name} (IsQueuedFree={area.IsQueuedForDeletion()})");
	}
	private void OnAreaExited (Area2D area)
	{
		GD.Print($"{Name}: {nameof(OnAreaExited)} -> {area.Name} (IsQueuedFree={area.IsQueuedForDeletion()})");
		m_areas.Remove(area);
	}

	private void OnBodyEntered (PhysicsBody2D body)
	{
		GD.Print($"{Name}: {nameof(OnBodyEntered)} -> {body.Name} (IsQueuedFree={body.IsQueuedForDeletion()})");
		m_bodies.Add(body);
	}
	private void OnBodyContinue (PhysicsBody2D body)
	{
		GD.Print($"{Name}: {nameof(OnBodyContinue)} -> {body.Name} (IsQueuedFree={body.IsQueuedForDeletion()})");
	}
	private void OnBodyExited (PhysicsBody2D body)
	{
		GD.Print($"{Name}: {nameof(OnBodyExited)} -> {body.Name} (IsQueuedFree={body.IsQueuedForDeletion()})");
		m_bodies.Remove(body);
	}

}
