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
            _cmd.Add("upload", UploadCommand);
            _cmd.Add("show", ShowCommand);
            _cmd.Add("open" , OpenCommand);
            _cmd.Add("search", SearchCommand);

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
                        throw new ArgumentException("Unknown command");

                    _cmd[query[.. query.IndexOf(' ')]].Invoke(query.Substring(query.IndexOf(' ') + 1).Trim());
                }
                catch (ArgumentException e)
                {
                    Writer.Colored(e.Message, ConsoleColor.Red);
                }
                catch (InvalidDataException e)
                {
                    _depth--;
                    Writer.Colored(e.Message, ConsoleColor.Blue);
                }
                catch (ApplicationException e)
                {
                    Writer.Colored(e.Message, ConsoleColor.DarkMagenta);
                }
                catch (Exception)
                {
                    continue;
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

            if(_depth == 3)
                throw new ArgumentException("You can't go any deeper (._.')");

            if (!int.TryParse(query, out int id))
                throw new ArgumentException("Unknown command");

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
        private void UploadCommand(string query)
        {
            if(_depth != 3)
                throw new ApplicationException("You should choose a course directory");

            if(!int.TryParse(query[..query.IndexOf(' ')], out int id))
                throw new ApplicationException("Wrong course id");

            query = query.Substring(query.IndexOf(' ')).Trim();

            if (!File.Exists(query))
                throw new ApplicationException("File not found");

            Menu.UploadFile(id, query);
        }
        private void ShowCommand(string query)
        {
            if (_depth != 3)
                throw new ApplicationException("You should choose a course directory");

            if (!int.TryParse(query, out int id))
                throw new ApplicationException("Wrong course id");

            var files = Menu.ShowFiles(id);
            if(!files.Any())
                throw new ApplicationException("Course does not contain files");

            foreach(var file in files)
                Writer.WriteLine($"{file.Name}\t{file.CreationTime}");

            _depth = 4;
        }
        private void OpenCommand(string query)
        {
            var fileContent = Menu.ReadFile(query);

            if(fileContent == null)
                throw new ArgumentException("Wrong file name");

            Writer.WriteLine(fileContent);
        }
        private void SearchCommand(string query)
        {
            var files = Menu.SearchFiles(_depth, query);

            if (!files.Any())
                throw new ApplicationException("No pattern matching files");

            foreach (var file in files)
                Console.WriteLine(file);
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
