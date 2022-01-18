using Godot;

public class MovingBody : KinematicBody2D
{

	[Export] public Vector2 Velocity { get; set; } = Vector2.Zero;

	public override void _PhysicsProcess (float delta)
	{
		base._PhysicsProcess(delta);

		Velocity = MoveAndSlide(Velocity, Vector2.Up);
	}

	private void OnAreaEnteredInteractArea (Area2D area)
	{
		GD.Print($"{Name}: {nameof(OnAreaEnteredInteractArea)} -> {area.Name} (IsQueuedFree={area.IsQueuedForDeletion()})");
	}

	private void OnAreaExitedInteractArea (Area2D area)
	{
		GD.Print($"{Name}: {nameof(OnAreaExitedInteractArea)} -> {area.Name} (IsQueuedFree={area.IsQueuedForDeletion()})");
	}

	private void OnBodyEnteredInteractArea (PhysicsBody2D body)
	{
		GD.Print($"{Name}: {nameof(OnBodyEnteredInteractArea)} -> {body.Name} (IsQueuedFree={body.IsQueuedForDeletion()})");
	}

	private void OnBodyExitedInteractArea (PhysicsBody2D body)
	{
		GD.Print($"{Name}: {nameof(OnBodyExitedInteractArea)} -> {body.Name} (IsQueuedFree={body.IsQueuedForDeletion()})");
	}

}
