# Array of Structures

Moving all data into struct types would yield much more compact data-layout.

Why not throw away the dynamic list on [GameObject](../CompositionGame/GameObject.cs) as well?

This could leave a memory footprint for GameObject similar to the following: 

| GameObject A | GameObject B | GameObject C | GameObject D | GameObject E |
|:---:|:---:|:---:|:---:|:---:|
|Position Velocity | Position Velocity | Position Velocity | Position Velocity | Position Velocity |

There is [another way](StructureOfArrays.md) as well