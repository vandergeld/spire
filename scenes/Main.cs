using Godot;
using Godot.Collections;

namespace Spire.scenes;

public partial class Main : Node2D
{
    private Node2D _previewFloor;
    private PackedScene _floorScene;

    private const int TILE_WIDTH = 128;
    private const int TILE_HEIGHT = 64;

    private Dictionary<Vector2, Node2D> _tiles = new Dictionary<Vector2, Node2D>();

    public override void _Ready()
    {
        _floorScene = ResourceLoader.Load<PackedScene>("res://scenes/floors/Floor.tscn");

        _previewFloor = _floorScene.Instantiate<Node2D>();
        _previewFloor.Modulate = new Color(1, 1, 1, 0.5f);
        AddChild(_previewFloor);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("left_click") && !@event.IsEcho())
        {
            PlaceFloorAtMousePosition();
            GetViewport().SetInputAsHandled();
        }
    }

    public override void _Process(double delta)
    {
        var gridPosition = ScreenToIso(GetGlobalMousePosition());
        _previewFloor.ZIndex = (int)(gridPosition.X + gridPosition.Y);
        _previewFloor.GlobalPosition = IsoToScreen(gridPosition);
    }

    private Vector2 ScreenToIso(Vector2 screenPos)
    {
        float x = (screenPos.X / (TILE_WIDTH / 2.0f) + screenPos.Y / (TILE_HEIGHT / 2.0f)) / 2.0f;
        float y = (screenPos.Y / (TILE_HEIGHT / 2.0f) - screenPos.X / (TILE_WIDTH / 2.0f)) / 2.0f;

        return new Vector2(Mathf.Floor(x), Mathf.Floor(y));
    }

    private Vector2 IsoToScreen(Vector2 isoPos)
    {
        float x = (isoPos.X - isoPos.Y) * (TILE_WIDTH / 2.0f);
        float y = (isoPos.X + isoPos.Y) * (TILE_HEIGHT / 2.0f);

        return new Vector2(x, y);
    }

    private void PlaceFloorAtMousePosition()
    {
        var gridPosition = ScreenToIso(GetGlobalMousePosition());

        if (_tiles.ContainsKey(gridPosition))
            return;

        var floor = _floorScene.Instantiate<Node2D>();
        var screenPos = IsoToScreen(gridPosition);

        floor.GlobalPosition = screenPos;
        floor.ZIndex = (int)(gridPosition.X + gridPosition.Y);
        AddChild(floor);

        _tiles[gridPosition] = floor;
    }
}