using Hangfire_Demo.Models;
using System.Data;

namespace Hangfire_Demo.Services
{
    public interface Iservice
    {
        void SendEmail();
        void InsertRecords(product product);

        DataTable SyncData();

        List<product> GetAllRecords();
    }
}
