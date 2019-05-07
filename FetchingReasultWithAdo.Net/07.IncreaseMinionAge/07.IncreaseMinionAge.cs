using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using InitialSetup;

namespace _07.IncreaseMinionAge
{
    public class StartUp
    {
        public static void Main()
        {
            int[] minionsIds = Console.ReadLine()
                .Split()
                .Select(int.Parse)
                .ToArray();
            
            List<int> minionIds = new List<int>();
            List<string> minionaNames = new List<string>();
            List<int> minionAges = new List<int>();

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();

                SqlCommand command =
                    new SqlCommand($"SELECT * FROM Minions WHERE Id IN ({string.Join(", ", minionsIds)})", connection);
                SqlDataReader reader = command.ExecuteReader();

                using (reader)
                {
                    if (!reader.HasRows)
                    {
                        reader.Close();
                        connection.Close();
                        return;
                    }

                    while (reader.Read())
                    {
                        minionIds.Add((int) reader["Id"]);
                        minionaNames.Add((string) reader["Name"]);
                        minionAges.Add((int) reader["Age"]);
                    }
                }

                for (int i = 0; i < minionIds.Count; i++)
                {
                    int id = minionIds[i];
                    string name = string.Join(" ",
                        minionaNames[i].Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries).ToList()
                            .Select(n => n = char.ToUpper(n.First()) + n.Substring(1).ToLower()).ToArray());
                    int age = minionAges[i] + 1;

                    command = new SqlCommand("UPDATE Minions SET Name = @name, Age = @age WHERE Id = @Id", connection);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@age", age);
                    command.Parameters.AddWithValue("@id", id);

                    command.ExecuteNonQuery();
                }

                command = new SqlCommand($"SELECT * FROM Minions", connection);

                reader = command.ExecuteReader();

                using (reader)
                {
                    if (!reader.HasRows)
                    {
                        reader.Close();
                        connection.Close();
                        return;
                    }

                    while (reader.Read())
                    {
                        Console.WriteLine($"{(int) reader["Id"]} {(string) reader["Name"]} {(int) reader["Age"]}");
                    }
                }

                connection.Close();
            }
        }
    }
}
