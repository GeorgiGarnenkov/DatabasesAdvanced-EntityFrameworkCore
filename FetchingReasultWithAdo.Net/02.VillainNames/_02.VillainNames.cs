using System;
using System.Data.SqlClient;
using InitialSetup;

namespace _02.VillainNames
{
    public class StartUp
    {
        public static void Main()
        {
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();

                string villainsInfo =
                    "select v.Name, count(mv.MinionId) as MinionsCount from Villains as v join MinionsVillains as mv on mv.VillainId = v.Id group by v.Name having count(mv.MinionId) >= 3 order by MinionsCount desc";

                using (SqlCommand command = new SqlCommand(villainsInfo, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader[0]} -> {reader[1]}");
                        }
                    }
                }

                connection.Close();
            }
        }
    }
}
