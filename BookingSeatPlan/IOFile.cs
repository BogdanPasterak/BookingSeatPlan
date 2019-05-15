using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace BookingSeatPlan
{
    class IOFile
    {
        private static string path = null;

        internal static void NewFile()
        {
            FileDialog fileDialog = new SaveFileDialog();
            fileDialog.Title = "New Courses List";
            fileDialog.Filter = "txt files (*.txt)|*.txt";
            fileDialog.RestoreDirectory = true;

            try
            {
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    path = fileDialog.FileName;
                    using (File.Create(path)) { }
                }
                else
                {
                    path = null;
                    throw new CourseException("1");
                }
            }
            catch (IOException)
            {
                path = null;
                throw new CourseException("1");
            }
        }

        internal static void OpenFile()
        {
            FileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Open Courses List";
            fileDialog.Filter = "txt files (*.txt)|*.txt";
            fileDialog.RestoreDirectory = true;

            try
            {
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    path = fileDialog.FileName;
                    using (Stream stream = File.Open(path, FileMode.Open))
                    { }
                }
                else
                {
                    path = null;
                    throw new CourseException("2");
                }
            }
            catch (IOException)
            {
                path = null;
                throw new CourseException("2");
            }
        }

        internal static string[] ReadFile()
        {
            if (path != null)
            {
                try
                {
                    return File.ReadAllLines(path);
                }
                catch (IOException)
                {
                    throw new CourseException("2");
                }
            }
            else
            {
                throw new CourseException("2");
            }

        }

        internal static void WriteFile(string[] lines)
        {
            if (path != null)
            {
                try
                {
                    File.WriteAllLines(path, lines);
                }
                catch (IOException)
                {
                    throw new CourseException("3");
                }
            }
            else
            {
                throw new CourseException("3");
            }
        }

        internal static void AppendFile(string[] lines)
        {
            if (path != null)
            {
                try
                {
                    File.AppendAllLines(path, lines);
                }
                catch (IOException)
                {
                    throw new CourseException("3");
                }
            }
            else
            {
                throw new CourseException("3");
            }
        }

        public static void CloseFile ()
        {
            path = null;
        }

        public static bool IsFileOpen()
        {
            return (path != null);
        }

    }
}
