using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

namespace Ado_net
{
    public class Planet:IGetFromAPI<Planet>,ISaveToDB<Planet>
    {
        public string Name { get; set; }
        [JsonProperty(PropertyName = "rotation_period")]
        public string RotationPeriod { get; set; }
        [JsonProperty(PropertyName = "orbital_period")]
        public string OrbitalPeriod { get; set; }
        public string Diameter { get; set; }
        public string Climate { get; set; }
        public string Gravity { get; set; }
        public string Terrain { get; set; }
        [JsonProperty(PropertyName = "surface_water")]
        public string SurfaceWater { get; set; }
        public string Population { get; set; }
        public List<string> Residents { get; set; }
        public List<string> Films { get; set; }
        public DateTime Created { get; set; }
        public DateTime Edited { get; set; }
        public string Url { get; set; }

        public bool ExecuteInTransaction(DbConnection connection, List<DbCommand> commands)
        {
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    foreach (var command in commands)
                    {
                        command.Transaction = transaction;
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    return true;
                }
                catch (SqlException exception)
                {
                    TransactionFail(exception, transaction);
                    return false;
                }
                catch (DbException exception)
                {
                    TransactionFail(exception, transaction);
                    return false;

                }
                catch (Exception exception)
                {
                    TransactionFail(exception, transaction);
                    return false;
                }
            }
        }

        public List<Planet> GetListObjects(string url, out string urlNext)
        {
            List<Planet> planets = new List<Planet>();
            using (WebClient client = new WebClient())
            {
                JObject json = JObject.Parse(client.DownloadString(url));
                planets.AddRange(JsonConvert.DeserializeObject<List<Planet>>(json.GetValue("results").ToString()));
                urlNext = json.GetValue("next").ToString();
                Console.WriteLine("Download planets...");
                Console.WriteLine("Complete...");
            }
            return planets;
        }

        public Planet GetObject(int id)
        {
            string URI = $"https://swapi.co/api/planets/{id}";
            Planet obj;
            using (WebClient webClient = new WebClient())
            {
                Stream stream = webClient.OpenRead(URI);
                StreamReader reader = new StreamReader(stream);
                string request = reader.ReadToEnd();
                obj = JsonConvert.DeserializeObject<Planet>(request);
            }
            return obj;
        }

        public void SaveList(IGetFromAPI<Planet> getFrom)
        {
            string urlNext;
            using (var connection = new SqlConnection())
            {

                connection.ConnectionString = ConfigurationManager.ConnectionStrings["StarWarsConnectionString"].ConnectionString;
                connection.Open();

                do
                {

                    List<Planet> planets = getFrom.GetListObjects(Url, out urlNext);
                    List<DbCommand> commands = new List<DbCommand>();

                    foreach (var planet in planets)
                    {
                        var command = new SqlCommand
                        {
                            Connection = connection,
                            CommandText = $"insert into planets(name,rotationPeriod,orbitalPeriod,diameter,climate,gravity,terrain,surfaceWater,populationPlanet) values(@name, @rotationPeriod,@orbitalPeriod,@diameter,@climate,@gravity, @terrain, @surfaceWater,@population)"
                        };

                        command.Parameters.AddRange(new SqlParameter[]
                        {
                            new SqlParameter("@name",planet.Name),
                            new SqlParameter("@rotationPeriod",planet.RotationPeriod),
                            new SqlParameter("@orbitalPeriod",planet.OrbitalPeriod),
                            new SqlParameter("@diameter",planet.Diameter),
                            new SqlParameter("@climate",planet.Climate),
                            new SqlParameter("@gravity",planet.Gravity),
                            new SqlParameter("@terrain",planet.Terrain),
                            new SqlParameter("@surfaceWater", planet.Terrain),
                            new SqlParameter("@population", planet.Population),
                            

                        });

                        commands.Add(command);

                        Console.WriteLine("Creating the command...");
                    }

                    Console.WriteLine("Creating transaction...");
                    ExecuteInTransaction(connection, commands);
                    Console.WriteLine("Commiting transaction...");

                    Url = urlNext;

                } while (urlNext != string.Empty);

            }
        }

        public void TransactionFail(Exception exception, DbTransaction transaction)
        {
            Console.WriteLine(exception.Message);
            transaction.Rollback();
        }
    }
}
