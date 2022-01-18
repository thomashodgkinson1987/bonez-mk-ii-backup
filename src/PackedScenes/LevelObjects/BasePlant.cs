using Godot;
using Godot.Collections;

public class BasePlant : Area2D
{

	#region Nodes

	public Sprite node_sprite { get; private set; }
	public AnimationPlayer node_animationPlayer { get; private set; }

	#endregion // Nodes



	#region Exports

	[Export] public Texture DamagedTexture { get; private set; }
	[Export] public PackedScene DamagedPackedScene { get; private set; }

	#endregion // Exports



	#region Fields

	private bool m_hasBeenDamaged = false;
	private readonly RandomNumberGenerator m_rng = new RandomNumberGenerator();

	#endregion // Fields



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		node_sprite = GetNode<Sprite>("Sprite");
		node_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

		m_rng.Randomize();
	}

	#endregion // Godot methods



	#region Area2D callback methods

	private void OnAreaEntered (Area2D area)
	{
		if (area is Projectile projectile)
		{
			if (!m_hasBeenDamaged)
			{
				m_hasBeenDamaged = true;
				node_sprite.Texture = DamagedTexture;
				BasePlantDamagedEffect damagedEffect = (BasePlantDamagedEffect)DamagedPackedScene.Instance();
				damagedEffect.GlobalPosition = GlobalPosition;
				AddChild(damagedEffect);

				//Array nodesInGroup = GetTree().GetNodesInGroup("projectiles_holder");
				//if (nodesInGroup.Count > 0)
				//{
				//	Node2D node = (Node2D)nodesInGroup[0];
				//	node.AddChild(damagedEffect);
				//}
				//else
				//{
				//	GetTree().Root.AddChild(damagedEffect);
				//}
			}

			string animationName = projectile.Direction.x < 0 ? "SwayLeft" : "SwayRight";
			node_animationPlayer.Play(animationName);
		}
		else if (area.Name == "HurtArea")
		{
			Actor actor = area.GetParent<Actor>();

			if (actor.Velocity.x < 0)
				node_animationPlayer.Play("SwayLeft");
			else if (actor.Velocity.x > 0)
				node_animationPlayer.Play("SwayRight");
			else
			{
				string animationName = m_rng.RandiRange(0, 1) == 0 ? "SwayLeft" : "SwayRight";
				node_animationPlayer.Play(animationName);
			}

			if (actor is Shtomp)
			{
				if (!m_hasBeenDamaged)
				{
					m_hasBeenDamaged = true;
					node_sprite.Texture = DamagedTexture;
					BasePlantDamagedEffect damagedEffect = (BasePlantDamagedEffect)DamagedPackedScene.Instance();
					damagedEffect.GlobalPosition = GlobalPosition;
					AddChild(damagedEffect);

					//Array nodesInGroup = GetTree().GetNodesInGroup("projectiles_holder");
					//if (nodesInGroup.Count > 0)
					//{
					//	Node2D node = (Node2D)nodesInGroup[0];
					//	node.AddChild(damagedEffect);
					//}
					//else
					//{
					//	GetTree().Root.AddChild(damagedEffect);
					//}
				}
			}
		}
	}

	#endregion // Area2D callback methods

}
