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
using System.Data;
using System.Data.Common;

namespace Ado_net
{
    class Program
    {
        static void Main(string[] args)
        {         
            IGetFromAPI<Person> people = new Person();
            ISaveToDB<Person> DbSaverPeople = new Person
            {
                Url = $"https://swapi.co/api/people/?page=1&format=json"
            };
            IGetFromAPI<Planet> planets = new Planet();
            ISaveToDB<Planet> DbSaverPlanets = new Planet()
            {
                Url = $"https://swapi.co/api/planets/?page=1&format=json"
            };
            DbSaverPlanets.SaveList(planets);
            DbSaverPeople.SaveList(people);
        }


    }
}
    

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
