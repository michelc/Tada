# Tada

Quelques outils pour générer des fichiers pour la partie "M" d'une application ASP.NET Core MVC.

Exemples pour MvcMovie :

* tada model Movie Movie_ID:int Title:+string(60) ReleaseDate:DateTime Genre:Genre Price:decimal(18,2) Rating:+RatingEnum Directors:Director[]
* tada model Genre Genre_ID:int Title:+string(30) Movies:Movie[]
* tada model Director Director_ID:int Name:+string(60) Movies:Movie[]
* tada enum RatingEnum NA G PG PG_13 R NC_17
* tada context Genre Director Movie

A faire :

* Générer les fichiers (pour l'instant il n'y a que l'affichage)
* Tester (utilisation perso => ne pas chercher les cas tordus)
* Gérer vraiment le n° de version => comment ?
* Transformer en .NET Core global tool (dotnet tada ...) ?


## tada --help

```
Tada Command-Line Tools (0.0.1)

Utilisation : tada [options] [commande] [arguments]

Options :
  -h|--help                  Affiche l'aide de la ligne de commande
  -p|--project <PROJET>      Nom du projet pour définir le namespace
  --version                  Affiche la version utilisée

Commandes :
  model                      Génère une classe Models/NomEntite.cs
  enum                       Génère une classe Models/NomEnum.cs
  context                    Génère une classe Models/ProjetContext.cs

Pour plus d'informations sur une commande, lancer 'tada [commande] --help'.
```


## tada model --help

```
Tada Command-Line Tools (0.0.1) - Génère une classe Models/NomEntite.cs

Utilisation : tada model [options] [arguments]

Options :
  -h|--help                  Affiche l'aide de la ligne de commande
  -p|--project <PROJET>      Nom du projet pour définir le namespace
                             (obtenu depuis PROJET.csproj sinon)

Arguments : Nom de l'entité suivi de la description de ses propriétés (sous la forme Nom:{+}Type)

Exemples :
  tada model Genre Genre_ID:int Title:+string(30) --project MvcMovie
  tada model Genre Genre_ID:int Title:+string(30)

Exemple de code généré :
  Movie_ID:int               [Key]       // première propriété avec un nom terminé par "_ID"
                             public int Movie_ID { get; set; }
  Title:+string(60)          [Required]  // le type est précédé par "+"
                             [StringLength(60)]
                             public string Title { get; set; }
  ReleaseDate:DateTime       public DateTime ReleaseDate { get; set; }
  Genre:Genre                public int Genre_ID { get; set; }
                             [ForeignKey("Genre_ID")]
                             public virtual Genre Genre { get; set; }
  Price:decimal(18,2)        [Column(TypeName = "decimal(18, 2)")]
                             public decimal Price { get; set; }
  Rating:+RatingEnum         [Required]
                             public RatingEnum Rating { get; set; }
  Directors:Director[]       public ICollection<Director> Directors { get; set; }
```


## tada enum --help

```
Tada Command-Line Tools (0.0.1) - Génère une classe Models/NomEnum.cs

Utilisation : tada enum [options] [arguments]

Options :
  -h|--help                  Affiche l'aide de la ligne de commande
  -p|--project <PROJET>      Nom du projet pour définir le namespace
                             (obtenu depuis PROJET.csproj sinon)

Arguments : Nom de l'enum suivi de ses valeurs

Exemples :
  tada enum Rating Enfants Interdit_moins_13_ans Interdit_mineurs Tout_public --project MvcMovie
  tada enum Rating Enfants Interdit_moins_13_ans Interdit_mineurs Tout_public
  tada enum -p MvcFacture Tva Normal Intermédiaire Réduit Particulier Zéro 
```


## tada context --help

```
Tada Command-Line Tools (0.0.1) - Génère une classe Models/ProjetContext.cs

Utilisation : tada context [options] [arguments]

Options :
  -h|--help                Affiche l'aide de la ligne de commande
  -p|--project <PROJET>    Nom du projet pour définir le namespace
                           (obtenu depuis PROJET.csproj sinon)

Arguments : Liste des entités du DbContext.

Exemples :
  tada context Movie Genre Director --project MvcMovie
  tada context Movie Genre Director
  tada -p MvcBlog context Blog Post Comment
  tada context -p MvcFacture Client Article Facture Ligne
```


## Compilation

Pour obtenir un .EXE qui est ensuite utilisable par `c:\Mvc\Tada\exe\tada.exe`, exécuter la commande :

```
c:\Mvc\Tada> dotnet publish --output "c:/mvc/tada/exe" --runtime win-x64 --configuration Release -p:PublishSingleFile=true --self-contained false
```
