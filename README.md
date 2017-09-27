# Cellular Automata Cave Generation System

This is a Unity3D editor plugin to create procedurally generated caves using the cellular automata model and marching cubes algorithm.

## Getting Started

Download all the files into your Unity3D project folder.
Run by clicking Tools/Cellular Automata Terrain Tool 3D

### Prerequisites

```
Unity3D version 5.6.0f3 or above
```

### Installing

Open a Unity project and import all the files into it.
Open the editor and click on the Tools menu and on Cellular Automata Terrain Tool 3D
You are ready to run

## Running the generator

First step is to generate the initial grid, be very careful with very large sizes.
Next you should fill the grid with random values.
Next you should smooth out the result, this uses the cellular automata algorithm on 3d, checking all 26 neighboors, make sure to test and adjust the lower and upper limits accordingly.
After you are happy with the preview result you can click on generate mesh to create the mesh.

### Break down into end to end tests



## Built With

* [Unity3D](https://unity3d.com/) - The world’s favorite game engine
* [Marching Cubes](https://github.com/Scrawk/Marching-Cubes) - Marching Cubes Algorithm


## Authors

* **Carlos Augusto Restrepo** - *Initial work* - [Tuto96](https://github.com/Tuto96)

## License

This project is licensed under the CC Zero v1.0 Universal

## Acknowledgments

* Digital Dust for their marching cubes implementation
* Sebastian Lague for his amazing cellular automata tutorial
* Vancouver Film School
