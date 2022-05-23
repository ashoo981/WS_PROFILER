using System;
using System.Xml;
using System.Data.SqlClient;
using Npgsql;

namespace SfaCSharp
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Hello World!");
            XmlDocument doc = new XmlDocument();
            string URLx = "G:\\Repos\\SfaCsharpApp\\SfaCSharp\\SfaCSharp\\WORKSPACE.XML";
            doc.Load(URLx);
            XmlElement root = doc.DocumentElement;

            XmlNodeList elemList = root.GetElementsByTagName("TableName");
            XmlNodeList elemList1 = root.GetElementsByTagName("Name");
            XmlNodeList elemList2 = root.GetElementsByTagName("Version");
            //XmlNodeList elemList3 = root.GetElementsByTagName("srs");
            XmlNodeList elemList4 = root.GetElementsByTagName("WKText");
            string test_date = DateTime.Now.ToString("MM/dd/yyyy");
            Console.WriteLine(elemList.Count);

            string conn_tbl_check = null;
            conn_tbl_check = "Host=localhost;Username=postgres;Password=q1w2e3r4t5y6;Port=5432;Database=nilbis20210528";
            string sql1 = null;
            sql1 = "select count(*) from information_schema.tables where table_name='test_result_ws' ";
            NpgsqlCommand command1;

            NpgsqlConnection con1 = new NpgsqlConnection(conn_tbl_check);
            command1 = new NpgsqlCommand(sql1, con1);
            con1.Open();
            int count = Convert.ToInt32(command1.ExecuteScalar());
            Console.WriteLine(count);
            if (count == 0)
            {
                string con_tta = null;
                con_tta = "Host=localhost;Username=postgres;Password=q1w2e3r4t5y6;Port=5432;Database=nilbis20210528";
                string sqlcreatetable = "create table test_result_ws (objectid serial not null,ws_name character varying(255),test_date character varying(255),table_name character varying(255),count_time character varying(255),dbname character varying(255),table_row_count integer,ncversion character varying (255), wktext character varying (2048));";
                NpgsqlConnection conttc = new NpgsqlConnection(con_tta);
                NpgsqlCommand command2 = new NpgsqlCommand(sqlcreatetable, conttc);
                conttc.Open();
                command2.ExecuteNonQuery();
                Console.WriteLine("tablo oluşturuldu");





            }


            else
            {


                for (int i = 0; i < elemList.Count; i++)
                {
                    string tbl;
                    string wsName;
                    string ncVersion;
                    string coordinateSystem;
                    string wktext;
                    string dbname = "nilbis20210528";
                    tbl = elemList[i].InnerText.ToString();
                    wsName = elemList1[0].InnerText.ToString();
                    ncVersion = elemList2[0].InnerText.ToString();
                    //coordinateSystem = elemList3[0].Attributes.ToString();
                    wktext = elemList4[0].InnerText.ToString();

                    Console.WriteLine(tbl);
                    string connetionString = null;
                    connetionString = "Host=localhost;Username=postgres;Password=q1w2e3r4t5y6;Port=5432;Database=nilbis20210528";
                    NpgsqlConnection conn = new NpgsqlConnection(connetionString);
                    string sql = null;
                    string sqlinsert = null;
                    NpgsqlCommand command;
                    NpgsqlCommand commandinsert;

                    sql = "select count(*) from " + tbl + ';';
                    sqlinsert = "insert into test_result_ws (ws_name,test_date,  table_name, count_time, dbname, table_row_count,ncversion,wktext) values (@tbl1,@tbl2,@tbl3,@tbl4,@tbl5,@tbl6,@tbl7,@tbl9)";
                    Console.WriteLine(sql);
                   
                    command = new NpgsqlCommand(sql, conn);
                    commandinsert = new NpgsqlCommand(sqlinsert, conn);






                    try
                    {
                        conn.Open();
                        DateTime zaman1 = DateTime.Now;
                        int counttable = Convert.ToInt32(command.ExecuteScalar());
                        DateTime zaman2 = DateTime.Now;
                        string count_time = Convert.ToString((zaman2 - zaman1).TotalMilliseconds);
                        Console.WriteLine((zaman2 - zaman1).ToString());
                        commandinsert.Parameters.AddWithValue("@tbl1", wsName);
                        commandinsert.Parameters.AddWithValue("@tbl2", test_date);
                        commandinsert.Parameters.AddWithValue("@tbl3", tbl);
                        commandinsert.Parameters.AddWithValue("@tbl4", count_time);
                        commandinsert.Parameters.AddWithValue("@tbl5", dbname);
                        commandinsert.Parameters.AddWithValue("@tbl6", counttable);
                        commandinsert.Parameters.AddWithValue("@tbl7", ncVersion);
                        //commandinsert.Parameters.AddWithValue("@tbl8", coordinateSystem);
                        commandinsert.Parameters.AddWithValue("@tbl9", wktext);

                        Console.WriteLine(sqlinsert);

                        commandinsert.ExecuteNonQuery();










                    }
                    catch (Exception e)
                    {

                        Console.WriteLine(e.Message);
                    }
                    conn.Close();

                }
            }





        }


    }
}
