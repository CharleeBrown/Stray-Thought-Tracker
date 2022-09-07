using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Data;
using System.Text;
using System.IO;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.IO;
using System.Configuration;
using MongoDB.Driver.Core;

namespace StrayThought
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            listView1.Columns.Add("Date");
            listView1.Columns.Add("Thought");
            listView1.View = View.Details;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Conn conns = new Conn();

            if(e.KeyChar == (char)Keys.Enter)
            {
                var data = textBox1.Text;
                conns.Connector(data);
                ListViewItem info = new ListViewItem(DateTime.Now.ToShortDateString());
                
                info.SubItems.Add(data);
                listView1.Items.Add(info);
                textBox1.Clear();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Update updateList = new Update();

            var updates = updateList.PullThoughts();
            DataTable dt = new DataTable();
            
            ListViewItem mainItem = new ListViewItem(updates[0]);
            IDataAdapter adapter = new IDataAdapter(updates);
            mainItem.SubItems.Add(updates[1]);
            listView1.Items.Add(mainItem);


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ListViewItem listitem = new ListViewItem(dr["pk_Location_ID"].ToString());
                listitem.SubItems.Add(dr["var_Location_Name"].ToString());
                listitem.SubItems.Add(dr["fk_int_District_ID"].ToString());
                listitem.SubItems.Add(dr["fk_int_Company_ID"].ToString());
                listView1.Items.Add(listitem);
            }

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

    public class Conn
    {
   
        public string Connector(string thoughts) {
            //Grab current date
            DateTime current = DateTime.Now;
            var x = ConfigurationManager.ConnectionStrings["mongos"];
            var settings = MongoClientSettings.FromConnectionString(x.ConnectionString);
            //settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            var db = client.GetDatabase("misc");
            var collect = db.GetCollection<BsonDocument>("bday");
            BsonDocument addObj = new BsonDocument().Add("date", current.ToShortDateString());
            // add connection to configuration
            addObj.Add("thought", thoughts);

            collect.InsertOne(addObj);
            string yo = db.DatabaseNamespace.ToString();
            return yo;
        }
    }

    public class Update
    {
        public List<string> PullThoughts()
        {
            List<string> transfer = new List<string>();
            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://lee:Gamez2232@cluster0.guc9f.mongodb.net/?retryWrites=true&w=majority");

            
            var client = new MongoClient(settings);

            var db = client.GetDatabase("misc");

            var collect = db.GetCollection<BsonDocument>("bday");
            var cursor = collect.Find(_ => true).ToList();

            var filter = Builders<BsonDocument>.Filter.Lt("date", "8/31/2022");
            var projection = Builders<BsonDocument>.Projection.Include("date").Include("thought").Exclude("_id");
            var result = collect.Find(filter).Project(projection).ToList();

              
                        foreach(BsonDocument doc in result)
                        {
                        List<string> thoughts = new List<string>();
                            thoughts.Add(doc["date"].ToString());
                            thoughts.Add(doc["thought"].ToString());
                            transfer = thoughts; 

                        }
            if (result.Count > 0)
            {
                foreach (var item in result)
                {

                    List<string> go = new List<string>();
                    go.Add(item["date"].ToString());
                    go.Add(item["thought"].ToString());
                    transfer = go;
                }
            }
            else
            {
                MessageBox.Show("No Records");
            }

            return transfer;

        }
        
    }

    class DataObj
    {
        [BsonElement("date")]
        public string date { get; set; }

        [BsonElement("thought")]
        public string thought { get; set; }
    }
}