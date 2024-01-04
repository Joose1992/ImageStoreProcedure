using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;

namespace ImageStoreProcedure
{
    public class ReadImageFromDb
    {
        string connectionString = "Server=lenovo_Jose;Database=PracticeDb;Trusted_Connection=True;";
        private int imageIDToRetrieve;

        public ReadImageFromDb(int imageID)
        {
            imageIDToRetrieve = imageID;
        }
        public void DownloadAndSaveImage()
        {
            byte[] imageData = RetrieveImageFromDatabase(connectionString, imageIDToRetrieve);
            if(imageData != null)
            {
                Image retrieveImage = ByteArrayToImage(imageData);
                string path = string.Format(@"C:\Users\josem\Desktop\ImagesFromDatabase\image{0}.jpg", imageIDToRetrieve);
                retrieveImage.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                Console.WriteLine("image retrieved and saved!");
            }
            else
            {
                Console.WriteLine("Error retrieving image from database.");
            }
        }

        private byte[] RetrieveImageFromDatabase(string connectionString, int imageID)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT IMAGEDATA FROM IMAGES WHERE IMAGEID = @IMAGEID";
                    using(SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        command.Parameters.Add("@IMAGEID", System.Data.SqlDbType.Int).Value = imageID;

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return (byte[])reader["ImageData"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving image from database: {ex.Message}");
                return null;
            }
            return null;
        }
        private Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }
    }
}
