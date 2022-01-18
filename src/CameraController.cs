using Godot;

[Tool]
public class CameraController : Camera2D
{

	public void UpdateLimitsFromRect (ReferenceRect referenceRect)
	{
		LimitLeft = (int)referenceRect.RectPosition.x;
		LimitRight = LimitLeft + (int)referenceRect.RectSize.x;
		LimitTop = (int)referenceRect.RectPosition.y;
		LimitBottom = LimitTop + (int)referenceRect.RectSize.y;
	}

}
