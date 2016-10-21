using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tech_Tatva_16__Windows_10_.Classes
{
    public class DatabaseHelperClass
    {
        SQLiteConnection dbConn;

        //Create Tabble 
        public async Task<bool> onCreate(string DB_PATH)
        {
            try
            {
                if (!CheckFileExists(DB_PATH).Result)
                {
                    using (dbConn = new SQLiteConnection(DB_PATH))
                    {
                        dbConn.CreateTable<EventClass>();
                    }
                }
                return true;
            }
            catch
            {
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
        public EventClass ReadEventByName(String name)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                try
                {
                    var existingconact = dbConn.Query<EventClass>("select * from EventClass where Name = '" + name + "'").FirstOrDefault();
                    return existingconact;
                }
                catch
                { return null; }
            }
        }

        // Retrieve the specific Event from the database. 
        public EventClass ReadEventById(int id)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingconact = dbConn.Query<EventClass>("select * from EventClass where id = " + id).FirstOrDefault();
                return existingconact;
            }
        }

        public List<EventClass> SearchEvents(String name)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingconact = dbConn.Query<EventClass>("select * from EventClass where Name like '" + name + "%'");
                return existingconact;
            }
        }

        // Retrieve the all contact list from the database. 
        public List<EventClass> ReadEvents()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<EventClass> myCollection = dbConn.Table<EventClass>().ToList<EventClass>();
                return myCollection;
            }
        }

        //Update existing conatct 
        public void UpdateEvent(EventClass eve)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingconact = dbConn.Query<EventClass>("select * from EventClass where id =" + eve.id).FirstOrDefault();
                if (existingconact != null)
                {
                    existingconact.Name = eve.Name;

                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Update(existingconact);
                    });
                }
            }
        }
        // Insert the new contact in the EventClass table. 
        public void Insert(EventClass neweve)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.Insert(neweve);
                });
            }
        }

        public void Insert(List<EventClass> neweve)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    foreach(EventClass eve in neweve)
                       dbConn.Insert(eve);
                });
            }
        }

        //Delete specific contact 
        public void DeleteEvent(int Id)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingconact = dbConn.Query<EventClass>("select * from Events where id =" + Id).FirstOrDefault();
                if (existingconact != null)
                {
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Delete(existingconact);
                    });
                }
            }
        }
        //Delete all contactlist or delete EventClass table 
        public void DeleteAllEvents()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                //dbConn.RunInTransaction(() => 
                //   { 
                dbConn.DropTable<EventClass>();
                dbConn.CreateTable<EventClass>();
                dbConn.Dispose();
                dbConn.Close();
                //}); 
            }
        }
    }
}

