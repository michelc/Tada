using System;
using System.Collections.Generic;

namespace Tada
{
    public class ContextTool : BaseTool, ITool
    {
        public List<string> Entities { get; set; }

        public ContextTool(string command, string[] args) : base(command, args) { }

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

            Name = Project + "Context";
            Entities = new List<string>(Arguments);

            Console.WriteLine(ToCode());
            return;
        }

        public new void ShowHelp()
        {
            Console.Write($"{Title} ({Version}) - ");

            Console.WriteLine("Génère une classe Models/ProjetContext.cs");
            Console.WriteLine();
            Console.WriteLine("Utilisation : tada context [options] [arguments]");
            Console.WriteLine();
            Console.WriteLine("Options :");
            Console.WriteLine("  -h|--help                Affiche l'aide de la ligne de commande");
            Console.WriteLine("  -p|--project <PROJET>    Nom du projet pour définir le namespace");
            Console.WriteLine("                           (obtenu depuis PROJET.csproj sinon)");
            Console.WriteLine();
            Console.WriteLine("Arguments : Liste des entités du DbContext.");
            Console.WriteLine();
            Console.WriteLine("Exemples :");
            Console.WriteLine("  tada context Movie Genre Director --project MvcMovie");
            Console.WriteLine("  tada context Movie Genre Director");
            Console.WriteLine("  tada -p MvcBlog context Blog Post Comment");
            Console.WriteLine("  tada context -p MvcFacture Client Article Facture Ligne");
        }

        public new string ToCode()
        {
            var entities = "";
            foreach (var v in Entities)
            {
                entities += SPACES + "public DbSet<" + v + "> " + v + "s { get; set; }" + Environment.NewLine;
            }
            entities = entities.Substring(0, entities.Length - Environment.NewLine.Length);

            var code = $@"using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace {Project}.Models
{{
  public class {Name} : DbContext
  {{
    public {Name}(DbContextOptions<{Name}> options) : base(options) {{ }}

{entities}

    protected override void OnModelCreating(ModelBuilder modelBuilder) {{ }}
  }}
}}";

            return code;
        }
    }
}
