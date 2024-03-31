using BookPricesApp.Core.Utils;
using BookPricesApp.Domain;
using System.Data.SQLite;

namespace BookPricesApp.Repo.Migrations;
public class MigrationStartup
{
    public Result<int, Exception> InitDB()
    {
        try
        {

            if (!Directory.Exists(Config.DBFolderPath))
            {
                Directory.CreateDirectory(Config.DBFolderPath);
            }

            if (!File.Exists(Config.DBFilePath))
            {
                SQLiteConnection.CreateFile(Config.DBFilePath);
            }

            var tables = new Tables();

            lock (Global.LockObject)
            {
                using var con = new SQLiteConnection(Global.CS);
                con.Open();

                foreach (var query in tables.CreateTableArray)
                {
                    using var cmd = new SQLiteCommand(query, con);
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
        }
        catch (Exception ex)
        {
            return ex ;
        }
        return 0;
    }
}
