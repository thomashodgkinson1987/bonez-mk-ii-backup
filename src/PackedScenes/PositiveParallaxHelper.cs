using Godot;

[Tool]
public class PositiveParallaxHelper : Node2D
{

	public void UpdateWithoutRect ()
	{
		Camera2D node_camera = GetParent().GetNode<CanvasLayer>("MainLayer").GetNode("Bonez").GetNode<Camera2D>("Camera2D");

		for (int i = 0; i < GetChildCount(); i++)
		{
			ParallaxBackground parallaxBackground = GetChild<ParallaxBackground>(i);
			parallaxBackground.Transform = new Transform2D(0f, new Vector2((float)node_camera.LimitRight / 2, 0f));
			parallaxBackground.Offset = parallaxBackground.Transform.origin;
			parallaxBackground.ScrollBaseOffset = new Vector2()
			{
				x = (parallaxBackground.Transform.origin.x - (GetViewportRect().Size.x / 2)) * ((float)(i + 1) / GetChildCount()),
				y = 0f,
			};

			ParallaxLayer left = parallaxBackground.GetNode<ParallaxLayer>("Left");
			ParallaxLayer center = parallaxBackground.GetNode<ParallaxLayer>("Center");
			ParallaxLayer right = parallaxBackground.GetNode<ParallaxLayer>("Right");

			left.MotionOffset = new Vector2(-parallaxBackground.ScrollBaseOffset.x, 0f);
			center.MotionOffset = Vector2.Zero;
			right.MotionOffset = new Vector2(parallaxBackground.ScrollBaseOffset.x, 0f);
		}
	}

	public void UpdateWithRect (ReferenceRect rect)
	{
		UpdateWithoutRect();
	}

	private void _on_PositiveLayers_visibility_changed ()
	{
		if (Visible)
		{
			UpdateWithoutRect();
		}
	}

}
