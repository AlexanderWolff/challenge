using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivacity
{
    class PathFinder
    {
        class Point
        {
            public int x, y, distance;

            public Point(int X, int Y, int D)
            {
                this.x = X;
                this.y = Y;
                this.distance = D;
            }
        }
        

        public Map original_map, final_map;

        int startX, startY, endX, endY;

        public PathFinder() { }
        public PathFinder(Map input_map)
        {
            this.original_map = input_map;
        }

        /// <summary>
        /// Source: https://en.wikipedia.org/wiki/Pathfinding
        /// </summary>
        public string findPath()
        {
            //Initialise
            this.final_map = original_map;

            //Calculate numbermap
            List<Point> numberMap = calculateNumberMap(endX, endY);

            //Find viable paths
            List<List<Point>> paths = calculateViablePaths(numberMap);

            //Find path with lowest sum of coordinate values
            List<Point> finalPath = new List<Point>();
            int coordinateSum = int.MaxValue;
            foreach(var path in paths)
            {
                if( sumCoordinates(path) < coordinateSum )
                {
                    coordinateSum = sumCoordinates(path);
                    finalPath = path;
                }
            }

            //Update Map
            foreach (var point in finalPath)
            {
                this.final_map.point[point.x, point.y] = 'O';
            }
            this.final_map.point[startX, startY] = 'S';
            this.final_map.point[endX, endY] = 'E';

            //Return coordinates as string
            string output = "";
            foreach (Point point in finalPath)
            {
                output += int_to_string(point.x);
                output += int_to_string(point.y);
            }

            return output;
        }

        private string int_to_string(int input)
        {
            string output = "";
            if(input < 9)
            {
                output += "0";
            }

            output += input.ToString();

            return output;
        }

        private int sumCoordinates(List<Point> path)
        {
            int sum = 0;

            foreach(Point point in path)
            {
                sum += point.x + point.y;
            }

            return sum;
        }

        private List<List<Point>> calculateViablePaths(List<Point> numberMap)
        {
            //Initialisation
            List<List<Point>> paths = new List<List<Point>>();
            List<Point> path = new List<Point>();

            //Starting Point
            path.Add( new Point(startX, startY, 0) );
            paths.Add(path);

            //For all different paths/forks
            for (int i = 0; i < paths.Count; i++)
            {
                List<Point> current_path = paths[paths.Count - 1];
                Point current_point = current_path[current_path.Count - 1];

                //While the end has not yet been reached
                while (!((current_point.x == endX) && (current_point.y == endY)))
                {
                    //Gather adjacent points
                    List<Point> adjacent = gatherAdjacent(current_point, numberMap);

                    //Find Lowest distance
                    int min_distance = int.MaxValue;
                    Point next_point = new Point(0,0,0);
                    foreach(Point adj in adjacent)
                    {
                        if( min_distance > adj.distance )
                        {
                            min_distance = adj.distance;
                            next_point = adj;
                        }
                    }

                    //IF fork: create new path
                    int path_count = 0;
                    foreach (Point adj in adjacent)
                    {
                        if (min_distance == adj.distance)
                        {
                            path_count++;
                            
                            if(path_count == 1)
                            {
                                //Append to current path
                                current_path.Add(next_point);

                                //Update current point
                                current_point = next_point;
                            }
                            if(path_count>1)
                            {
                                //Create new path
                                List<Point> new_path = new List<Point>(current_path);

                                //Append to new path
                                new_path[new_path.Count-1] = adj;
                                paths.Add(new_path);
                            }
                        }
                    }
                }

                paths[i] = current_path;
            }

            return paths;
        }

        private List<Point> gatherAdjacent(Point target, List<Point> numberMap)
        {
            List<Point> adjacent = new List<Point>();

            foreach(Point point in numberMap)
            {
                if (((Math.Abs(point.x - target.x) == 1) && (Math.Abs(point.y - target.y) == 0)) ||
                   ((Math.Abs(point.x - target.x) == 0) && (Math.Abs(point.y - target.y) == 1))) adjacent.Add(point);
            }

            return adjacent;
        }

        /// <summary>
        /// Generate numbermap used in implementing simple pathfinding algorithm
        /// </summary>
        private List<Point> calculateNumberMap(int X, int Y)
        {
            List<Point> root = new List<Point>();
            root.Add(new Point(X, Y, 0));

            for (int i = 0; i < 100; i++)
            {
                int limit = (int)(Math.Pow(4, i));

                List<Point> current_root = root;
                for (int j = 0; j < limit; j++)
                {
                    List<Point> buffer_path = new List<Point>();
                    Point current_point = current_root[j];

                    //Break IF starting point is reached
                    if (this.original_map.point[current_point.x, current_point.y] == 'S') break;

                    //Search adjacent squares
                    int current_distance = current_point.distance;

                    Point p;

                    p = new Vivacity.PathFinder.Point(current_point.x + 1, current_point.y, current_distance + 1);
                    if (filterPoint(root, p, current_distance) && (!isPresent(root, p))) root.Add(p);

                    p = new Vivacity.PathFinder.Point(current_point.x - 1, current_point.y, current_distance + 1);
                    if (filterPoint(root, p, current_distance) && (!isPresent(root, p))) root.Add(p);

                    p = new Vivacity.PathFinder.Point(current_point.x, current_point.y + 1, current_distance + 1);
                    if (filterPoint(root, p, current_distance) && (!isPresent(root, p))) root.Add(p);

                    p = new Vivacity.PathFinder.Point(current_point.x, current_point.y - 1, current_distance + 1);
                    if (filterPoint(root, p, current_distance) && (!isPresent(root, p))) root.Add(p);
                }
            }

            return root;
        }

        private int latestDistance(List<Point> paths)
        {
            int latest = 0;

            foreach(Point point in paths)
            {
                if (point.distance > latest) latest = point.distance;
            }

            return latest;
        }

        private List<Point> filterPoints(List<Point> paths, List<Point> input, int distance)
        {
            List<Point> output = new List<Point>();

            foreach(Point point in input)
            {
                //Make sure point is within maze
                if ((point.x > original_map.x_length) || (point.y > original_map.y_length)) continue;
                if ((point.x < 0) || (point.y < 0)) continue;

                //Make sure point is not a wall
                if (original_map.point[point.x, point.y] == 'X') continue;

                //Make sure point is not already filled with shorter path
                if (pointDistance(paths, point) <= distance) continue;

                //Approve point
                output.Add(point);
            }

            return output;
        }
        private bool filterPoint(List<Point> paths, Point point, int distance)
        {        
            //Make sure point is within maze
            if ((point.x > original_map.x_length) || (point.y > original_map.y_length)) return false;
            if ((point.x < 0) || (point.y < 0)) return false;

            //Make sure point is not a wall
            if (original_map.point[point.x, point.y] == 'X') return false;

            //Make sure point is not already filled with shorter path
            if (pointDistance(paths, point) <= distance) return false;

            //Approve point
            return true;
        }

        private bool isPresent(List<Point> paths, Point input)
        {
            foreach(Point point in paths)
            {
                if ((point.x == input.x) && (point.y == input.y)) return true;
            }
            return false;
        }

        private int pointDistance(List<Point> paths, Point input)
        {
            foreach(Point point in paths)
            {
                if((point.x == input.x)&&(point.y == input.y))
                {
                    return point.distance;
                }
            }
            return int.MaxValue;
        }

        /// <summary>
        /// Sets start coordinate
        /// </summary>
        public void setStart(int x, int y)
        {
            this.startX = x;
            this.startY = y;

            this.original_map.point[x, y] = 'S';
        }

        /// <summary>
        /// Sets end coordinate
        /// </summary>
        public void setEnd(int x, int y)
        {
            this.endX = x;
            this.endY = y;

            this.original_map.point[x, y] = 'E';
        }

        
    }
}
