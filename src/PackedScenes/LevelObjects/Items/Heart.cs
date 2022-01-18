using Godot;

public class Heart : Area2D
{

	#region Area2D callback methods

	private void OnAreaEntered (Area2D area)
	{
		if (IsQueuedForDeletion()) return;

		QueueFree();
	}

	#endregion // Area2D callback methods

}
