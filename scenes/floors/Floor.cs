using Godot;

namespace Spire.scenes.floors;

public partial class Floor : Node2D
{
	private Sprite2D _floor;
	
	public override void _Ready()
	{
		_floor = new Sprite2D();
		_floor.Texture = ResourceLoader.Load<Texture2D>("res://assets/floor.png");
		_floor.Modulate = Colors.White;
		_floor.Centered = false;
		AddChild(_floor);
	}
}