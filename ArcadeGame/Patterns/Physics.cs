using ArcadeGame.Models;

namespace ArcadeGame.Patterns;

public interface IPhysicsModel
{
    void Update(GameObject gameObject, GameWorld world, double deltaTime);
}

public sealed class SimplePhysicsModel : IPhysicsModel
{
    public void Update(GameObject gameObject, GameWorld world, double deltaTime)
    {
        if (deltaTime <= 0) return;

        var newPosition = gameObject.Position + gameObject.Velocity * deltaTime;

        var maxX = Math.Max(0, world.Bounds.X - gameObject.Size.X);
        var maxY = Math.Max(0, world.Bounds.Y - gameObject.Size.Y);

        gameObject.Position = new Vector2(
            Math.Clamp(newPosition.X, 0, maxX),
            Math.Clamp(newPosition.Y, 0, maxY));
    }
}