using Godot;

public class Bone : Area2D
{

	#region Signals

	[Signal] public delegate void OnCollectedSignal (Bone bone);

	#endregion // Signals



	#region Nodes

	public AnimationPlayer node_animationPlayer { get; private set; }

	#endregion // Nodes



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		node_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		node_animationPlayer.Play("Hover");
	}

	#endregion // Godot methods



	#region Area2D callback methods

	private void OnAreaEntered (Area2D area)
	{
		if (IsQueuedForDeletion()) return;

		EmitSignal(nameof(OnCollectedSignal), this);
		QueueFree();
	}

	#endregion // Area2D callback methods

}
