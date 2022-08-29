using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Data;
using System.Text;
using System.IO;
using System.Linq;
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
                var cursor = from doc in conns.connected(data)
                             where data != null
                             select doc;

                foreach(var item in cursor)
                {
                    info.SubItems.Add(item);
                }
                info.SubItems.Add(data);
                listView1.Items.Add(info);
                textBox1.Clear();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Update ups = new Update();

            var tests = ups.PullThoughts();
            IEnumerable<ListViewItem> lv = listView1.Items.Cast<ListViewItem>();
            listView1.Items.Add(tests);
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
        public ListViewItem PullThoughts()
        {
            
            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://lee:Gamez2232@cluster0.guc9f.mongodb.net/?retryWrites=true&w=majority");

            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);

            var db = client.GetDatabase("misc");

            var collect = db.GetCollection<BsonDocument>("bday");

            BsonDocument query = new BsonDocument();

            query.Add("", "*");

            var results = collect.Find(new BsonDocument()).FirstOrDefault();

            ListViewItem thoughts = new ListViewItem();
        
                thoughts.SubItems.Add(results["thought"].ToString());
                thoughts.SubItems.Add(results["date"].ToString());
                
                //holdList.Add(results["thought"].ToString());
       
           
            return thoughts;
        }
            
           // BsonDocument addObj = new BsonDocument().Add("date", current.ToShortDateString());
            // add connection to configuration

        
        
    }
}