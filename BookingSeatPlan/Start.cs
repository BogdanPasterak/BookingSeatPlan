using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookingSeatPlan
{
    public partial class Start : Form
    {
        List<Course> courses;
        bool change = false;

        public Start()
        {
            InitializeComponent();
            StartSetting();
            //Debug.WriteLine(Converter.CoursesToStrings(Course.GetExample())[2]);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                IOFile.NewFile();
                StartSetting();
                courses = new List<Course>();
                ShowEditCourses();
            }
            catch (CourseException ce)
            {
                MessageBox.Show(ce.Message, "Error");
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            try
            {
                IOFile.WriteFile(Converter.CoursesToStrings(courses));
                MessageBox.Show("Data has been saved", "Save");
            }
            catch (CourseException ce)
            {
                MessageBox.Show(ce.Message, "Error");
            }

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                IOFile.OpenFile();

                courses = Converter.StringsToCourses( IOFile.ReadFile() );
                foreach (Course course in courses)
                {
                    Debug.WriteLine(course);
                }
                saveToolStripMenuItem.Enabled = true;
                AddItemToCombo();
                ShowEditCourses();
            }
            catch (CourseException ce)
            {
                MessageBox.Show(ce.Message, "Error");
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // if any change ask about saving that
            if (change)
            {
                if( MessageBox.Show("Do you want to save changes", "Exit",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Save();
                }
            }
            Close();
        }

        private void cbCourses_SelectedIndexChanged(object sender, EventArgs e)
        {
            // if choise course name
            if (cbCourses.SelectedIndex > 0)
            {
                string courseName = cbCourses.SelectedItem.ToString();
                // open new form with parametrs and get answer if any change
                SeatPlan seatPlan = new SeatPlan(courses, courseName);
                if (seatPlan.ShowDialog() == DialogResult.OK)
                {
                    //Debug.WriteLine(seatPlan.Change.ToString());
                    // any changes
                    change = change || seatPlan.Change;
                    cbCourses.SelectedIndex = 0;
                }
                
            }
        }

        private void AddItemToCombo()
        {
            List<string> names = new List<string>();
            names.Add("Not any");

            foreach (Course course in courses)
            {
                if (!names.Contains(course.Name))
                {
                    names.Add(course.Name);
                }
            }

            cbCourses.Items.Clear();
            foreach (string name in names)
            {
                cbCourses.Items.Add(name);
            }
            cbCourses.SelectedIndex = 0;

        }

        private void ShowEditCourses()
        {
            pnlNewCorses.Visible = true;
        }


        private void StartSetting()
        {
            pnlNewCorses.Visible = false;
            txtCourseName.Clear();
            txtCourseCost.Clear();
            dtpCourseDate.Value = DateTime.Today.AddMonths(1);
        }

        private void CourseAdded()
        {
            Course course = courses[courses.Count - 1];
            string message = "Cours " + course.Name + " added\n";
            message += "On position " + courses.Count.ToString();
            MessageBox.Show(message, "Adding");
            AddItemToCombo();
        }

        private void btnNextCoutse_Click(object sender, EventArgs e)
        {
            if (Validator.EndTyping(txtCourseName))
            {
                StartSetting();
                saveToolStripMenuItem.Enabled = true;
                return;
            }
            // for test only auto generate course
            if (txtCourseName.Text.Length == 0)
            {
                Course course = Course.GetExample();
                Debug.WriteLine(course);
                txtCourseName.Text = course.Name;
                dtpCourseDate.Text = course.Date;
                txtCourseCost.Text = course.Cost;
            }

            if (!Validator.NameCourse(txtCourseName))
            {
                string message = "Error : 005\nInvalid Name of Course";
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!Validator.DateCourse(dtpCourseDate))
            {
                string message = "Error : 004\nInvalid Date";
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!Validator.CostCourse(txtCourseCost))
            {
                string message = "Error : 006\nInvalid Cost of Course";
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (courses.Count >= 100)
            {
                MessageBox.Show("Error : 007\nMaximum 100 Coures");
            }
            else if (NumberCourses(txtCourseName.Text) >= 10)
            {
                MessageBox.Show("Error : 008\nMaximum 10 Coures with the same name");
            }
            else
            {
                courses.Add(new Course(txtCourseName.Text, dtpCourseDate.Text, txtCourseCost.Text, "FFFFFFFFFFFF"));
                CourseAdded();
                txtCourseCost.Clear();
                txtCourseName.Focus();
                txtCourseName.SelectAll();
                change = true;
            }
        }

        private int NumberCourses(string name)
        {
            int number = 0;
            foreach (Course course in courses)
            {
                if (course.Name == name)
                {
                    number++;
                }
            }
            return number;
        }
    }
}
