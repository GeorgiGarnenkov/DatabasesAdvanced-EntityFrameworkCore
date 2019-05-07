using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using InitialSetup;

namespace _06.PrintAllMinionNames
{
    public class Program
    {
        public static void Main()
        {
            List<string> entryMinions = new List<string>();
            List<string> arrangedMinions = new List<string>();

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT Name FROM Minions", connection);

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
                        entryMinions.Add((string)reader["Name"]);
                    }
                }

                connection.Close();
            }

            while (entryMinions.Count > 0)
            {
                arrangedMinions.Add(entryMinions[0]);
                entryMinions.RemoveAt(0);

                if (entryMinions.Count > 0)
                {
                    arrangedMinions.Add(entryMinions[entryMinions.Count - 1]);
                    entryMinions.RemoveAt(entryMinions.Count - 1);
                }
            }

            arrangedMinions.ForEach(m => Console.WriteLine(m));
        }
    }
}
