using System;
using System.Collections.Generic;
using System.Text;

namespace Enin_Homework_08
{
    public class Director
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public int Age { get; set; } = default!;
        public int Pay { get; set; } = default!;

        public Director() { }
        public Director( string fname, string lname, int age, int pay)
        {
            FirstName = fname;
            LastName = lname;
            Age = age;
            Pay = pay;
        }
        public void PrintDirector()
        {
            Console.WriteLine(
                $"Department Director\n" +
                $"First name: {FirstName}\n" +
                $"Last name: {LastName}\n" +
                $"Age: {Age}\n" +
                $"Pay: {Pay}\n");
        }
    }
}

