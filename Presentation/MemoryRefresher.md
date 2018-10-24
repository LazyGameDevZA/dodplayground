# Memory Allocation Refresher

Depending on the type you're creating in memory there's an impact on where stuff goes in memory.

## Stack vs Heap

Value vs Reference types go different places.

![StackVSHeap](https://csharpcorner-mindcrackerinc.netdna-ssl.com/UploadFile/rmcochran/csharp_memory01122006130034PM/Images/heapvsstack1.gif)

More info can be found [here](https://www.c-sharpcorner.com/article/C-Sharp-heaping-vs-stacking-in-net-part-i/).

## Typical memory allocation

Object graphs end up randomly scattered throughout the heap.

![Simplified Memory](https://software.intel.com/sites/default/files/managed/fb/d7/started-with-the-new-unity-entity-component-fig3.png)

More info can be found [here](https://software.intel.com/en-us/articles/get-started-with-the-unity-entity-component-system-ecs-c-sharp-job-system-and-burst-compiler)

[Next](ArrayOfStructures.md)