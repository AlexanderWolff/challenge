using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Vivacity
{
    class Map
    {
        public int x_length, y_length;
        public char[,] point;

        /// <summary>
        /// Constructor
        /// </summary>
        public Map(int X, int Y)
        {
            this.x_length = X;
            this.y_length = Y;

            //Populate With Empty Value ('.')
            this.point = newMap(X, Y);
        }

        /// <summary>
        /// Generates and initialises a new map of specified size
        /// </summary>
        private static char[,] newMap(int x_length, int y_length)
        {
            char[,] map = new char[x_length + 1, y_length + 1];

            for (int x = 0; x <= x_length; x++)
            {
                for (int y = 0; y <= y_length; y++)
                {
                    map[x, y] = '.';
                }
            }
            return map;
        }

        /// <summary>
        /// Generates a string representation of the Map
        /// </summary>
        public override string ToString()
        {
            string output = "";

            for (int y = 0; y <= y_length; y++)
            {
                for (int x = 0; x <= x_length; x++)
                {
                    output += point[x, y] + " ";
                }
                output += "\n";
            }
            return output;
        }
        public string ToString(bool nospace)
        {
            string output = "";

            for (int y = 0; y <= y_length; y++)
            {
                for (int x = 0; x <= x_length; x++)
                {
                    output += nospace ? point[x, y] + "" : point[x, y] + " ";
                }
                output += "\n";
            }
            return output;
        }

        /// <summary>
        /// Updates a point in the map
        /// </summary>
        public void populate(int x, int y)
        {
            this.point[x, y] = 'X';
        }

        /// <summary>
        /// Extract 2 integers representing X and Y positions from string coordinate in x(int)y(int) format
        /// </summary>
        static int[] extractXY(string coordinate)
        {
            //Initialise
            int[] XY = new int[2];
            string buffer = "";

            //Ignore first char (x)
            for (int i = 1; i < coordinate.Length; i++)
            {
                //Append characters until 'y' is reached
                if (coordinate[i] != 'y') buffer += coordinate[i];
                else
                {
                    XY[0] = Convert.ToInt32(buffer);

                    buffer = "";

                    //Ignore y and continue appending
                    for (int j = i + 1; j < coordinate.Length; j++)
                    {
                        buffer += coordinate[j];
                    }

                    XY[1] = Convert.ToInt32(buffer);
                    break;
                }
            }
            return XY;
        }

        /// <summary>
        /// Returns a set of coordinate strings from a specified file
        /// </summary>
        static string[] getCoordinates(string file_path)
        {
            //Initialise variable array
            List<string> coordinate_list = new List<string>();

            //Read file into memory
            var coordinate_set = File.ReadAllLines(file_path).Select(a => a.Split(',')).ToArray();

            foreach (var coordinates in coordinate_set)
            {
                foreach (string coordinate in coordinates)
                {
                    //Ignore empty sets
                    if (coordinate == "") continue;

                    //Append valid strings
                    coordinate_list.Add(coordinate);
                }
            }
            return coordinate_list.ToArray();
        }

        static public Map generateMap(string path)
        {
            //Initialise
            List<int> X_coordinates = new List<int>();
            List<int> Y_coordinates = new List<int>();

            //Read each file in the specified directory
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                foreach (string coordinate in getCoordinates(file))
                {
                    int[] XY = extractXY(coordinate);

                    X_coordinates.Add(XY[0]);
                    Y_coordinates.Add(XY[1]);
                }
            }

            //Create empty map
            Map map = new Map(X_coordinates.Max(), Y_coordinates.Max());

            //Populate map with coordinates
            for (int i = 0; i < X_coordinates.Count; i++)
            {
                map.populate(X_coordinates[i], Y_coordinates[i]);
            }

            Console.Write(map.ToString());

            return map;
        }
    }
}
