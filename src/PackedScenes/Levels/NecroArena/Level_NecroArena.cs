using Godot;
using Godot.Collections;

public class Level_NecroArena : Level
{

	#region Nodes

	public LevelHUD_NecroArena node_hud { get; private set; }

	public Necro node_necro { get; private set; }

	public AnimationPlayer node_animationPlayer { get; private set; }

	#endregion // Nodes



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		node_hud = GetNode<LevelHUD_NecroArena>("HUD");
		node_necro = GetNode<Necro>("MainLayer/Actors/Necros/Necro");
		node_animationPlayer = GetNode<AnimationPlayer>("NecroAnimationPlayer");

		node_necro.Connect(nameof(Actor.OnHealthChangedSignal), this, nameof(OnNecroHealthChanged));
		node_hud.NecroHealth = node_necro.Health;

		node_necro.Target = node_bonez;

		LevelSaver.Instance.LoadLevelData(this);

		node_animationPlayer.Play("ANIM_0001");
	}

	#endregion // Godot methods



	#region Public methods

	private void PlayRandomAnimation (string anim_name)
	{
		Array<string> animationNames = new Array<string>();

		animationNames.Add("ANIM_0001");
		animationNames.Add("ANIM_0002");
		animationNames.Add("ANIM_0003");
		animationNames.Add("ANIM_0004");

		RandomNumberGenerator rng = new RandomNumberGenerator();
		rng.Randomize();

		string animationName = anim_name;

		while (animationName == anim_name)
		{
			animationName = animationNames[rng.RandiRange(1, 3)];
		}

		node_animationPlayer.Play(animationName);
	}

	#endregion // Public methods



	#region Callbacks

	private void OnNecroHealthChanged (Necro necro, int oldHealth, int newHealth)
	{
		node_hud.NecroHealth = newHealth;

		float per = (float)necro.Health / (float)necro.MaxHealth;

		if (oldHealth > 0 && newHealth <= 0)
		{
			node_animationPlayer.Stop();
			Engine.TimeScale *= 0.5f;
			node_bonez.Position = new Vector2(208f, -680f);
			node_bonez.Visible = false;
			node_bonez.IsPollInput = false;
			node_bonez.IsMove = false;
			node_camera.SmoothingSpeed = 0.1f;
			LoadCredits();
		}
	}

	private async void LoadCredits ()
	{
		node_fader.FadeTime = 3f;
		node_fader.FadeToOpaque();
		await ToSignal(node_fader, nameof(Fader.OnFadedToOpaqueSignal));
		Engine.TimeScale = 1f;
		GetTree().ChangeScene("res://assets/scenes/credits_scene.tscn");
	}

	protected override void OnPlayerHealthChanged (Bonez player, int oldHealth, int newHealth)
	{
		base.OnPlayerHealthChanged(player, oldHealth, newHealth);

		node_hud.PlayerHealth = (float)newHealth / (float)player.MaxHealth;
	}

	protected override void OnPlayerFireRateTimerChanged (Bonez player, float fireRate, float fireRateTimer)
	{
		base.OnPlayerFireRateTimerChanged(player, fireRate, fireRateTimer);

		node_hud.PlayerFireRateTimer = fireRateTimer / fireRate;
	}

	#endregion // Callbacks

}
