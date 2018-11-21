using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Configuration;

namespace Ado_net
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 29; i < 50; i++)
            {
                string URI = $"https://swapi.co/api/people/{i}/";
                using (WebClient webClient = new WebClient())
                {
                    Stream stream = webClient.OpenRead(URI);
                    StreamReader reader = new StreamReader(stream);
                    string request = reader.ReadToEnd();
                    Person obj = JsonConvert.DeserializeObject<Person>(request);
                    Console.WriteLine(obj.BirthYear);
                    using (var connection = new SqlConnection())
                    {
                        connection.ConnectionString = ConfigurationManager.ConnectionStrings["StarWarsConnectionString"].ConnectionString;
                        connection.Open();
                        SqlCommand command = new SqlCommand();
                        command.Connection = connection;
                        command.CommandText = $"insert into People values ('{obj.Name}','{obj.Height}','{obj.Gender}')";
                        command.ExecuteNonQuery();
                        //SqlCommand selectCommand = new SqlCommand();
                        //selectCommand.Connection = connection;
                        //selectCommand.CommandText = "select * from people";
                        //SqlDataReader dataReader = selectCommand.ExecuteReader();
                        //while (dataReader.NextResult())
                        //{
                        //    Person receivedPerson = new Person()
                        //    {
                        //        Name = dataReader["name"].ToString(),
                        //        Height = dataReader["height"].ToString()
                        //    };
                        //}
                    }
                }
                System.Threading.Thread.Sleep(80);
            }
            
        }
    }
}
