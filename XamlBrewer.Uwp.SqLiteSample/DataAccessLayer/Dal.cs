namespace XamlBrewer.Uwp.SqLiteSample.DataAccessLayer
{
    using SQLite.Net;
    using SQLite.Net.Platform.WinRT;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Windows.ApplicationModel;
    using Windows.Storage;
    using XamlBrewer.Uwp.SqLiteSample.Models;

    internal static class Dal
    {
        private static string dbPath = string.Empty;
        private static string DbPath
        {
            get
            {
                if (string.IsNullOrEmpty(dbPath))
                {
                    dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Storage.sqlite");
                }

                return dbPath;
            }
        }

        private static SQLiteConnection DbConnection
        {
            get
            {
                return new SQLiteConnection(new SQLitePlatformWinRT(), DbPath);
            }
        }

        public static async Task CreateDatabase()
        {
            // Create a new connection
            using (var db = DbConnection)
            {
                // Activate Tracing
                db.TraceListener = new DebugTraceListener();

                // Create the table if it does not exist
                var c = db.CreateTable<Person>();
                var info = db.GetMapping(typeof(Person));

                // All info from http://www.jamesbondwiki.com/page/James+Bond+Actors

                Person person = new Person();
                person.Id = 1;
                person.Name = "Sean Connery";
                person.DayOfBirth = new DateTime(1930, 8, 25);
                person.Description = "Sir Thomas Sean Connery was born in Edinburgh, Scotland on August 25, 1930. His father was a milkman and they lived in a very poor area. He was once a bodybuilder and was chosen to play Bond in Dr.No after Cary Grant was approached, and would only commit to one film, not a series";
                StorageFile file = await StorageFile.GetFileFromPathAsync(Path.Combine(Package.Current.InstalledLocation.Path, @"Assets\Pictures\SeanConnery.jpg"));
                person.Picture = await file.AsByteArray();
                var i = db.InsertOrReplace(person);

                person = new Person();
                person.Id = 2;
                person.Name = "George Lazenby";
                person.DayOfBirth = new DateTime(1939, 9, 5);
                person.Description = "George Robert Lazenby was born in Goulburn, New South Wales, Australia, on 5 September 1939. He is the second official actor to portray 007, following Sean Connery, and the youngest actor in the role to this day, at age 29. It was Lazenby's first serious acting role (he was a model before).";
                file = await StorageFile.GetFileFromPathAsync(Path.Combine(Package.Current.InstalledLocation.Path, @"Assets\Pictures\GeorgeLazenby.jpg"));
                person.Picture = await file.AsByteArray();
                i = db.InsertOrReplace(person);

                person = new Person();
                person.Id = 3;
                person.Name = "Roger Moore";
                person.DayOfBirth = new DateTime(1927, 10, 14);
                person.Description = "Sir Roger George Moore was born in Stockwell, London, England, on 14 October 1927. At the age of 87, he is currently the oldest living Bond actor. At the age of 58 in A View To A Kill, he is the oldest actor to have appeared in a Bond film.";
                file = await StorageFile.GetFileFromPathAsync(Path.Combine(Package.Current.InstalledLocation.Path, @"Assets\Pictures\RogerMoore.jpg"));
                person.Picture = await file.AsByteArray();
                i = db.InsertOrReplace(person);

                person = new Person();
                person.Id = 4;
                person.Name = "Timothy Dalton";
                person.DayOfBirth = new DateTime(1944, 3, 21);
                person.Description = "Timothy Peter Dalton was born in Colwyn Bay, Wales on March 21, 1944 (though some sources say 1946), the eldest of five children to a successful advertising agent and his wife. He is Welsh with a dash of Irish and Italian.";
                file = await StorageFile.GetFileFromPathAsync(Path.Combine(Package.Current.InstalledLocation.Path, @"Assets\Pictures\TimothyDalton.jpg"));
                person.Picture = await file.AsByteArray();
                i = db.InsertOrReplace(person);

                person = new Person();
                person.Id = 5;
                person.Name = "Pierce Brosnan";
                person.DayOfBirth = new DateTime(1953, 5, 16);
                person.Description = "Pierce Brendan Brosnan was born in Navan, County Meath, Ireland on May 16, 1953. He moved to London with his family in 1964. He became a commercial artist after leaving school, but was introduced to acting by a co-worker who was in a theatre group in the evenings.";
                file = await StorageFile.GetFileFromPathAsync(Path.Combine(Package.Current.InstalledLocation.Path, @"Assets\Pictures\PierceBrosnan.jpg"));
                person.Picture = await file.AsByteArray();
                i = db.InsertOrReplace(person);

                person = new Person();
                person.Id = 6;
                person.Name = "Daniel Craig";
                person.DayOfBirth = new DateTime(1986, 5, 2);
                person.Description = "Daniel Wroughton Craig was born 2 March 1968 in Chester, Cheshire, England. He was brought up in Liverpool and moved to London when he was 16 to join the National Youth Theatre, later landing a spot at the Guildhall School of Music and Drama.";
                file = await StorageFile.GetFileFromPathAsync(Path.Combine(Package.Current.InstalledLocation.Path, @"Assets\Pictures\DanielCraig.jpg"));
                person.Picture = await file.AsByteArray();
                i = db.InsertOrReplace(person);
            }
        }

        public static void DeletePerson(Person person)
        {
            // Create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), dbPath))
            {
                // Activate Tracing
                db.TraceListener = new DebugTraceListener();

                // Object model:
                //db.Delete(person);

                // SQL Syntax:
                db.Execute("DELETE FROM Person WHERE Id = ?", person.Id);
            }
        }

        public static List<Person> GetAllPersons()
        {
            List<Person> models;

            // Create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), dbPath))
            {
                // Activate Tracing
                db.TraceListener = new DebugTraceListener();

                models = (from p in db.Table<Person>()
                          select p).ToList();
            }

            return models;
        }

        public static Person GetPersonById(int Id)
        {
            // Create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), dbPath))
            {
                // Activate Tracing
                db.TraceListener = new DebugTraceListener();
                Person m = (from p in db.Table<Person>()
                            where p.Id == Id
                            select p).FirstOrDefault();
                return m;
            }
        }

        public static void SavePerson(Person person)
        {
            // Create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), dbPath))
            {
                // Activate Tracing
                db.TraceListener = new DebugTraceListener();

                if (person.Id == 0)
                {
                    // New
                    db.Insert(person);
                }
                else
                {
                    // Update
                    db.Update(person);
                }
            }
        }
    }
}