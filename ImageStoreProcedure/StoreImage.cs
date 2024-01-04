using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace ImageStoreProcedure
{
    public class StoreImage
    {
        private readonly string connectionString = "Server=lenovo_Jose;Database=PracticeDb;Trusted_Connection=True;";
        private string imagePath;
        public StoreImage(string path)
        {
            imagePath = path;
        }

        public void UploadImage()
        {
            byte[] imageData = ReadImageFile(imagePath);
            if (imageData != null)
            {
                InsertImageIntoDatabase(connectionString, imageData);
                Console.WriteLine("Image inserted into database!");
            }
            else
            {
                Console.WriteLine("Error reading image file");
            }
        }
        private byte[] ReadImageFile(string imagePath)
        {
            try
            {
                using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        return br.ReadBytes((int)fs.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading image file: {ex.Message}");
                return null;
            }
        }
        private void InsertImageIntoDatabase(string connectionString, byte[] imageData)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO IMAGES (ImageData) VALUES (@ImageData)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                    {
                        cmd.Parameters.Add("@ImageData", SqlDbType.VarBinary, -1).Value = imageData;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting image into database: {ex.Message}");
            }
        }
    }
}
