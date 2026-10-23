using ArcadeGame.Patterns;

namespace ArcadeGame.Models;

public sealed class GameWorld
{
    private readonly List<GameObject> _objects = new();

    public Vector2 Bounds { get; }

    public GameWorld(Vector2 bounds)
    {
        if (bounds.X <= 0 || bounds.Y <= 0)
            throw new ArgumentOutOfRangeException(nameof(bounds), "Границы мира должны быть положительными.");
        Bounds = bounds;
    }

    public IReadOnlyList<GameObject> Objects => _objects;
    public Player? Player => _objects.OfType<Player>().FirstOrDefault();

    public void Add(GameObject gameObject)
    {
        ArgumentNullException.ThrowIfNull(gameObject);
        if (_objects.Any(o => o.Id == gameObject.Id))
            throw new InvalidOperationException($"Объект с идентификатором «{gameObject.Id}» уже существует.");
        _objects.Add(gameObject);
    }

    public Enemy? GetEnemy(string id)
        => _objects.OfType<Enemy>().FirstOrDefault(e => e.Id == id);

    public void UpdateBehaviors(double deltaTime)
    {
        foreach (var o in _objects.ToArray())
            o.UpdateBehavior(this, deltaTime);
    }

    public void UpdatePhysics(IPhysicsModel physics, double deltaTime)
    {
        foreach (var o in _objects.ToArray())
            o.UpdatePhysics(physics, this, deltaTime);
    }
}