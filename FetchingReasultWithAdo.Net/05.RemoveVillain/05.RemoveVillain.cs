using System;
using System.Data.SqlClient;
using InitialSetup;

namespace _05.RemoveVillain
{
    public class StartUp
    {
        public static void Main()
        {
            var inputId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction();

                int villainId = GetVillainId(inputId, connection, transaction);

                if (villainId == 0)
                {
                    Console.WriteLine("No such villain was found.");
                }
                else
                {
                    try
                    {
                        int countOfReleasedMinions = GetReleasedMinionsCount(villainId, connection, transaction);
                        string villainName = GetVillainName(villainId, connection, transaction);
                        DeleteVillain(villainId, connection, transaction);
                        Console.WriteLine($"{villainName} was deleted.");
                        Console.WriteLine($"{countOfReleasedMinions} minions were released.");
                    }
                    catch (SqlException e)
                    {
                        transaction.Rollback();
                    }
                    
                }

                connection.Close();
            }
        }

        private static void DeleteVillain(int villainId, SqlConnection connection, SqlTransaction transaction)
        {
            string deleteVillain = $"delete from Villains where Id = {villainId}";

            using (SqlCommand command = new SqlCommand(deleteVillain, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private static string GetVillainName(int villainId, SqlConnection connection, SqlTransaction transaction)
        {
            string villainNameSql = $"select Name from Villains where Id = {villainId}";

            using (SqlCommand command = new SqlCommand(villainNameSql, connection))
            {
                return (string)command.ExecuteScalar();
            }
        }

        private static int GetReleasedMinionsCount(int villainId, SqlConnection connection, SqlTransaction transaction)
        {
            string releaseMinions = $"delete FROM MinionsVillains where VillainId = {villainId}";

            using (SqlCommand command = new SqlCommand(releaseMinions, connection))
            {
                return command.ExecuteNonQuery();
            }
        }

        private static int GetVillainId(int inputId, SqlConnection connection, SqlTransaction transaction)
        {
            string inputIdSql = "select Id from Villains where Id = @Id";

            using (SqlCommand command = new SqlCommand(inputIdSql, connection))
            {
                command.Parameters.AddWithValue("@Id", inputId);

                if (command.ExecuteScalar() == null)
                {
                    return 0;
                }

                return (int)command.ExecuteScalar();
            }
        }
    }
}
