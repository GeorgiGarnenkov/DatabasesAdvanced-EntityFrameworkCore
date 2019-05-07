using System;
using System.Data.SqlClient;
using InitialSetup;

namespace _03.AddMinion
{
    public class Program
    {
        public static void Main()
        {
            var firstInput = Console.ReadLine().Split(new []{':', ' '}, StringSplitOptions.RemoveEmptyEntries);
            var secondInput = Console.ReadLine().Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            
            var minionName = firstInput[1];
            var minionAge = int.Parse(firstInput[2]);
            var minionTown = firstInput[3];

            var villainName = secondInput[1];

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();

                int townId = GetTownId(minionTown, connection);

                int villainId = GetVillainId(villainName, connection);

                int minionId = InsertMinionAndGetId(minionName, minionAge, townId, connection);

                AssignMinionToVillain(villainId, minionId, connection);

                Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");

                connection.Close();
            }

        }

        private static void AssignMinionToVillain(int villainId, int minionId, SqlConnection connection)
        {
            string insertMinionVillain =
                "insert into MinionsVillains (MinionId, VillainId) values (@minionId, @villainId)";

            using (SqlCommand command = new SqlCommand(insertMinionVillain, connection))
            {
                command.Parameters.AddWithValue("@minionId", minionId);
                command.Parameters.AddWithValue("@villainId", villainId);
                command.ExecuteNonQuery();
            }
        }

        private static int InsertMinionAndGetId(string minionName, int minionAge, int townId, SqlConnection connection)
        {
            string insertMinion = "insert into Minions (Name, Age, TownId) values (@minionName, @minionAge, @townId)";

            using (SqlCommand command = new SqlCommand(insertMinion, connection))
            {
                command.Parameters.AddWithValue("@minionName", minionName);
                command.Parameters.AddWithValue("@minionAge", minionAge);
                command.Parameters.AddWithValue("@townId", townId);
                command.ExecuteNonQuery();
            }

            string minionId = "select Id from Minions where Name = @name";

            using (SqlCommand command = new SqlCommand(minionId, connection))
            {
                command.Parameters.AddWithValue("@name", minionName);

                return (int) command.ExecuteScalar();
            }
        }

        private static int GetVillainId(string villainName, SqlConnection connection)
        {
            string townSql = "select Id from Villains where Name = @Name";

            using (SqlCommand command = new SqlCommand(townSql, connection))
            {
                command.Parameters.AddWithValue("@Name", villainName);

                if (command.ExecuteScalar() == null)
                {
                    InsertIntoVillains(villainName, connection);
                }

                return (int)command.ExecuteScalar();

            }
        }

        private static void InsertIntoVillains(string villainName, SqlConnection connection)
        {
            string insertVillain = "insert into Villains (Name, EvilnessFactorId) values (@villainName, 4)";

            using (SqlCommand command = new SqlCommand(insertVillain, connection))
            {
                command.Parameters.AddWithValue("@villainName", villainName);
                command.ExecuteNonQuery();
                Console.WriteLine($"Villain {villainName} was added to the database.");
            }
        }

        private static int GetTownId(string minionTown, SqlConnection connection)
        {
            string townSql = "select Id from Towns where Name = @Name";

            using (SqlCommand command = new SqlCommand(townSql, connection))
            {
                command.Parameters.AddWithValue("@Name", minionTown);

                if (command.ExecuteScalar() == null)
                {
                    InsertIntoTowns(minionTown, connection);
                }

                return (int) command.ExecuteScalar();

            }
        }

        private static void InsertIntoTowns(string minionTown, SqlConnection connection)
        {
            string insertTown = "insert into Towns (Name) values (@minionTown)";

            using (SqlCommand command = new SqlCommand(insertTown, connection))
            {
                command.Parameters.AddWithValue("@minionTown", minionTown);
                command.ExecuteNonQuery();

                Console.WriteLine($"Town {minionTown} was added to the database.");
            }
        }
    }
}
