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
                EditCourses();
            }
            catch (CourseException ce)
            {
                MessageBox.Show(ce.Message, "Error");
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                IOFile.WriteFile(Converter.CoursesToStrings(courses));
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
            }
            catch (Exception ex)
            {
                //Error(2);
                MessageBox.Show(ex.Message);
            }
        }





        private void EditCourses()
        {
            pnlNewCorses.Visible = true;
        }

        // Error message display
        private void Error(int v)
        {
            string message;

            switch (v)
            {
                case 1:
                    message = "Error : 001\n";
                    message += "File incorrect format or\nmissing or dialog cancelled";
                    break;
                case 2:
                    message = "Error : 002\n";
                    message += "File open error or dialog cancelled";
                    break;
                case 3:
                    message = "Error : 003\n";
                    message += "File save error";
                    break;
                case 4:
                    message = "Error : 004\n";
                    message += "Invalid Date";
                    break;
                case 5:
                    message = "Error : 005\n";
                    message += "Invalid Name of Course";
                    break;
                case 6:
                    message = "Error : 006\n";
                    message += "Invalid Cost of Course";
                    break;
                default:
                    message = "Error : 100\n";
                    message += "Unknown error";
                    break;
            }

            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            string message = "Cours " + txtCourseName.Text + " added\n";
            message += "On position " + courses.Count.ToString();
            MessageBox.Show(message, "Adding");
        }

        private void btnNextCoutse_Click(object sender, EventArgs e)
        {
            if (Validator.EndTyping(txtCourseName))
            {
                StartSetting();
                saveToolStripMenuItem.Enabled = true;
            }
            // for test only auto generate course
            else if (txtCourseName.Text.Length == 0)
            {
                Course course = Course.GetExample();
                Debug.WriteLine(course);
                courses.Add(course);
                CourseAdded();
            }
            else
            {
                if (!Validator.NameCourse(txtCourseName))
                {
                    Error(5);
                }
                else if (!Validator.DateCourse(dtpCourseDate))
                {
                    Error(4);
                }
                else if (!Validator.CostCourse(txtCourseCost))
                {
                    Error(6);
                }
                else
                {
                    courses.Add(new Course(txtCourseName.Text, dtpCourseDate.Text, txtCourseCost.Text, "FFFFFFFFFFFF"));
                    CourseAdded();
                    txtCourseCost.Clear();
                    txtCourseName.Focus();
                    txtCourseName.SelectAll();
                }
            }
        }

    }
}
