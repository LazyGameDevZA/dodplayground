using ComponentGame.Components;
using static Common.Constants;

namespace ComponentGame
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
            EntityCount = DotCount + BubbleCount;
            VelocityModifierCount = BubbleCount;
            PositionComponents = new PositionComponent[EntityCount];
            VelocityConstraintComponents = new VelocityConstraintComponent[EntityCount];
            VelocityComponents = new VelocityComponent[EntityCount];
            SpriteComponents = new SpriteComponent[EntityCount];
            SizeComponents = new SizeComponent[VelocityModifierCount];
            VelocityModifierComponents = new VelocityModifierComponent[VelocityModifierCount];
        }
    }
}
