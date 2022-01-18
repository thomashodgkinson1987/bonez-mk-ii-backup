using Godot;

public class Fader : CanvasLayer
{

	#region Nodes

	public ColorRect node_colorRect { get; private set; }

	#endregion  // Nodes



	#region Signals

	[Signal] public delegate void OnFadingToTransparentSignal (float per);
	[Signal] public delegate void OnFadedToTransparentSignal ();
	[Signal] public delegate void OnFadingToOpaqueSignal (float per);
	[Signal] public delegate void OnFadedToOpaqueSignal ();

	#endregion // Signals



	#region Exports

	[Export] public Color OpaqueColor { get; set; } = new Color(0f, 0f, 0f, 1f);
	[Export] public Color TransparentColor { get; set; } = new Color(0f, 0f, 0f, 0f);

	[Export] public float FadeTime { get; set; } = 0.5f;

	#endregion // Exports



	#region Properties

	public float FadeTimer { get; private set; } = 0f;
	public bool IsFadingToOpaque { get; private set; } = false;
	public bool IsFadingToTransparent { get; private set; } = false;
	public bool IsFading { get; private set; } = false;

	#endregion // Properties



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		node_colorRect = GetNode<ColorRect>("ColorRect");

		FadeTimer = 0f;
	}

	public override void _Process (float delta)
	{
		base._Process(delta);

		if (IsFading)
		{
			FadeTimer += delta;
			FadeTimer = Mathf.Clamp(FadeTimer, 0, FadeTime);

			float per = FadeTimer / FadeTime;

			if (IsFadingToOpaque)
				node_colorRect.SelfModulate = TransparentColor.LinearInterpolate(OpaqueColor, per);
			else if (IsFadingToTransparent)
				node_colorRect.SelfModulate = OpaqueColor.LinearInterpolate(TransparentColor, per);

			if (IsFadingToOpaque)
			{
				EmitSignal(nameof(OnFadingToOpaqueSignal), per);
			}
			else if (IsFadingToTransparent)
			{
				EmitSignal(nameof(OnFadingToTransparentSignal), per);
			}

			if (per == 1)
			{
				if (IsFadingToOpaque) EmitSignal(nameof(OnFadedToOpaqueSignal));
				else if (IsFadingToTransparent) EmitSignal(nameof(OnFadedToTransparentSignal));

				FadeTimer = 0f;

				IsFading = false;
				IsFadingToOpaque = false;
				IsFadingToTransparent = false;
			}
		}
	}

	#endregion // Godot methods



	#region Public methods

	public void FadeToOpaque ()
	{
		FadeTimer = 0f;
		IsFading = true;
		IsFadingToOpaque = true;
		IsFadingToTransparent = false;
		node_colorRect.SelfModulate = TransparentColor;
	}

	public void FadeToTransparent ()
	{
		FadeTimer = 0f;
		IsFading = true;
		IsFadingToOpaque = false;
		IsFadingToTransparent = true;
		node_colorRect.SelfModulate = OpaqueColor;
	}

	#endregion // Public methods

}
