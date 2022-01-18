using Godot;

[Tool]
public class NegativeParallaxHelper : Node2D
{

	public void UpdateWithoutRect ()
	{
		Camera2D node_camera = GetParent().GetNode<CanvasLayer>("MainLayer").GetNode("Bonez").GetNode<Camera2D>("Camera2D");

		for (int i = GetChildCount() - 1; i >= 0; i--)
		{
			ParallaxBackground parallaxBackground = GetChild<ParallaxBackground>(i);
			parallaxBackground.Transform = new Transform2D(0f, new Vector2((float)node_camera.LimitRight / 2, 0f));
			parallaxBackground.Offset = parallaxBackground.Transform.origin;
			parallaxBackground.ScrollBaseOffset = new Vector2()
			{
				x = -((parallaxBackground.Transform.origin.x - (GetViewportRect().Size.x / 2)) * ((float)(GetChildCount() - i) / GetChildCount())),
				y = 0f,
			};

			ParallaxLayer left = parallaxBackground.GetNode<ParallaxLayer>("Left");
			ParallaxLayer center = parallaxBackground.GetNode<ParallaxLayer>("Center");
			ParallaxLayer right = parallaxBackground.GetNode<ParallaxLayer>("Right");

			left.MotionOffset = new Vector2(Mathf.Abs(parallaxBackground.ScrollBaseOffset.x), 0f);
			center.MotionOffset = Vector2.Zero;
			right.MotionOffset = new Vector2(parallaxBackground.ScrollBaseOffset.x, 0f);
		}
	}

	public void UpdateWithRect (ReferenceRect rect)
	{
		UpdateWithoutRect();
	}

	private void _on_NegativeLayers_visibility_changed ()
	{
		if (Visible)
		{
			UpdateWithoutRect();
		}
	}

}
