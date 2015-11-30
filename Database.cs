using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.Collections.ObjectModel;

namespace Kaart2
{
    //This class for perform all database CRUD operations 

    public class Andmed
    {
        //The Id property is marked as the Primary Key
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string Nimi { get; set; }
        public double Ver { get; set; }
        public double Hor { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }

        public Andmed()
        {
            //empty constructor
        }
        public Andmed(string nimi, double ver, double hor, string text)
        {
            Nimi = nimi;
            Ver = ver;
            Hor = hor;
            Text = text;
            CreationDate = DateTime.Now;
        }
    }
    public class DatabaseHelperClass
    {
        SQLiteConnection dbConn;

        //Create Tabble 
        public bool OnCreate(string db_path)
        {
            try
            {
                if (!CheckFileExists(db_path).Result)
                {
                    using (dbConn = new SQLiteConnection(MainPage.DB_PATH))
                    {
                        dbConn.CreateTable<Andmed>();
                    }
                }
                return true;
            }
            catch(Exception Ex)
            {
                System.Diagnostics.Debug.WriteLine(Ex.Message);
                return false;
            }
        }
        private async Task<bool> CheckFileExists(string fileName)
        {
            try
            {
                var store = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Retrieve the specific contact from the database. 
        public static Andmed ReadContact(int id)
        {
            using (var dbConn = new SQLiteConnection(MainPage.DB_PATH))
            {
                var existingconact = dbConn.Query<Andmed>("select * from Andmed where Id =" + id).FirstOrDefault();
                return existingconact;
            }
        }
        // Retrieve the all contact list from the database. 
        public static ObservableCollection<Andmed> ReadContacts()
        {
            using (var dbConn = new SQLiteConnection(MainPage.DB_PATH))
            {
                List<Andmed> myCollection = dbConn.Table<Andmed>().ToList<Andmed>();
                ObservableCollection<Andmed> ContactsList = new ObservableCollection<Andmed>(myCollection);
                return ContactsList;
            }
        }

        //Update existing conatct 
        public static void UpdateContact(Andmed andmed)
        {
            using (var dbConn = new SQLiteConnection(MainPage.DB_PATH))
            {
                var existingandmed = dbConn.Query<Andmed>("select * from Contacts where Id =" + andmed.Id).FirstOrDefault();
                if (existingandmed != null)
                {
                    existingandmed.Nimi = andmed.Nimi;
                    existingandmed.Ver = andmed.Ver;
                    existingandmed.Ver = andmed.Hor;
                    existingandmed.Text = andmed.Text;
                    existingandmed.CreationDate = andmed.CreationDate;
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Update(existingandmed);
                    });
                }
            }
        }
        // Insert the new contact in the Contacts table. 
        public static void Insert(Andmed andmed)
        {
            using (var dbConn = new SQLiteConnection(MainPage.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.Insert(andmed);
                });
            }
        }

        //Delete specific contact 
        public static void DeleteContact(int Id)
        {
            using (var dbConn = new SQLiteConnection(MainPage.DB_PATH))
            {
                
                var existingconact = dbConn.Query<Andmed>("select * from Andmed where Id =" + Id).FirstOrDefault();
                if (existingconact != null)
                {
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Delete(existingconact);
                    });
                }
            }
        }
        //Delete all contactlist or delete Contacts table 
        public static void DeleteAllContact()
        {
            using (var dbConn = new SQLiteConnection(MainPage.DB_PATH))
            {
              //  dbConn.RunInTransaction(() => 
              //     { 
                dbConn.DropTable<Andmed>();
                dbConn.CreateTable<Andmed>();
                //dbConn.Close();
              //  }); 
            }
            
        }
    }
}
