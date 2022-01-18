using Godot;

public class Water : Area2D
{

	#region Nodes

	public AnimatedSprite node_animatedSprite { get; private set; }

	#endregion // Nodes



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		node_animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		node_animatedSprite.Frame = 0;
	}

	#endregion // Godot methods

}
