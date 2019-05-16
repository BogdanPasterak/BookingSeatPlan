using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookingSeatPlan
{
    public partial class SeatPlan : Form
    {
        public bool Change { get; set; }
        List<Course> courses;
        string courseName;
        int[] coursesIndex;

        public SeatPlan(List<Course> courses, string courseName)
        {
            InitializeComponent();
            Change = false;
            this.courses = courses;
            this.courseName = courseName;
            ChoiseCourses();
            InitView();
        }

        private void InitView()
        {
            lblCourseName.Text = courseName;

            int counter = 0;
            int index;

            for (int nr = 0; nr < coursesIndex.Length; nr++)
            {
                index = coursesIndex[nr];
                for (int seat = 0; seat < 12; seat++)
                {
                    TextBox box = new TextBox();
                    box.Location = new Point(seat * 22, nr * 25);
                    box.TextAlign = HorizontalAlignment.Center;
                    box.Size = new Size(20, 20);
                    box.Tag = counter;
                    box.Click += Box_Click;

                    BookedBox(box, (courses[index].Seat[seat] == 'B'), seat);
                    pnlView.Controls.Add(box);
                    counter++;
                }
                Label date = new Label();
                date.Location = new Point(265, nr * 25);
                date.Text = courses[index].Date;
                date.TextAlign = ContentAlignment.MiddleCenter;
                pnlView.Controls.Add(date);
                Label cost = new Label();
                cost.Location = new Point(335, nr * 25);
                cost.TextAlign = ContentAlignment.MiddleCenter;
                cost.Text = courses[index].Cost;
                pnlView.Controls.Add(cost);

            }
        }

        private void ChoiseCourses()
        {
            int counter = 0;
            // count number of matched courses
            foreach (Course course in courses)
            {
                if (course.Name == courseName)
                {
                    counter++;
                }
            }
            coursesIndex = new int[counter];
            // store indexes
            counter = 0;
            foreach (Course course in courses)
            {
                if (course.Name == courseName)
                {
                    coursesIndex[counter++] = courses.IndexOf(course);
                }
            }

            SortCourses();

        }

        private void Box_Click(object sender, EventArgs e)
        {
            //Debug.WriteLine((sender as TextBox).Tag.ToString());
            TextBox box = sender as TextBox;
            int nr = int.Parse(box.Tag.ToString());
            int row = nr / 12;
            int seat = nr % 12;
            Change = true;
            if(box.Text == "B")
            {
                // change view
                BookedBox(box, false, seat);
                // change model
                char[] seats = courses[coursesIndex[row]].Seat.ToCharArray();
                seats[seat] = 'F';
                courses[coursesIndex[row]].Seat = new string(seats);
            }
            else
            {
                // change view
                BookedBox(box, true, seat);
                // change model
                char[] seats = courses[coursesIndex[row]].Seat.ToCharArray();
                seats[seat] = 'B';
                courses[coursesIndex[row]].Seat = new string(seats);
            }
            Debug.WriteLine(courses[coursesIndex[row]].Seat);


        }

        private void BookedBox(TextBox box, bool booked, int seat)
        {
            if (booked)
            {
                box.Text = "B";
                box.BackColor = Color.LightGreen;
            }
            else
            {
                box.Text = (seat + 1).ToString();
                box.BackColor = Color.LightGray;
            }
        }

        private void SortCourses()
        {
            Array.Sort(coursesIndex,
                delegate (int x, int y) {
                    return (DateTime.Parse(courses[x].Date)).CompareTo(DateTime.Parse(courses[y].Date));
                }
            ); 
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //Change = textBox1.Text;
        }
    }
}
