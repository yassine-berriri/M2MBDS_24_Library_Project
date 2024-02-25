# MBDS-24-Library-Project

## Descirption 

Ce projet représente une étape importante dans notre parcours d'apprentissage et de maîtrise des technologies .NET et C#. En tant que réalisation concrète dans le cadre de notre cours sur .NET, ce projet nous a permis de mettre en pratique et d'approfondir notre compréhension de plusieurs concepts clés tels que les API REST, les ORM, WPF, et ASP.NET.

## API REST

### Fonctionnalités

#### - Récupération d'un livre avec son contenu : 

Permet de récupérer les détails d'un livre spécifique, y compris son contenu.

##### Endpoint: /GetBook/{id}

#### - Listage des genres disponibles : 
Fournit une liste de tous les genres disponibles dans la librairie.

##### Endpoint: /GetGenres

#### - Listage des livres (sans le contenu) : 
Permet de récupérer une liste de livres avec pagination et filtrage par genre ou une liste de genres.

#####  Endpoint: /GetBooks?genre={genre}&limit={limit}&offset={offset}

La réponse inclut un header Pagination indiquant l'index de début et de fin des livres retournés, ainsi que le nombre total de livres sélectionnés.
Exemple de header : Pagination: 10-25/536

### Documentation Swagger

La documentation complète de l'API est disponible sur Swagger à l'adresse suivante :  http://localhost:5001/swagger/index.html

## Application Windows (WPF)

### Fonctionnalités

#### - Affichage et listage des livres : 

L'application permet de lister les N premiers livres (limite définie par l'utilisateur).

#### - Détails des livres : 

Affiche les détails d'un livre sélectionné.

#### - Listage des genres : 

Permet de lister tous les genres disponibles.

#### - Affichage des livres par genre : 

Affiche les N premiers livres d'un genre ou liste de genres spécifiques.

#### - Pagination : 

Gère la pagination des livres (par exemple, via des pages).

#### - Lecture de livre : 

Utilise l'API System.Speech.SpeechSynthesizer pour lire le contenu du livre.

#### - Contrôles de lecture : 

Gère la lecture, la pause et la reprise de la lecture.

#### - Lecture à partir de la sélection : 

Permet à l'utilisateur de lancer la lecture à partir d'un mot sélectionné via un clic droit.

## Administration
### Fonctionnalités

#### - Consulter la liste des livres :
Affiche la liste complète de tous les livres présents dans la bibliothèque.

#### - Ajouter des livres : 
Les utilisateurs peuvent ajouter de nouveaux livres à la bibliothèque en spécifiant les détails tels que le titre, le contenu, l'auteur, le genre, etc.

#### - Supprimer des livres : 
Les utilisateurs ont la possibilité de supprimer des livres de la bibliothèque.

#### - Modifier un livre existant :
Permet aux utilisateurs de modifier les détails d'un livre existant, y compris le remplacement du champ auteur par une liaison vers une classe Auteur.

#### - Consulter la liste des genres :
Affiche la liste de tous les genres disponibles dans la bibliothèque.

#### - Ajouter de nouveaux genres : 
Permet d'ajouter de nouveaux genres à la bibliothèque via une interface dédiée.

#### - Supprimer des genres : 
Les utilisateurs ont la possibilité de supprimer des genres de la bibliothèque.

#### - Remplacement du champ auteur :
Le champ auteur de type string dans la classe Livre est remplacé par une liaison vers une classe Auteur, permettant à un livre d'avoir plusieurs auteurs.

#### - Ajouter des auteurs : 
Les utilisateurs peuvent ajouter de nouveaux auteurs en spécifiant les détails tels que le Nom, la date de naissance, la biographie, etc.

#### - Supprimer des auteurs : 
Les utilisateurs ont la possibilité de supprimer des auteurs.

#### - Modifier un auteur existant :
Permet aux utilisateurs de modifier les détails d'un auteur existant.

#### - Filtres dans la liste des livres :
Affiche des filtres dans la liste des livres pour permettre aux utilisateurs de filtrer par auteurs et genres.

#### - Statistiques :
Une page spécifique (Dashboard) affiche des statistiques, notamment le nombre total de livres disponibles, le nombre de livres par auteur, le nombre de livres par genre ainsi que le nombre maximum, minimum, médian et moyen de mots dans un livre.

#### - Importation depuis l'OpenLibrary :
Les utilisateurs peuvent importer les détails d'un livre en utilisant l'API OpenLibrary (https://openlibrary.org/). Il suffit de fournir le contenu et l'ISBN pour récupérer automatiquement l'auteur, la description et enregistrer les informations en base de données.


#### Groupe

###### - Ibrahim KRIMI
###### - Badis BOUCHEFFA
###### - Yassine BERRIRI
###### - Marwane LARBI
