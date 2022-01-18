using Godot;

public class LevelExit : Area2D
{

	#region Signals

	[Signal] public delegate void OnExitAreaEnteredSignal ();

	#endregion // Signals



	#region Fields

	private bool m_hasBeenTouched = false;

	#endregion // Fields



	#region Area2D callback methods

	private void OnAreaEnteredExitArea (Area2D area)
	{
		if (m_hasBeenTouched) return;

		m_hasBeenTouched = true;
		EmitSignal(nameof(OnExitAreaEnteredSignal));
	}

	#endregion // Area2D callback methods

}
