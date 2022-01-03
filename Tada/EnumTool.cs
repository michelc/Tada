using System;
using System.Collections.Generic;
using System.Linq;

namespace Tada
{
    public class EnumTool : BaseTool, ITool
    {
        public List<string> Values { get; set; }

        public EnumTool(string command, string[] args) : base(command, args) { }

        public new void Run()
        {
            if (Help || (Arguments.Length == 0))
            {
                ShowHelp();
                return;
            }

            if (Project == "")
            {
                ErrorProject();
                return;
            }

            Name = Arguments[0];
            Values = new List<string>(Arguments).Skip(1).ToList();

            Console.WriteLine(ToCode());
            return;
        }

        public new void ShowHelp()
        {
            Console.Write($"{Title} ({Version}) - ");

            Console.WriteLine("Génère une classe Models/NomEnum.cs");
            Console.WriteLine();
            Console.WriteLine("Utilisation : tada enum [options] [arguments]");
            Console.WriteLine();
            Console.WriteLine("Options :");
            Console.WriteLine("  -h|--help                  Affiche l'aide de la ligne de commande");
            Console.WriteLine("  -p|--project <PROJET>      Nom du projet pour définir le namespace");
            Console.WriteLine("                             (obtenu depuis PROJET.csproj sinon)");
            Console.WriteLine();
            Console.WriteLine("Arguments : Nom de l'enum suivi de ses valeurs");
            Console.WriteLine();
            Console.WriteLine("Exemples :");
            Console.WriteLine("  tada enum Rating Enfants Interdit_moins_13_ans Interdit_mineurs Tout_public --project MvcMovie");
            Console.WriteLine("  tada enum Rating Enfants Interdit_moins_13_ans Interdit_mineurs Tout_public");
            Console.WriteLine("  tada enum -p MvcFacture Tva Normal Intermédiaire Réduit Particulier Zéro");
        }

        public new string ToCode()
        {
            var values = "";
            foreach (var v in Values)
            {
                values += SPACES + v + "," + Environment.NewLine;
            }
            values = values.Substring(0, values.Length - Environment.NewLine.Length);
            values = values.Substring(0, values.Length - ",".Length);

            var code = $@"namespace {Project}.Models
{{
  public enum {Name}
  {{
{values}
  }}
}}";

            return code;
        }
    }
}
