using Godot;

public class BreakableBlock : StaticBody2D
{

	#region Nodes

	public AnimatedSprite node_animatedSprite { get; private set; }

	#endregion // Nodes



	#region Fields

	private bool m_isBreaking = false;

	#endregion // Fields



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		node_animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
	}

	#endregion // Godot methods



	#region Callback methods

	private void OnAreaEnteredTriggerArea (Area2D area)
	{
		if (m_isBreaking) return;

		Break();
	}

	private void OnBreakAnimationFinished ()
	{
		QueueFree();
	}

	#endregion // Callback methods



	#region Public methods

	public void Break ()
	{
		if (m_isBreaking) return;

		m_isBreaking = true;
		node_animatedSprite.Play("break");
	}

	#endregion // Public methods

}
