using System;
using System.Diagnostics;
using System.IO;

namespace Tada
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Debugger.IsAttached)
            {
                var temp = "";
                // Exemples abrégé / complet
                temp = "model Movie Movie_ID:int Title:+string(60) ReleaseDate:DateTime Genre Price:decimal(18,2) Rating:+Enum Directors:[]";
                temp = "model Movie Movie_ID:int Title:+string(60) ReleaseDate:DateTime Genre:Genre Price:decimal(18,2) Rating:+RatingEnum Directors:Director[]";
                // Exemples pour MvcMovie
                temp = "model Movie Movie_ID:int Title:+string(60) ReleaseDate:DateTime Genre:Genre Price:decimal(18,2) Rating:+RatingEnum Directors:Director[]";
                temp = "model -p MvcMovie Genre Genre_ID:int Title:+string(30) Movies:Movie[]";
                temp = "model Director Director_ID:int Name:+string(60) Movies:Movie[] --project MvcMovie";
                temp = "enum RatingEnum NA G PG PG_13 R NC_17";
                temp = "context Genre Director Movie";

                args = temp.Split(" ");

                Console.WriteLine("c:\\debug> tada " + temp);
                Console.WriteLine();
            }

            Run(args);

            if (Debugger.IsAttached)
            {
                Console.WriteLine();
                Console.Write("c:\\debug> Appuyer sur une touche... ");
                Console.ReadLine();
            }
        }

        static void Run(string[] args)
        {
            //
            var arguments = new Arguments(args);

            // Quand on demande la version
            var version = arguments.GetBoolOption("version");
            if (version)
            {
                Console.WriteLine(GetToolsVersion());
                return;
            }

            // Quand on demande de l'aide
            var help = arguments.GetBoolOption("help");

            // Définition du projet concerné
            var project = arguments.GetStringOption("project");
            if (project == "")
            {
                // Ou prend le nom du fichier .csproj
                var dir = Directory.GetCurrentDirectory();
                var index = dir.IndexOf(@"\bin\Debug\");
                if (index > 0) dir = dir.Substring(0, index);
                var files = Directory.GetFiles(dir, "*.csproj");
                if (files.Length == 1)
                {
                    project = files[0].Replace(dir + @"\", "").Replace(".csproj", "");
                }
            }

            var tool = GetTool(arguments);
            tool.Title = GetToolsTitle();
            tool.Version = GetToolsVersion();
            tool.Help = help;
            tool.Project = project;

            tool.Run();
        }

        private static ITool GetTool(Arguments arguments)
        {
            var command = arguments.Pop().ToLower();

            if (command == "model") return new ModelTool(command, arguments.ToArray());
            if (command == "enum") return new EnumTool(command, arguments.ToArray());
            if (command == "context") return new ContextTool(command, arguments.ToArray());
            return new BaseTool(command, arguments.ToArray());
        }

        private static string GetToolsTitle()
        {
            return "Tada Command-Line Tools";
        }

        private static string GetToolsVersion()
        {
            return "0.0.1";
        }
    }
}
