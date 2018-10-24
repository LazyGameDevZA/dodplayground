# Object Oriented Game

Use a simple object hierarchy. Parent [GameObject](../OOGame/GameObject.cs) with [Dot](../OOGame/Dot.cs) and [Bubble](../OOGame/Bubble.cs) as children.

## Object initialization

* [GameObject](../OOGame/GameObject.cs#L26-L32)

* [Dot](../OOGame/Dot.cs#L28-L46)

* [Bubble](../OOGame/Bubble.cs#L30-L44)

## Update loops for each object

* [GameObject](../OOGame/GameObject.cs#L34-L50) -> basic movement and keeping objects on-screen

* [Dot](../OOGame/Dot.cs#L48-L64) -> check if moving through a bubble and adjust velocity accordingly

* [Bubble](../OOGame/Bubble.cs) has no specific update

[Issues with design](OOGameIssues.md)