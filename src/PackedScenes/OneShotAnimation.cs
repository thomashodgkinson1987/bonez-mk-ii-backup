using Godot;

public class OneShotAnimation : AnimatedSprite
{

	#region Signals

	[Signal] public delegate void OnDestroySignal (OneShotAnimation oneShotAnimation);

	#endregion // Signals



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		Play();
	}

	#endregion // Godot methods



	#region Public methods

	public void Destroy ()
	{
		if (IsQueuedForDeletion()) return;

		OnAnimationFinished();
	}

	#endregion // Public methods



	#region AnimatedSprite callbacks

	private void OnAnimationFinished ()
	{
		if (IsQueuedForDeletion()) return;

		EmitSignal(nameof(OnDestroySignal), this);

		QueueFree();
	}

	#endregion // AnimatedSprite callbacks

}
