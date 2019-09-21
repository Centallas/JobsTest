using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace CCG
{
    class Program
    {
        static void Main(string[] args)
        {
            // Table to store the query results
            DataTable table = new DataTable();


            string conectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CCG;Integrated Security=True";
            // Creates a SQL connection
            using (var connection = new SqlConnection(conectionString))
            {
                connection.Open();

                // Creates a SQL command
                using (var command = new SqlCommand("SELECT * FROM viewcoduni", connection))
                {
                    // Loads the query results into the table
                    table.Load(command.ExecuteReader());
                }

                connection.Close();
                //connection.Dispose();


                Console.WriteLine("\n-----------------------------------------------------");
                Console.WriteLine("\t" + "NIT " + "\t" + "CODIGO UNICO");
                Console.WriteLine("-------------------------------------------------------");

                foreach (DataRow o in table.Select("", "CODIGOUNICO DESC"))
                {
                    Console.WriteLine("\t" + o["NIT"] + "\t" + "\t" + o["CODIGOUNICO"]);
                }

                string val;
                Console.WriteLine("\n-----------------------------------------------------");
                Console.WriteLine("ESCRIBA CODIGO UNICO PARA CONSULTAR LOS DATOS:       ");
                val = Console.ReadLine();

                DataTable table1 = new DataTable();
                connection.Open();
                using (var command1 = new SqlCommand("SELECT * FROM viewcoduni WHERE CODIGOUNICO = " + val, connection))
                {
                    table1.Load(command1.ExecuteReader());
                }

                connection.Close();
                Console.WriteLine("\n-------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("\t" + "RAZONSOCIAL " + "\t" + "NOMBRECOMERCIO" + "\t" + "NIT" + "\t" + "CODIGOUNICO" + "\t" + "FECHAREGISTRO");
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------");

                foreach (DataRow o in table1.Select())
                {
                    Console.WriteLine("\t" + o["RAZONSOCIAL"] + "\t" + o["NOMBRECOMERCIO"] + "\t" + o["NIT"] + "\t" + o["CODIGOUNICO"] + "\t" + o["FECHAREGISTRO"]);

                }

                string valnit;
                Console.WriteLine("\n-----------------------------------------------------");
                Console.WriteLine("CONSULTAR POR VALOR NIT:       ");
                valnit = Console.ReadLine();

                DataTable table2 = new DataTable();
                connection.Open();
                using (var command2 = new SqlCommand("SELECT * FROM viewnit WHERE NIT =   " + valnit, connection))
                {
                    table2.Load(command2.ExecuteReader());
                }
                connection.Close();
                Console.WriteLine("\n--------------------------------------------------------------------------------------------");
                Console.WriteLine("\t" + "RAZONSOCIAL " + "\t" + "NOMBRECOMERCIO" + "\t" + "NIT" + "\t" + "CODIGOUNICO" + "\t");
                Console.WriteLine("----------------------------------------------------------------------------------------------");

                foreach (DataRow o in table2.Select())
                {
                    Console.WriteLine("\t" + o["RAZONSOCIAL"] + "\t" + o["NOMBRECOMERCIO"] + "\t" + o["NIT"] + "\t" + o["CODIGOUNICO"]);

                }

                string nitID;
                Console.WriteLine("\n-----------------------------------------------------");
                Console.WriteLine("CONSULTAR POR NIT XML:  ");
                nitID = Console.ReadLine();
                Console.WriteLine("-----------------------------------------------------");

                //DataTable table3 = new DataTable();



                connection.Open();

                //string query = @"SELECT * FROM [dbo].[fnGetEmployeeInfo](@empID);";
                string query = @"SELECT [dbo].[fnGetNitData](@nitID)";
                using (var command3 = new SqlCommand(query, connection))
                {
                    try
                    {
                        //parameter value will be set from command line
                        SqlParameter param1 = new SqlParameter();
                        param1.ParameterName = "@nitID";
                        param1.SqlDbType = SqlDbType.Int;
                        param1.Value = nitID;
                        command3.Parameters.Add(param1);

                        //var reader = command3.ExecuteReader();
                        //https://www.c-sharpcorner.com/UploadFile/mahesh/display-an-xml-file-on-console/
                        //var reader = command3.ExecuteReader();


                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(command3.ExecuteXmlReader());

                        if (xmlDoc.HasChildNodes)
                            xmlDoc.Save(Console.Out);
                        else
                            Console.WriteLine("NO EXISTE NINGÚN REGISTRO CON EL NÚMERO NIT {0}",nitID);
                        //using (XmlReader reader = command3.ExecuteXmlReader())
                        //{

                        //    //int x = 1;
                        //    while (reader.Read())
                        //    {
                        //        var s = reader.ReadOuterXml();
                        //        //foreach(var item in s)
                        //        Console.WriteLine(s + "/n");
                        //        // do something with s
                        //    }

                        //}

                    }
                    catch (Exception)
                    {

                        throw;
                    }

                    connection.Close();
                    connection.Dispose();
                }



                Console.ReadLine();



            }

            // You can add a breakpoint here to examine the contents of the table
        }


        //foreach (DataRow row in table.Rows)
        //{
        //    Console.WriteLine("_________");
        //    foreach (var item in row.ItemArray)
        //    {
        //        Console.WriteLine(item);
        //    }
        //}
    }
}
