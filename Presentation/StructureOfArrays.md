# Structure of Arrays

[GameObject](../CompositionGame/GameObject.cs) is merely serving as a logical grouping of data. There's no need to put a hard constraint on this grouping.

## How then?

Put each component in their own array, but ensure that the same index in all arrays refer to the same "object"

| | 0 | 1 | 2 | 3 | 4 |
|:---:|:---:|:---:|:---:|:---:|:---:|
|Position | A | B | C | D | E |
|Velocity | A | B | C | D | E |

A different approach to transform data is required though.

[Back to rules](GameRules.md)