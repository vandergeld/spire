using Godot;

namespace Spire.scenes;

public partial class Main : Node2D
{
    private Sprite2D _sprite;
    private PackedScene _floorScene;

    public override void _Ready()
    {
        _floorScene = ResourceLoader.Load<PackedScene>("res://scenes/floors/Floor.tscn");

        _sprite = new Sprite2D();
        _sprite.Texture = ResourceLoader.Load<Texture2D>("res://assets/floor.png");
        _sprite.Centered = false;
        AddChild(_sprite);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("left_click"))
        {
            PlaceFloorAtMousePosition();
        }
    }

    public override void _Process(double delta)
    {
        var gridPosition = GetMouseGridCellPosition();
        _sprite.GlobalPosition = gridPosition * 64;
    }

    private Vector2 GetMouseGridCellPosition()
    {
        var mousePosition = GetGlobalMousePosition();
        var gridPosition = mousePosition / 64;
        gridPosition = gridPosition.Floor();
        return gridPosition;
    }
    
    private void PlaceFloorAtMousePosition()
    {
        var floor = _floorScene.Instantiate<Node2D>();
        var gridPosition = GetMouseGridCellPosition();
        floor.GlobalPosition = gridPosition * 64;
        AddChild(floor);
    }
}