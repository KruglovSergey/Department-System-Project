using System;
namespace Homework_08
{
    #nullable enable
    [Serializable]
    public class Worker
    {
        public int ID { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public int Age { get; set; }
        public string DepartmentIN { get; set; } = default!;
        public int Pay { get; set; }
        public int NumbOfProjects { get; set; }

        public Worker() { }
        public Worker(int id, string fname,string lname,int age, string depName, int pay,int projects)
        {
            ID = id;
            FirstName = fname;
            LastName = lname;
            Age = age;
            DepartmentIN = depName;
            Pay = pay;
            NumbOfProjects = projects;
        }
        public void LukePrintWorker()
        {
            Console.WriteLine(
                $"ID: {ID}\n"+
                $"First name: {FirstName}\n" +
                $"Last name: {LastName}\n" +
                $"Age: {Age}\n" +
                $"Department: {DepartmentIN}\n" +
                $"Pay: {Pay}\n" +
                $"Number of projects: {NumbOfProjects}\n");
        }
       
    }
#nullable restore
}
