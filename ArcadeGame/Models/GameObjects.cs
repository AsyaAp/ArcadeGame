using ArcadeGame.Patterns;

namespace ArcadeGame.Models;

public abstract class GameObject
{
    public string Id { get; }
    public Vector2 Position { get; internal set; }
    public Vector2 Velocity { get; set; }
    public Vector2 Size { get; }
    public bool IsActive { get; protected set; } = true;

    public abstract string Kind { get; }

    protected GameObject(string id, Vector2 position, Vector2 size, Vector2 velocity)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Идентификатор объекта не задан.", nameof(id));

        Id = id;
        Position = position;
        Size = size;
        Velocity = velocity;
    }

    public virtual void UpdateBehavior(GameWorld world, double deltaTime) { }

    public void UpdatePhysics(IPhysicsModel physics, GameWorld world, double deltaTime)
    {
        if (!IsActive) return;
        physics.Update(this, world, deltaTime);
    }

    public void Render(IRenderer renderer, GameWorld world)
    {
        renderer.Draw(this, world);
    }
}

public sealed class Player : GameObject
{
    public Player(string id, Vector2 position, Vector2 size)
        : base(id, position, size, Vector2.Zero) { }

    public override string Kind => "Player";
}

public sealed class Platform : GameObject
{
    public Platform(string id, Vector2 position, Vector2 size)
        : base(id, position, size, Vector2.Zero) { }

    public override string Kind => "Platform";
}

public sealed class Enemy : GameObject
{
    public double Speed { get; set; }
    public IEnemyBehavior Behavior { get; private set; }

    public Enemy(string id, Vector2 position, Vector2 size, double speed, IEnemyBehavior behavior)
        : base(id, position, size, Vector2.Zero)
    {
        if (speed <= 0)
            throw new ArgumentOutOfRangeException(nameof(speed), "Скорость должна быть больше 0.");

        Speed = speed;
        Behavior = behavior ?? throw new ArgumentNullException(nameof(behavior));
    }

    public override string Kind => "Enemy";

    public void SetBehavior(IEnemyBehavior behavior)
    {
        Behavior = behavior ?? throw new ArgumentNullException(nameof(behavior));
    }

    public override void UpdateBehavior(GameWorld world, double deltaTime)
    {
        Behavior.Update(this, world, deltaTime);
    }
}