using ArcadeGame.Models;

namespace ArcadeGame.Patterns;

public interface IEnemyBehavior
{
    string Name { get; }
    void Update(Enemy enemy, GameWorld world, double deltaTime);
}

public sealed class ChaseBehavior : IEnemyBehavior
{
    public string Name => "Chase";

    public void Update(Enemy enemy, GameWorld world, double deltaTime)
    {
        var player = world.Player;
        if (player is null) { enemy.Velocity = Vector2.Zero; return; }

        var delta = player.Position - enemy.Position;
        enemy.Velocity = delta.Length > 1e-6
            ? delta.Normalized() * enemy.Speed
            : Vector2.Zero;
    }
}

public sealed class PatrolBehavior : IEnemyBehavior
{
    private Vector2 _direction;

    public PatrolBehavior() : this(new Vector2(1, 0)) { }

    public PatrolBehavior(Vector2 initialDirection)
    {
        _direction = initialDirection.Length > 1e-6
            ? initialDirection.Normalized()
            : new Vector2(1, 0);
    }

    public string Name => "Patrol";

    public void Update(Enemy enemy, GameWorld world, double deltaTime)
    {
        var maxX = Math.Max(0, world.Bounds.X - enemy.Size.X);
        var maxY = Math.Max(0, world.Bounds.Y - enemy.Size.Y);

        if (enemy.Position.X <= 0.01 && _direction.X < 0)
            _direction = new Vector2(Math.Abs(_direction.X), _direction.Y);
        if (enemy.Position.X >= maxX - 0.01 && _direction.X > 0)
            _direction = new Vector2(-Math.Abs(_direction.X), _direction.Y);
        if (enemy.Position.Y <= 0.01 && _direction.Y < 0)
            _direction = new Vector2(_direction.X, Math.Abs(_direction.Y));
        if (enemy.Position.Y >= maxY - 0.01 && _direction.Y > 0)
            _direction = new Vector2(_direction.X, -Math.Abs(_direction.Y));

        enemy.Velocity = _direction.Normalized() * enemy.Speed;
    }
}

public sealed class IdleBehavior : IEnemyBehavior
{
    public string Name => "Idle";

    public void Update(Enemy enemy, GameWorld world, double deltaTime)
    {
        enemy.Velocity = Vector2.Zero;
    }
}