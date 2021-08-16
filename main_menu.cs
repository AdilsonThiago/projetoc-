using Godot;
using System;

public class main_menu : Node2D
{
    PackedScene Fase;
    public override void _Ready()
    {
        Fase = GD.Load<PackedScene>("res://fase.tscn");
    }

    public void _on_Button_pressed()
    {
        Spatial fase = (Spatial)Fase.Instance();
        GetParent().AddChild(fase);
        fase.GetNode<CanvasLayer>("hud").CallDeferred("Mudar_Nome", GetNode<TextEdit>("TextEdit").Text);
        QueueFree();
    }
}
