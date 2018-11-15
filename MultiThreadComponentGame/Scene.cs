using Common;
using MultiThreadComponentGame.Components;

namespace MultiThreadComponentGame
{
    internal static class Scene
    {
        public static readonly int EntityCount;
        public static readonly int VelocityModifierCount;
        public static readonly PositionComponent[] PositionComponents;
        public static readonly VelocityConstraintComponent[] VelocityConstraintComponents;
        public static readonly VelocityComponent[] VelocityComponents;
        public static readonly SpriteComponent[] SpriteComponents;
        public static readonly SizeComponent[] SizeComponents;
        public static readonly VelocityModifierComponent[] VelocityModifierComponents;

        static Scene()
        {
            EntityCount = Constants.DotCount + Constants.BubbleCount;
            VelocityModifierCount = Constants.BubbleCount;
            PositionComponents = new PositionComponent[EntityCount];
            VelocityConstraintComponents = new VelocityConstraintComponent[EntityCount];
            VelocityComponents = new VelocityComponent[EntityCount];
            SpriteComponents = new SpriteComponent[EntityCount];
            SizeComponents = new SizeComponent[VelocityModifierCount];
            VelocityModifierComponents = new VelocityModifierComponent[VelocityModifierCount];
        }
    }
}
