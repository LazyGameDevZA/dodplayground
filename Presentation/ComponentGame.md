# Component Game

Each component lives in an array specific to it's type. Systems operate on these arrays to apply transformations.

## Components

* [Position](../ComponentGame/Components/PositionComponent.cs)
* [Velocity](../ComponentGame/Components/VelocityComponent.cs)
* [VelocityModifier](../ComponentGame/Components/VelocityModifierComponent.cs)
* [VelocityConstraint](../ComponentGame/Components/VelocityComponent.cs)
* [Size](../ComponentGame/Components/SizeComponent.cs)
* [Sprite](../ComponentGame/Components/SpriteComponent.cs)

## Initialization

* [Single Bootstrap](../ComponentGame/ComponentGame.cs#L38-L98)

## Update Loops

* [MoveSystem](../ComponentGame/Systems/MoveSystem.cs#L32-L55)
* [VelocityModifierSystem](../ComponentGame/Systems/VelocityModifierSystem.cs#L36-L63)

[Issues with design](ComponentGameIssues.md)