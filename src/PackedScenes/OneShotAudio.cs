using Godot;

public class OneShotAudio : AudioStreamPlayer
{

	#region Signals

	[Signal] public delegate void OnDestroySignal (OneShotAudio oneShotAudio);

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

		OnAudioFinished();
	}

	#endregion // Public methods



	#region AudioStreamPlayer callbacks

	private void OnAudioFinished ()
	{
		if (IsQueuedForDeletion()) return;

		EmitSignal(nameof(OnDestroySignal), this);

		QueueFree();
	}

	#endregion // AudioStreamPlayer callbacks

}
