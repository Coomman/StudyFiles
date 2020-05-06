using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StudyFiles.Core;

namespace StudyFiles.UI
{
    public static class Extensions
    {
        public static void Colored(this TextWriter writer, string str, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(str);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    public class Client
    {
        private TextReader Reader { get; }
        private TextWriter Writer { get; }
        private Facade Menu { get; } = new Facade();
        private bool IsRunning { get; set; } = true;

        private readonly Dictionary<string, Action<string>> _cmd = new Dictionary<string, Action<string>>();
        private int _depth;

        public Client(TextReader input, TextWriter output)
        {
            Reader = input;
            Writer = output;

            _cmd.Add("cd", CdCommand);
            _cmd.Add("add", AddCommand);

            Visualize();
        }

        public void LoadMenu()
        {
            while (IsRunning)
            {
                Writer.Write(">>> ");
                try
                {
                    string query = Reader.ReadLine();

                    if (query == "exit")
                        break;

                    if (query == "ls")
                    {
                        Visualize(-1);
                        continue;
                    }

                    if (query == "clear")
                    {
                        Console.Clear();
                        continue;
                    }

                    if (!query.Contains(' '))
                        throw new ArgumentException();

                    _cmd[query[.. query.IndexOf(' ')]].Invoke(query.Substring(query.IndexOf(' ') + 1).Trim());
                }
                catch (ArgumentException)
                {
                    Writer.Colored("Unknown command", ConsoleColor.Red);
                }
                catch (InvalidDataException e)
                {
                    _depth--;
                    Writer.Colored(e.Message, ConsoleColor.Blue);
                }
            }
        }

        private void CdCommand(string query)
        {
            if (query == "..")
            {
                _depth = Math.Max(0, _depth - 1);
                Visualize();
                return;
            }

            if (!int.TryParse(query, out int id))
                throw new ArgumentException();

            _depth = Math.Min(3, _depth + 1);
            Visualize(id);
        }
        private void AddCommand(string query)
        {
            switch (_depth)
            {
                case 0:
                    Menu.AddUniversity(query);
                    break;
                case 1:
                    Menu.AddFaculty(query);
                    break;
                case 2:
                    Menu.AddDiscipline(query);
                    break;
                case 3:
                    Menu.AddCourse(query);
                    break;
            }

            Visualize();
        }

        public void Visualize(int id = -1)
        {
            switch (_depth)
            {
                case 0:
                    ShowUniversities();
                    break;
                case 1:
                    ShowFaculties(id);
                    break;
                case 2:
                    ShowDisciplines(id);
                    break;
                case 3:
                    ShowCourses(id);
                    break;
            }
        }

        private void ShowUniversities()
        {
            var unis = Menu.GetUniversities();
            if (!unis.Any())
                throw new InvalidDataException("Empty Universities list");

            foreach (var uni in unis)
                Writer.WriteLine($"{uni.ID} - {uni.Name}");
        }
        private void ShowFaculties(int id)
        {
            var facs = Menu.GetFaculties(id);
            if (!facs.Any())
                throw new InvalidDataException("Empty Faculties list");

            foreach (var fac in facs)
                Writer.WriteLine($"{fac.ID} - {fac.Name}");
        }
        private void ShowDisciplines(int id)
        {
            var disc = Menu.GetDisciplines(id);
            if (!disc.Any())
                throw new InvalidDataException("Empty Disciplines list");

            foreach (var dis in disc)
                Writer.WriteLine($"{dis.ID} - {dis.Name}");
        }
        private void ShowCourses(int id)
        {
            var cour = Menu.GetCourses(id);
            if (!cour.Any())
                throw new InvalidDataException("Empty Courses list");

            foreach (var course in cour)
                Writer.WriteLine($"{course.ID} - {course.Teacher}");
        }
    }
}
