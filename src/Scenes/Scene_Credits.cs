using Godot;

public class Scene_Credits : CanvasLayer
{

	public void LoadMainMenu ()
	{
		GetTree().ChangeScene("res://assets/scenes/main_menu_scene.tscn");
	}

}
