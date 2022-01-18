using Godot;

[Tool]
public class MapBoundsHelper : ReferenceRect
{

	#region Nodes

	public StaticBody2D node_leftStaticBody { get; private set; }
	public StaticBody2D node_rightStaticBody { get; private set; }
	public StaticBody2D node_topStaticBody { get; private set; }
	public StaticBody2D node_bottomStaticBody { get; private set; }

	public CollisionShape2D node_leftCollisionShape { get; private set; }
	public CollisionShape2D node_rightCollisionShape { get; private set; }
	public CollisionShape2D node_topCollisionShape { get; private set; }
	public CollisionShape2D node_bottomCollisionShape { get; private set; }

	public RectangleShape2D node_leftRectangleShape { get; private set; }
	public RectangleShape2D node_rightRectangleShape { get; private set; }
	public RectangleShape2D node_topRectangleShape { get; private set; }
	public RectangleShape2D node_bottomRectangleShape { get; private set; }

	#endregion // Nodes



	#region Signals

	[Signal] public delegate void OnMapBoundsResizedSignal (ReferenceRect referenceRect);

	#endregion // Signals



	#region Godot methods

	public override void _Ready ()
	{
		base._Ready();

		node_leftStaticBody = GetNode<StaticBody2D>("LeftStaticBody2D");
		node_rightStaticBody = GetNode<StaticBody2D>("RightStaticBody2D");
		node_topStaticBody = GetNode<StaticBody2D>("TopStaticBody2D");
		node_bottomStaticBody = GetNode<StaticBody2D>("BottomStaticBody2D");

		node_leftCollisionShape = node_leftStaticBody.GetNode<CollisionShape2D>("CollisionShape2D");
		node_rightCollisionShape = node_rightStaticBody.GetNode<CollisionShape2D>("CollisionShape2D");
		node_topCollisionShape = node_topStaticBody.GetNode<CollisionShape2D>("CollisionShape2D");
		node_bottomCollisionShape = node_bottomStaticBody.GetNode<CollisionShape2D>("CollisionShape2D");

		node_leftRectangleShape = (RectangleShape2D)node_leftCollisionShape.Shape;
		node_rightRectangleShape = (RectangleShape2D)node_rightCollisionShape.Shape;
		node_topRectangleShape = (RectangleShape2D)node_topCollisionShape.Shape;
		node_bottomRectangleShape = (RectangleShape2D)node_bottomCollisionShape.Shape;
	}

	#endregion // Godot methods



	#region Callback methods

	private void OnMapBoundsResized ()
	{
		node_leftStaticBody = GetNode<StaticBody2D>("LeftStaticBody2D");
		node_rightStaticBody = GetNode<StaticBody2D>("RightStaticBody2D");
		node_topStaticBody = GetNode<StaticBody2D>("TopStaticBody2D");
		node_bottomStaticBody = GetNode<StaticBody2D>("BottomStaticBody2D");

		node_leftCollisionShape = node_leftStaticBody.GetNode<CollisionShape2D>("CollisionShape2D");
		node_rightCollisionShape = node_rightStaticBody.GetNode<CollisionShape2D>("CollisionShape2D");
		node_topCollisionShape = node_topStaticBody.GetNode<CollisionShape2D>("CollisionShape2D");
		node_bottomCollisionShape = node_bottomStaticBody.GetNode<CollisionShape2D>("CollisionShape2D");

		node_leftRectangleShape = (RectangleShape2D)node_leftCollisionShape.Shape;
		node_rightRectangleShape = (RectangleShape2D)node_rightCollisionShape.Shape;
		node_topRectangleShape = (RectangleShape2D)node_topCollisionShape.Shape;
		node_bottomRectangleShape = (RectangleShape2D)node_bottomCollisionShape.Shape;

		node_leftStaticBody.Position = new Vector2()
		{
			x = -16f,
			y = RectSize.y / 2
		};
		node_leftRectangleShape.Extents = new Vector2()
		{
			x = RectSize.y / 2,
			y = node_leftRectangleShape.Extents.y
		};

		node_rightStaticBody.Position = new Vector2()
		{
			x = RectSize.x + 16f,
			y = RectSize.y / 2
		};
		node_rightRectangleShape.Extents = new Vector2()
		{
			x = RectSize.y / 2,
			y = node_rightRectangleShape.Extents.y
		};

		node_topStaticBody.Position = new Vector2()
		{
			x = RectSize.x / 2,
			y = -16f
		};
		node_topRectangleShape.Extents = new Vector2()
		{
			x = RectSize.x / 2,
			y = node_topRectangleShape.Extents.y
		};

		node_bottomStaticBody.Position = new Vector2()
		{
			x = RectSize.x / 2,
			y = RectSize.y + 16f
		};
		node_bottomRectangleShape.Extents = new Vector2()
		{
			x = RectSize.x / 2,
			y = node_bottomRectangleShape.Extents.y
		};

		EmitSignal(nameof(OnMapBoundsResizedSignal), this);
	}

	#endregion // Callback methods

}
