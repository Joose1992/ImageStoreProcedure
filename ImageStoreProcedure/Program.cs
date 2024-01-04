using System.Data;
using System.Data.SqlClient;
using System.Net.Security;

namespace ImageStoreProcedure
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Users\josem\Pictures\Screenshots\Screenshot 2023-09-28 121500.png";
            StoreImage store = new StoreImage(path);
            store.UploadImage();
        }

        

        
    }
}
