# C# DOD Playground

Inspiration for this project came from  and serves as an experiment to better understand how a Data-Oriented Design might be approached in C#. The basic version of this project is a bunch of dots flying about on the screen and having their speed altered by the bubbles they pass through.

## Getting Started

Project setup should be relatively simple. Once the repository had been cloned simply restore all NuGet packages and issue a full build twice. Due to a slight oddity in how content is built within MonoGame there were some issues in having the common content copy correctly to the output directory.

It's also advisable to install the MonoGame sdk as it takes care of installing the required C++ redistributables, but the most common content build issue that arises is remedied by installing the redistributable from [here](http://www.microsoft.com/en-NZ/download/details.aspx?id=30679). 

## Running

Each of the examples can be found under the "01 - Game" solution folder. There are 3 different samples:

* More traditional OOP approach. Dots and Bubbles both inherit from a base GameObject class.
* Naive Composition approach. Game logic has been moved into components contained within lists on a GameObject.
* Component driven approach where logic lives in two systems with an SoA (Structure of Arrays) driving "GameObject" like behaviour.

## Acknowledgments

* Aras Pranckevičius with [these slides](https://aras-p.info/texts/files/2018Academy%20-%20ECS-DoD.pdf) he put together to demonstrate to the Data-Oriented Design principles Unity tries to apply with their ECS implementation.
* Richard Fabian's [Data-Oriented Design](http://www.dataorienteddesign.com/site.php) provided a number of insights into the topic.
* Jacco Bikker's tutorial on [Practical SIMD Programming](http://www.cs.uu.nl/docs/vakken/magr/2017-2018/files/SIMD%20Tutorial.pdf)

Many more sources can be found [here](https://github.com/dbartolini/data-oriented-design) and it's also seeing updates as more sources become available.

## Presentation

[Go to presentation](Presentation/Index.md)