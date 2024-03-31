using BookPricesApp.Core.Utils;
using BookPricesApp.Domain;
using System.Data.SQLite;

namespace BookPricesApp.Repo.Migrations;
public class MigrationStartup
{
    public Option InitDB()
    {
        try
        {
            string filePath = Config.DBFilePath;

            if (!File.Exists(filePath))
            {
                SQLiteConnection.CreateFile(filePath);
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
            return new Option { Ex = ex };
        }
        return new();
    }
}
