using Enin_Homework_08;
using System;
using System.Collections.Generic;

namespace Homework_08
{
#nullable enable
    [Serializable]
    public class Department
    {
        public int NumbOfWorkers { get; set; }
        public DateTime DateOfCreation { get; set; }
        public string Name { get; set; } = default!;

        public List<Worker> workers = default!;
        public Director director = default!;

        public Department() { }

        public Department(string name, Director dir )
        {
            Name = name;
            DateOfCreation = DateTime.Now;
            workers = new List<Worker>();
            NumbOfWorkers = 0;
            director = dir;
        }
        public void PrintDepInfo()
        {
            Console.WriteLine($"\n Department Name: {Name}\n" +
                $" Date of creation: {DateOfCreation}\n" +
                $" Number of Workers: {NumbOfWorkers}\n" +
                $" Department Director: {director.FirstName} {director.LastName} \n");
        }

        public void PrintWorkerList()
        {
            Console.WriteLine($"{"№",3}|{"ID",4}|{"First Name",12}|{"Last Name",14}|{"Age",5}|{"Department",15}|{"Pay",7}|{"Projects",9}");
            Console.WriteLine($"------------------------------------------------------------------------------");
            int count = 1;
            foreach (Worker w in workers)
            {
                Console.WriteLine($"{count,3}|{w.ID,4}|{w.FirstName,12}|{w.LastName,14}|{w.Age,5}|{w.DepartmentIN,15}|{w.Pay,7}|{w.NumbOfProjects,3}");
                count++;
            }
            Console.WriteLine();
        }

    }
#nullable restore
}
