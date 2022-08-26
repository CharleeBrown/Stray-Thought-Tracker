using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Data;
using System.Text;
using System.IO;
namespace StrayThought
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Conn conns = new Conn();

            if(e.KeyChar == (char)Keys.Enter)
            {
                conns.connected(textBox1.Text);
                listBox1.Items.Add(textBox1.Text);
                textBox1.Clear();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Update ups = new Update();
            foreach(var item in ups.PullThoughts())
            {
                listBox1.Items.Add(item);
            }
        }
    }

    public class Conn
    {
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
            List<string> holdList = new List<string>();
            DateTime current = DateTime.Now;
            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://lee:Gamez2232@cluster0.guc9f.mongodb.net/?retryWrites=true&w=majority");
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            var db = client.GetDatabase("misc");
            var collect = db.GetCollection<BsonDocument>("bday");
            BsonDocument query = new BsonDocument();
            query.Add("date", current.ToShortDateString());
            var results = collect.Find(new BsonDocument()).FirstOrDefault();
            if (results.Any()) {
                MessageBox.Show(results.ToString());
                holdList.Add(results["thought"].ToString());
            }
           
            return holdList;
        }
            
           // BsonDocument addObj = new BsonDocument().Add("date", current.ToShortDateString());
            // add connection to configuration

        
        
    }
}