using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivacity
{
    class Integration
    {
        /// <summary>
        /// Task 4 (integration) function - does not output to file as that was not specified
        /// </summary>
        static void Task4(int zeros, string directory_path, int startX, int startY, int endX, int endY)
        {
            PathFinder pathfinder = new PathFinder(Map.generateMap(directory_path));
            pathfinder.setStart(0, 2);
            pathfinder.setEnd(5, 2);
            string code = pathfinder.findPath();

            //Print Maze to Console
            Console.WriteLine("\n");
            Console.WriteLine(pathfinder.final_map.ToString());

            //Gets Hash
            Hashing.getCode(code, zeros);
        }
    }
}
