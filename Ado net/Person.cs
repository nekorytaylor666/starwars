using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Data.Common;


namespace Ado_net
{
    public class Person : IGetFromAPI<Person>, ISaveToDB<Person>
    {
        private string url = $"https://swapi.co/api/people/?page=1&format=json";
        public string Name { get; set; }
        public string Height { get; set; }
        public string Mass { get; set; }
        [JsonProperty(PropertyName = "hair_color")]
        public string HairColor { get; set; }
        [JsonProperty(PropertyName = "skin_color")]
        public string SkinColor { get; set; }
        [JsonProperty(PropertyName = "eye_color")]
        public string EyeColor { get; set; }
        [JsonProperty(PropertyName = "birth_year")]
        public string BirthYear { get; set; }
        public string Gender { get; set; }
        public string Homeworld { get; set; }
        public List<string> Films { get; set; }
        public List<string> Species { get; set; }
        public List<object> Vehicles { get; set; }
        public List<object> Starships { get; set; }
        public DateTime Created { get; set; }
        public DateTime Edited { get; set; }
        public string Url { get; set; }

        public Person GetObject(int id)
        {
            string URI = $"https://swapi.co/api/people/{id}";
            Person obj;
            using (WebClient webClient = new WebClient())
            {
                Stream stream = webClient.OpenRead(URI);
                StreamReader reader = new StreamReader(stream);
                string request = reader.ReadToEnd();
                obj = JsonConvert.DeserializeObject<Person>(request);
            }
            return obj;
        }

        public List<Person> GetListObjects(string url, out string urlNext)
        {

            List<Person> people = new List<Person>();
            using (WebClient client = new WebClient())
            {
                JObject json = JObject.Parse(client.DownloadString(url));
                people.AddRange(JsonConvert.DeserializeObject<List<Person>>(json.GetValue("results").ToString()));
                urlNext = json.GetValue("next").ToString();
                Console.WriteLine("Download people...");
                Console.WriteLine("Complete...");
            }
            return people;
        }

        public void SaveList(IGetFromAPI<Person> getFrom)
        {
            string urlNext;
            using (var connection = new SqlConnection())
            {
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["StarWarsConnectionString"].ConnectionString;
                connection.Open();
                do
                {
                    List<Person> people = getFrom.GetListObjects(url, out urlNext);
                    List<DbCommand> commands = new List<DbCommand>();
                    foreach (var person in people)
                    {

                        var command = new SqlCommand();
                        command.Connection = connection;
                        command.CommandText = $"insert into people values (@name,@height,@mass,@hairColor,@skinColor,@eyeColor,@gender,@homeWorld,@url)";

                        command.Parameters.AddRange(new SqlParameter[]
                        {
                    new SqlParameter("@name",person.Name),
                    new SqlParameter("@height",person.Height),
                    new SqlParameter("@mass",person.Mass),
                    new SqlParameter("@hairColor",person.HairColor),
                    new SqlParameter("@skinColor",person.SkinColor),
                    new SqlParameter("@eyeColor",person.EyeColor),
                    new SqlParameter("@gender",person.Gender),
                    new SqlParameter("@homeworld",person.Homeworld),
                    new SqlParameter("@url",person.Url),

                        });
                        commands.Add(command);
                        Console.WriteLine("Creating the command...");
                    }
                    ExecuteInTransaction(connection, commands);
                    url = urlNext;
                } while (urlNext!= string.Empty);
                
            }
        }

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

        public void TransactionFail(Exception exception, DbTransaction transaction)
        {
            Console.WriteLine(exception.Message);
            transaction.Rollback();
        }
    }
}
