using BookingSeatPlan.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
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
        List<Booked> bookings;
        string courseName;
        int[] coursesIndex;
        static string[] textToPrintHeder, textToPrintDate, textToPrintCost, seatToPrint;

        public SeatPlan(List<Course> courses, string courseName)
        {
            InitializeComponent();
            // init data
            Change = false;
            bookings = new List<Booked>();
            this.courses = courses;
            this.courseName = courseName;
            // start seting
            ChoiseCourses();
            InitView();
        }

        // build view
        private void InitView()
        {
            lblCourseName.Text = courseName;

            int counter = 0;
            int index;

            // loop for courses with same name
            for (int nr = 0; nr < coursesIndex.Length; nr++)
            {
                index = coursesIndex[nr];
                // loop of seat
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
                // add date and cost label
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

        // create array with indexes of courses
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

        // when click on seat...
        private void Box_Click(object sender, EventArgs e)
        {
            //Debug.WriteLine((sender as TextBox).Tag.ToString());
            TextBox box = sender as TextBox;
            int nr = int.Parse(box.Tag.ToString());
            int row = nr / 12;
            int seat = nr % 12;
            //
            Change = true;
            Booked booked = new Booked();
            booked.IndexCourse = coursesIndex[row];
            booked.SeatNumber = seat;

            if (box.Text == "B")
            {
                // change view
                BookedBox(box, false, seat);
                // change model
                char[] seats = courses[coursesIndex[row]].Seat.ToCharArray();
                seats[seat] = 'F';
                courses[coursesIndex[row]].Seat = new string(seats);
                booked.Occupied = false;
            }
            else
            {
                // change view
                BookedBox(box, true, seat);
                // change model
                char[] seats = courses[coursesIndex[row]].Seat.ToCharArray();
                seats[seat] = 'B';
                courses[coursesIndex[row]].Seat = new string(seats);
                booked.Occupied = true;
            }
            //Debug.WriteLine(courses[coursesIndex[row]].Seat);
            //Debug.WriteLine(booked);
            // add to list of booked place
            if (!bookings.Contains(booked) && booked.Occupied)
            {
                bookings.Add(booked);
                Debug.WriteLine("bookeds size = " + bookings.Count.ToString());
            }

        }

        // change view of seat
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

        // sort array by date of courses
        private void SortCourses()
        {
            Array.Sort(coursesIndex,
                delegate (int x, int y) {
                    return (DateTime.Parse(courses[x].Date)).CompareTo(DateTime.Parse(courses[y].Date));
                }
            ); 
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string s = "";
            textToPrintHeder = new string[bookings.Count];
            textToPrintDate = new string[bookings.Count];
            textToPrintCost = new string[bookings.Count];
            seatToPrint = new string[bookings.Count];
            int index = 0;
            foreach (Booked booked in bookings)
            {
                textToPrintHeder[index] = "BOOKING NUMBER:\t" + (index + 1).ToString() + "\t";
                textToPrintHeder[index] += courses[booked.IndexCourse].Name;
                textToPrintDate[index] = courses[booked.IndexCourse].Date;
                textToPrintCost[index] = courses[booked.IndexCourse].Cost;
                seatToPrint[index] = courses[booked.IndexCourse].Seat;
                index++;
            }

            // print to printer
            PrintDialog printDlg = new PrintDialog();
            PrintDocument printDoc = new PrintDocument();
            printDoc.DocumentName = "Print Holidays";
            printDlg.Document = printDoc;
            printDlg.AllowSelection = true;
            printDlg.AllowSomePages = true;
            //Call ShowDialog
            if (printDlg.ShowDialog() == DialogResult.OK)
            {
                printDoc.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                printDoc.Print();
            }

        }

        private static void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            Font font = new Font("Consolas", 10);
            Font fontBold = new Font("Consolas", 10, FontStyle.Bold);
            Font fontHeder = new Font("Consolas", 15, FontStyle.Bold);
            string line = "-------------------------------------------------------------------------------------";
            Image chair = Resources.chair;
            Image user = Resources.user;

            //ev.Graphics.DrawPie(new Pen(Brushes.Red, 7), new RectangleF(new PointF(20, 10), new Size(100, 80)), 30, 320);
            //ev.Graphics.DrawEllipse(new Pen(Brushes.Blue, 2), new RectangleF(new PointF(80, 25), new Size(10, 10)));
            for (int i = 0; i < textToPrintHeder.Length; i++)
            {
                ev.Graphics.DrawString(textToPrintHeder[i], fontHeder, Brushes.Black, ev.MarginBounds.Left + 10, i * 100);
                ev.Graphics.DrawString("Date:", fontBold, Brushes.Black, ev.MarginBounds.Left , i * 100 + 35);
                ev.Graphics.DrawString(textToPrintDate[i], font, Brushes.Black, ev.MarginBounds.Left + 60, i * 100 + 35);
                ev.Graphics.DrawString("Cost:", fontBold, Brushes.Black, ev.MarginBounds.Left, i * 100 + 60);
                ev.Graphics.DrawString(textToPrintCost[i], font, Brushes.Black, ev.MarginBounds.Left + 60, i * 100 + 60);
                ev.Graphics.DrawString(line, font, Brushes.Black, ev.MarginBounds.Left, i * 100 + 80);
                char[] seat = seatToPrint[i].ToCharArray();
                for (int j = 0; j < 12; j++)
                    ev.Graphics.DrawImage(((seat[j] == 'B') ? user: chair), 250 + j * 47, i * 100 + 25);

            }
            //ev.Graphics.DrawString(textToPrint, font, Brushes.Black, ev.MarginBounds.Left, 0, new StringFormat());
            //ev.Graphics.DrawString(textToPrintHeder, fontBold, Brushes.Black, ev.MarginBounds.Left, 0, new StringFormat());
            //ev.Graphics.DrawString(textToPrintTitle, fontHeder, Brushes.Black, ev.MarginBounds.Left, 0, new StringFormat());
        }


    }
}
