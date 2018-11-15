using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Microsoft.Xna.Framework;
using MultiThreadComponentGame.Components;
using Math = System.Math;
using MathF = Common.MathF;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MultiThreadComponentGame.Systems
{
    public class MoveSystem
    {
        private readonly struct Bounds
        {
            public readonly int MinX, MaxX, MinY, MaxY;

            public Bounds(int minX, int maxX, int minY, int maxY)
            {
                this.MinX = minX;
                this.MaxX = maxX;
                this.MinY = minY;
                this.MaxY = maxY;
            }
        }
        private readonly Bounds bounds;
        private readonly int length;
        private readonly int sliceLength;
        private readonly PositionComponent[] positionComponents;
        private readonly VelocityComponent[] velocityComponents;

        public MoveSystem(
            GraphicsDeviceManager graphics,
            int length,
            PositionComponent[] positionComponents,
            VelocityComponent[] velocityComponents)
        {
            const int minX = 0;
            var maxX = graphics.PreferredBackBufferWidth;
            const int minY = 0;
            var maxY = graphics.PreferredBackBufferHeight;
            this.bounds = new Bounds(minX, maxX, minY, maxY);
            this.length = length;
            this.sliceLength = this.length / Environment.ProcessorCount + 1;
            this.positionComponents = positionComponents;
            this.velocityComponents = velocityComponents;
        }

        public Task Update(float deltaTime, Task inputDeps)
        {
            var jobs = ScheduleJobs(deltaTime, inputDeps);
            return Task.WhenAll(jobs);
        }

        private IEnumerable<Task> ScheduleJobs(float deltaTime, Task inputDeps)
        {
            Memory<PositionComponent> positions = this.positionComponents;
            Memory<VelocityComponent> velocities = this.velocityComponents;

            for(int i = 0; i < Environment.ProcessorCount; i++)
            {
                var startIndex = i * this.sliceLength;
                var sliceLength = Math.Min(this.sliceLength, this.length - startIndex);

                var positionsSlice = positions.Slice(startIndex, sliceLength);
                var velocitiesSlice = velocities.Slice(startIndex, sliceLength);
                yield return inputDeps.ContinueWith((x) => RunJob(
                                                        deltaTime, sliceLength, this.bounds, positionsSlice, velocitiesSlice));
            }
        }
        
        private static void RunJob(
            float deltaTime,
            int sliceLength,
            Bounds bounds,
            Memory<PositionComponent> positionsSlice,
            Memory<VelocityComponent> velocitiesSlice)
        {
            var positions = positionsSlice.Span;
            var velocities = velocitiesSlice.Span;
            for(int i = 0; i < sliceLength; i++)
            {
                var position = positions[i];

                position.Value += velocities[i].Value * deltaTime;

                var velocity = velocities[i];

                var x = MathF.Select(1, -1, position.Value.X < bounds.MinX || position.Value.X > bounds.MaxX);
                var y = MathF.Select(1, -1, position.Value.Y < bounds.MinY || position.Value.Y > bounds.MaxY);
                var multiply = new Vector2(x, y);

                velocity.Value *= multiply;

                position.Value = new Vector2(position.Value.X.Clamp(bounds.MinX, bounds.MaxX), position.Value.Y.Clamp(bounds.MinY, bounds.MaxY));

                positions[i] = position;
                velocities[i] = velocity;
            }
        }
    }
}
