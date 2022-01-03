using System;

namespace Tada
{
    public class BaseTool : ITool
    {
        public string Title { get; set; }
        public string Version { get; set; }
        public bool Help { get; set; }
        public string Project { get; set; }
        public string Command { get; set; }
        public string Name { get; set; }
        public string[] Arguments { get; set; }

        public const string SPACES = "    ";

        public BaseTool(string command, string[] args)
        {
            Command = command;
            Arguments = args;
        }

        public void Run()
        {
            ShowHelp();
        }

        public string ToCode()
        {
            return "";
        }

        public void ShowHelp()
        {
            Console.WriteLine($"{Title} ({Version})");
            Console.WriteLine();
            Console.WriteLine("Utilisation : tada [options] [commande] [arguments]");
            Console.WriteLine();
            Console.WriteLine("Options :");
            Console.WriteLine("  -h|--help                  Affiche l'aide de la ligne de commande");
            Console.WriteLine("  -p|--project <PROJET>      Nom du projet pour définir le namespace");
            Console.WriteLine("  --version                  Affiche la version utilisée");
            Console.WriteLine();
            Console.WriteLine("Commandes :");
            Console.WriteLine("  model                      Génère une classe Models/NomEntite.cs");
            Console.WriteLine("  enum                       Génère une classe Models/NomEnum.cs");
            Console.WriteLine("  context                    Génère une classe Models/ProjetContext.cs");
            Console.WriteLine();
            Console.WriteLine("Pour plus d'informations sur une commande, lancer 'tada [commande] --help'.");
        }

        public void ErrorProject()
        {
            Console.WriteLine($"{Title} ({Version})");
            Console.WriteLine();
            Console.WriteLine("Projet introuvable :");
            Console.WriteLine("- L'option --project <PROJET> n'est pas définie");
            Console.WriteLine("- Il n'existe pas de fichier PROJET.csproj dans le dossier en cours");
        }
    }
}

