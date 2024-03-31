using BookPricesApp.Core.Utils;
using BookPricesApp.Domain;

namespace BookPricesApp.Repo;
public static class Global
{
    public static object LockObject = new object();
    public static string CS =>  $"Data Source={Config.DBFilePath};Version=3;";
}
