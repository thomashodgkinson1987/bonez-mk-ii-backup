using Godot;

public class OneShotParticles : CPUParticles2D
{

	#region Signals

	[Signal] public delegate void OnDestroySignal (OneShotParticles oneShotParticles);

	#endregion // Signals



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		Emitting = true;
	}

	public override void _Process (float delta)
	{
		base._Process(delta);

		if (IsQueuedForDeletion()) return;

		if (!Emitting) Destroy();
	}

	#endregion // Godot methods



	#region Public methods

	public void Destroy ()
	{
		if (IsQueuedForDeletion()) return;

		OnParticlesFinished();
	}

	#endregion // Public methods



	#region Callback methods

	private void OnParticlesFinished ()
	{
		if (IsQueuedForDeletion()) return;

		EmitSignal(nameof(OnDestroySignal), this);

		QueueFree();
	}

	#endregion // Callback methods

}
