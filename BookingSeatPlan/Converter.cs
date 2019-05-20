using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookingSeatPlan
{
    class Converter
    {
        internal static string[] CoursesToStrings(Course course)
        {
            string[] lines = new string[4];

            lines[0] = AddQuotes(course.Name);
            lines[1] = AddQuotes(course.Date);
            lines[2] = AddQuotes(course.Cost);
            lines[3] = AddQuotes(course.Seat);

            return lines;
        }

        internal static string[] CoursesToStrings(List<Course> courses)
        {
            string[] lines = new string[courses.Count * 4];
            int counter = 0;

            foreach (Course course in courses)
            {
                string[] courseLines = CoursesToStrings(course);
                foreach (string line in courseLines)
                {
                    lines[counter++] = line;
                }
            }

            return lines;
        }

        internal static List<Course> StringsToCourses(string[] lines)
        {
            List<Course> courses = new List<Course>(); 
            int maxLines = (lines.Length / 4) * 4;
            string name, date, cost, seat;

            for (int i = 0; i < maxLines; )
            {
                name = RemoveQuote(lines[i++]);
                date = RemoveQuote(lines[i++]);
                cost = RemoveQuote(lines[i++]);
                seat = RemoveQuote(lines[i++]);

                if (name.Length > 0 && Validator.Date(date) && Validator.Cost(cost) && Validator.Seat(seat))
                {
                    courses.Add(new Course(name, date, cost, seat));
                }
                else
                {
                    MessageBox.Show("Incorect Data in File" +
                        "\nName:" + name +
                        "\nDate:" + date +
                        "\nCost:" + cost +
                        "\nSeat:" + seat );
                    break;
                }
            }


            return courses;
        }

        private static string RemoveQuote(string s)
        {
            return s.Trim().Replace("\"", "").Trim();
        }

        private static string AddQuotes(string s)
        {
            return "\"" + s + "\"";
        }

    }
}
