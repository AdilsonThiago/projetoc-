using Godot;
using System;

public class shoot : Area
{
    private bool Ativo = true;
    private float Velocidade = 200f;
    private Vector3 PreviaPosi = new Vector3(0, 0, 0);
    private AnimationPlayer Anim;

    public override void _Ready()
    {
        Anim = GetNode<AnimationPlayer>("anim");
    }

    public override void _Process(float delta)
    {
        if (Ativo)
        {
            PreviaPosi = Translation;
            Translation = Translation + new Vector3(0, 0, -Velocidade * delta);
        }
    }

    public void _on_Area_body_entered(RigidBody body)
    {
        if (Ativo && body.IsInGroup("asteroid"))
        {
            Anim.Play("destruir");
            Translation = PreviaPosi + new Vector3(0, 0, 1);
            Ativo = false;
            body.CallDeferred("Damage", 1f);
        }
    }
}
