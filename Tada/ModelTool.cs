using System;
using System.Collections.Generic;

namespace Tada
{
    public class ModelTool : BaseTool, ITool
    {
        public List<ModelColumn> Columns { get; set; }

        public ModelTool(string command, string[] args) : base(command, args) { }

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
            Columns = new List<ModelColumn>();

            var AnyKey = false;
            for (var a = 1; a < Arguments.Length; a++)
            {
                var Description = (Arguments[a] + ":").Split(":");
                var Name = Description[0];
                var Type = Description[1];
                int Size = 0;
                int Precision = 0;
                int Scale = 0;
                var IsRequired = false;
                var IsKey = false;
                if (Name.ToLower().EndsWith("_id"))
                {
                    if (!AnyKey) IsKey = true;
                    AnyKey = true;
                }
                if (Type.StartsWith("+"))
                {
                    Type = Type.Substring(1);
                    IsRequired = true;
                }
                if (Type.EndsWith("[]"))
                {
                    Type = Type.Substring(0, Type.Length - 2);
                    if (Type == "") Type = Name.Substring(0, Name.Length - 1);
                    Type = "ICollection<" + Type + ">";
                }
                if (Type.Contains("("))
                {
                    var index = Type.IndexOf("(");
                    var size = Type.Substring(index + 1);
                    size = size.Substring(0, size.Length - 1);
                    Type = Type.Substring(0, index);
                    if (size.Contains(","))
                    {
                        Precision = Convert.ToInt32(size.Split(",")[0]);
                        Scale = Convert.ToInt32(size.Split(",")[1]);
                    }
                    else if (Type.ToLower() == "string")
                    {
                        Size = Convert.ToInt32(size);
                    }
                    else
                    {
                        Precision = Convert.ToInt32(size);
                    }
                }

                var Column = new ModelColumn();
                Column.Name = Name;
                Column.Type = Type;
                Column.IsRequired = IsRequired;
                Column.IsKey = IsKey;
                Column.Size = Size;
                Column.Precision = Precision;
                Column.Scale = Scale;
                Columns.Add(Column);
            }

            Console.WriteLine(ToCode());
            return;
        }

        public new void ShowHelp()
        {
            Console.Write($"{Title} ({Version}) - ");

            Console.WriteLine("Génère une classe Models/NomEntite.cs");
            Console.WriteLine();
            Console.WriteLine("Utilisation : tada model [options] [arguments]");
            Console.WriteLine();
            Console.WriteLine("Options :");
            Console.WriteLine("  -h|--help                  Affiche l'aide de la ligne de commande");
            Console.WriteLine("  -p|--project <PROJET>      Nom du projet pour définir le namespace");
            Console.WriteLine("                             (obtenu depuis PROJET.csproj sinon)");
            Console.WriteLine();
            Console.WriteLine("Arguments : Nom de l'entité suivi de la description de ses propriétés (sous la forme Nom:{+}Type)");
            Console.WriteLine();
            Console.WriteLine("Exemples :");
            Console.WriteLine("  tada model Genre Genre_ID:int Title:+string(30) --project MvcMovie");
            Console.WriteLine("  tada model Genre Genre_ID:int Title:+string(30)");
            Console.WriteLine();
            Console.WriteLine("Exemple de code généré :");
            Console.WriteLine("  Movie_ID:int               [Key]       // première propriété avec un nom terminé par \"_ID\"");
            Console.WriteLine("                             public int Movie_ID { get; set; }");
            Console.WriteLine("  Title:+string(60)          [Required]  // le type est précédé par \"+\"");
            Console.WriteLine("                             [StringLength(60)]");
            Console.WriteLine("                             public string Title { get; set; }");
            Console.WriteLine("  ReleaseDate:DateTime       public DateTime ReleaseDate { get; set; }");
            Console.WriteLine("  Genre:Genre                public int Genre_ID { get; set; }");
            Console.WriteLine("                             [ForeignKey(\"Genre_ID\")]");
            Console.WriteLine("                             public virtual Genre Genre { get; set; }");
            Console.WriteLine("  Price:decimal(18,2)        [Column(TypeName = \"decimal(18, 2)\")] ");
            Console.WriteLine("                             public decimal Price { get; set; }");
            Console.WriteLine("  Rating:+RatingEnum         [Required]");
            Console.WriteLine("                             public RatingEnum Rating { get; set; }");
            Console.WriteLine("  Directors:Director[]       public ICollection<Director> Directors { get; set; }");
        }

        public new string ToCode()
        {
            var columns = "";
            foreach (var c in Columns)
            {
                columns += c.ToCode(SPACES);
            }
            columns = columns.Substring(0, columns.Length - Environment.NewLine.Length);
            columns = columns.Substring(0, columns.Length - Environment.NewLine.Length);
            var code = $@"using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace {Project}.Models
{{
  public class {Name}
  {{
{columns}
  }}
}}";

            return code;
        }
    }

    public class ModelColumn
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsRequired { get; set; }
        public bool IsKey { get; set; }
        public int Size { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }
        public bool IsForeignKey { get; set; }

        public string ToCode(string SPACES)
        {
            Type = GetType(Type, Name);
            if (Type == Name) IsForeignKey = true;

            var code = "";

            var attributs = "";
            if (IsKey) attributs += ", Key";
            if (IsRequired) attributs += ", Required";
            if (Size > 0) attributs += ", StringLength(" + Size + ")";
            if (Scale > 0)
                attributs += ", Column(TypeName = \"" + Type + "(" + Precision + ", " + Scale + ")\")";
            else if (Precision > 0)
                attributs += ", Column(TypeName = \"" + Type + "(" + Precision + ")\")";
            if (attributs != "")
                code += SPACES + "[" + attributs.Substring(2) + "]" + Environment.NewLine;

            if (IsForeignKey)
            {
                code += SPACES + $"[ForeignKey(\"{Name}_ID\")]" + Environment.NewLine;
                code += SPACES + $"public virtual {Type} {Name} {{ get; set; }}" + Environment.NewLine;
                code += SPACES + $"public int {Name}_ID {{ get; set; }}" + Environment.NewLine;
            }
            else
            {
                code += SPACES + $"public {Type} {Name} {{ get; set; }}" + Environment.NewLine;
            }

            return code + Environment.NewLine;
        }

        public string GetType(string type, string name)
        {
            if (type == "") return name;
            if (type.ToLower() == "enum") return name + "Enum";

            var Types = new[] { "string", "int", "int16", "int32", "int64", "bool", "float", "double", "DateTime", "decimal" };
            foreach (var t in Types)
            {
                if (type.ToLower() == t.ToLower()) return t;
            }

            return type;
        }
    }
}
