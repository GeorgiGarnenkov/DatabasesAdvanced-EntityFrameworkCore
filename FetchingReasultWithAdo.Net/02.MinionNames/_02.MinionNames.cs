using System;
using System.Data.SqlClient;
using InitialSetup;

namespace _02.MinionNames
{
    public class StartUp
    {
        public static void Main()
        {
            var villainId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();

                string villainName = GetVillainName(villainId, connection);

                if (villainName == null)
                {
                    Console.WriteLine($"No villain with ID {villainId} exists in the database.");
                }
                else
                {
                    Console.WriteLine($"Villain: {villainName}");
                    PrintNames(villainId, connection);
                }

                connection.Close();
            }
        }

        private static void PrintNames(int villainId, SqlConnection connection)
        {
            string minionsNameAge =
                "select m.Name, m.Age from Minions as m join MinionsVillains as mv on mv.MinionId = m.Id where mv.VillainId = @id";

            using (SqlCommand command = new SqlCommand(minionsNameAge, connection))
            {
                command.Parameters.AddWithValue("@id", villainId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("(no minions)");
                    }
                    else
                    {
                        var counter = 1;
                        while (reader.Read())
                        {
                            Console.WriteLine($"{counter++}. {reader[0]} {reader[1]}");
                        }
                    }
                    
                }
            }
        }

        private static string GetVillainName(int villainId, SqlConnection connection)
        {
            string nameSql = "select Name from villains where Id = @id";

            using (SqlCommand command = new SqlCommand(nameSql, connection))
            {
                command.Parameters.AddWithValue("@id", villainId);
                return (string)command.ExecuteScalar();
            }
        }
    }
}
