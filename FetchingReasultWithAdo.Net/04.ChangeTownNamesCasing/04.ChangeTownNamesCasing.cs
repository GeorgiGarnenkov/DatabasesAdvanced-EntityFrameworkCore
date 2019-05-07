using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using InitialSetup;

namespace _04.ChangeTownNamesCasing
{
    public class StartUp
    {
        public static void Main()
        {
            string countryName = Console.ReadLine();
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();

                int countryId = GetCountOfCountries(countryName, connection);

                if (countryId == 0)
                {
                    Console.WriteLine("No town names were affected.");
                }
                else
                {
                    int rowsCount = UpdateNames(countryId, connection);

                    Console.WriteLine($"{rowsCount} town names were affected.");

                    List<string> names = GetNames(countryId, connection);

                    Console.WriteLine($"[{String.Join(", ", names)}]");
                }

                connection.Close();
            }
        }

        private static List<string> GetNames(int countryId, SqlConnection connection)
        {
            List<string> listNames = new List<string>();

            string nameSql = "SELECT Name from Towns where CountryCode = @countryId";

            using (SqlCommand command = new SqlCommand(nameSql, connection))
            {
                command.Parameters.AddWithValue("@countryId", countryId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listNames.Add((string)reader[0]);
                    }
                }
            }

            return listNames;
        }

        private static int UpdateNames(int countryId, SqlConnection connection)
        {
            string updateStatement = "UPDATE Towns set Name = UPPER(Name) where CountryCode = @countryId";

            using (SqlCommand command = new SqlCommand(updateStatement, connection))
            {
                command.Parameters.AddWithValue("@countryId", countryId);

                return command.ExecuteNonQuery();
            }
        }

        private static int GetCountOfCountries(string countryName, SqlConnection connection)
        {
            string countryId =
                "SELECT TOP(1) c.Id from Towns as t join Countries as c on c.Id = t.CountryCode where c.Name = @name";
            using (SqlCommand command = new SqlCommand(countryId, connection))
            {
                command.Parameters.AddWithValue("@name", countryName);

                if (command.ExecuteScalar() == null)
                {
                    return 0;
                }

                return (int)command.ExecuteScalar();
            }
        }
    }
}
