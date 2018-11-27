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
            ISaveToDB<Person> DbSaver = new Person();
            DbSaver.SaveList(people);
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
