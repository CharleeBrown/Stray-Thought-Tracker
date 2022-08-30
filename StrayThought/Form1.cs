using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Data;
using System.Text;
using System.IO;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.IO;
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
              //  conns.connected(data);
                ListViewItem info = new ListViewItem(DateTime.Now.ToShortDateString());
                
                info.SubItems.Add(data);
                listView1.Items.Add(info);
                textBox1.Clear();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Update ups = new Update();

            var data = ups.PullThoughts();
            
           /* foreach(var item in data)
            {
                MessageBox.Show(item.ToString());
            }*/
           
            for(var i = 0; i< data.Count; i++)
            {
                ListViewItem adds = new ListViewItem(data["date"])
                listView1.Items.Add(data["date"]);
                listView1.Items.Add(data["thought"]);
            }
            listView1.Items.Add(data);
        }
    }

    public class Conn
    {
        // Connects to the database and inserts the thoughts to the database.
        public string connected(string thoughts) {
            DateTime current = DateTime.Now;
            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://lee:Gamez2232@cluster0.guc9f.mongodb.net/?retryWrites=true&w=majority");
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
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