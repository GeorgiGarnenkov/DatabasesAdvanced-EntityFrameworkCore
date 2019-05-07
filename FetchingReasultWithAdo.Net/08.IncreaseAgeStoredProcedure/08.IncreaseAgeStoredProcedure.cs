using System;
using System.Data.SqlClient;
using InitialSetup;

namespace _08.IncreaseAgeStoredProcedure
{
    public class StartUp
    {
        public static void Main()
        {
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("EXEC usp_GetOlder @minionId", connection);
                command.Parameters.AddWithValue("@minionId", id);
                command.ExecuteNonQuery();

                command = new SqlCommand("SELECT * FROM Minions WHERE Id = @minionId", connection);
                command.Parameters.AddWithValue("@minionId", id);
                var reader = command.ExecuteReader();

                using (reader)
                {
                    reader.Read();

                    Console.WriteLine($"{(string)reader["Name"]} - {(int)reader["Age"]} years old");
                }

                connection.Close();
            }
        }
    }
}
