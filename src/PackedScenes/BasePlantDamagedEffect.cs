using Godot;

public class BasePlantDamagedEffect : Node2D
{

	[Export] public PackedScene AudioPackedScene { get; private set; }
	[Export] public PackedScene ParticlesPackedScene { get; private set; }

	public override void _Ready ()
	{
		base._Ready();

		if (AudioPackedScene != null)
		{
			OneShotAudio audio = (OneShotAudio)AudioPackedScene.Instance();

			if (GetParentOrNull<BasePlant>() != null)
			{
				GetParent<BasePlant>().AddChild(audio);
			}
			else
			{
				GetTree().Root.AddChild(audio);
			}
		}

		if (ParticlesPackedScene != null)
		{
			OneShotParticles particles = (OneShotParticles)ParticlesPackedScene.Instance();

			if (GetParentOrNull<BasePlant>() != null)
			{
				GetParent<BasePlant>().AddChild(particles);
				particles.Position = Vector2.Zero;
			}
			else
			{
				particles.GlobalPosition = GlobalPosition;
				GetTree().Root.AddChild(particles);
			}
		}
	}

	public override void _Process (float delta)
	{
		base._Process(delta);

		QueueFree();
	}

}
