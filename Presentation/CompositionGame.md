# Composition Game

Move everything into components and have components apply logic. [GameObject](../CompositionGame/GameObject.cs) becomes only a container of [Components](../CompositionGame/Component.cs).

## Components

* [Position](../CompositionGame/Components/PositionComponent.cs)
* [Velocity](../CompositionGame/Components/VelocityComponent.cs)
* [Size](../CompositionGame/Components/SizeComponent.cs)
* [VelocityModifier](../CompositionGame/Components/VelocityModifierComponent.cs)
* [ModifyVelocity](../CompositionGame/Components/ModifyVelocityComponent.cs)
* [Size](../CompositionGame/Components/SizeComponent.cs)
* [Sprite](../CompositionGame/Components/SpriteComponent.cs)
* [WorldBounds](../CompositionGame/Components/WorldBoundsComponent.cs)

## Initialization

* [GameObject](../CompositionGame/GameObject.cs#L37-L43)
* [Position](../CompositionGame/Components/PositionComponent.cs#L18-L25)
* [Velocity](../CompositionGame/Components/VelocityComponent.cs#L29-L33)
* [ModifyVelocity](../CompositionGame/Components/ModifyVelocityComponent.cs#L17-L34)
* [WorldBounds](../CompositionGame/Components/WorldBoundsComponent.cs#L16-L22)

## Update Loops

* [GameObject](../CompositionGame/GameObject.cs#L45-L51)
* [Velocity](../CompositionGame/Components/VelocityComponent.cs#L35-L46)
* [ModifyVelocity](../CompositionGame/Components/ModifyVelocityComponent.cs#L36-L51)

[Issues with design](CompositionGameIssues.md)