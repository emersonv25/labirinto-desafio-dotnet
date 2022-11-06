## maze challenge-dotnet
This project was developed for a job opportunity challenge

## challenge

An entry will be given in a text file, where the first line contains the dimensions of the maze
(Lines Columns) and in the other lines the labyrinth itself, in which:
- 1 indicates a wall (i.e. cannot follow at this point in the matrix)
- 0 indicates a possible path to travel
- X is the starting point (not necessarily a corner of the map)

The objective is to find the only way out, without "walking the walls" and following the following order of
priority (when you can move):
1) Jump up (C)
2) Go left (E)
3) Go right (D)
4) Go down (B)

If a point is reached where it is not possible to move and/or there are no more positions
that have not yet been visited, you must return using the same path used up to this point
“dead end” to the last point where it had more than one possible position of movement. the order
movement is only used when there is more than one possible movement position for positions
not yet visited
