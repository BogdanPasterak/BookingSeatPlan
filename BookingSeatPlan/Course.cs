using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSeatPlan
{
    public class Course
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public string Cost { get; set; }
        public string Seat { get; set; }

        public Course()
        {
        }

        public Course(string name, string date, string cost, string seat)
        {
            Name = name;
            Date = date;
            Cost = cost;
            Seat = seat;
        }

        public static Course GetExample()
        {
            Random rand = new Random();
            string[] names = {"C++ For Beginer", "Pascal", "Python From Base", "Java Script For Advanced" };
            string name = names[rand.Next(names.Length)];
            string date = DateTime.Today
                                .AddDays(5 + rand.Next(25))
                                .AddMonths(rand.Next(24))
                                .ToShortDateString();
            string cost = "£" + (20 + rand.Next(36) * 5 +
                                    ((rand.Next(2) > 0) ?
                                    ((rand.Next(2) > 0) ? 0 : .5) : .99))
                                    .ToString("F2");
            string seat = "FFFFFFFFFFFF";

            return new Course(name, date, cost, seat);
        }

        public override string ToString()
        {
            return "Course:[Name:" + Name + ", Date:" + Date + ", Cost:" + Cost + ", Seat:" + Seat +  "]";
        }
    }
}
