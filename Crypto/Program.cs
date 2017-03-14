using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Vivacity
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("---\tPROGRAM START\t---");

            //Task Select
            int task = 4;
            PathFinder pathfinder = new PathFinder();
            switch (task)
            {
                case 1:
                    //Task 1 + Write output to a file
                    System.IO.File.WriteAllText(@"CoordinateSystem\output_task1.txt", Hashing.getCode("code-quality", 3));
                    break;

                case 2:
                    //Task 2 + Write output to a file
                    Map taskmap = Map.generateMap(@"CoordinateSystem");
                    System.IO.File.WriteAllText(@"CoordinateSystem\output_task2.txt", taskmap.ToString(true));
                    break;

                case 3:
                    //Task 3
                    int map = 1;
                    switch (map)
                    {

                        case 1: //Small Map
                            pathfinder = new PathFinder(Map.generateMap(@"CoordinateSystem\task3_test"));
                            pathfinder.setStart(0, 2);
                            pathfinder.setEnd(5, 2);

                            break;

                        case 2: //Big Map
                            pathfinder = new PathFinder(Map.generateMap(@"CoordinateSystem"));
                            pathfinder.setStart(1, 1);
                            pathfinder.setEnd(17, 27);
                            break;

                    }

                    pathfinder.findPath();

                    Console.WriteLine("\n");
                    Console.WriteLine(pathfinder.final_map.ToString());

                    break;

                case 4:
                    //Task 4 - Integration
                    pathfinder = new PathFinder(Map.generateMap(@"CoordinateSystem\task3_test"));
                    pathfinder.setStart(0, 2);
                    pathfinder.setEnd(5, 2);
                    string code = pathfinder.findPath();

                    //Print Maze to Console
                    Console.WriteLine("\n");
                    Console.WriteLine(pathfinder.final_map.ToString());

                    //Gets Hash
                    Hashing.getCode(code, 3);

                    break;

                default:
                    Console.WriteLine("Incorrect Task Number");
                    break;
            }

            //Await Program End
            Console.WriteLine("---\tPROGRAM END\t---");
            var end = Console.ReadKey();
        }




        

    }        
}
