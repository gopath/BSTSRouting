using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace BSTSRouting
{
    class DBConnect
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public DBConnect()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "localhost";
            database = "lintasbandung";
            uid = "root";
            password = "";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);
        }


        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server");
                        break;
                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        //Insert statement
        public void Insert()
        {
            string query = "INSERT INTO tableinfo (name, age) VALUES('asd fgh', '33')";

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Update statement
        public void Update()
        {
            string query = "UPDATE tableinfo SET name='asdf', age='22' WHERE name='asd fgh'";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Delete statement
        public void Delete()
        {
            string query = "DELETE FROM tableinfo WHERE name='John Smith'";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }
        
        //Select statement
        public List<string>[] Select()
        {
            string query = "SELECT * FROM tableinfo";

            //Create a list to store the result
            List<string>[] list = new List<string>[3];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["id"] + "");
                    list[1].Add(dataReader["name"] + "");
                    list[2].Add(dataReader["age"] + "");
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        //Count statement
        public int Count()
        {
            string query = "SELECT Count(*) FROM tableinfo";
            int Count = -1;

            //Open Connection
            if (this.OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                this.CloseConnection();

                return Count;
            }
            else
            {
                return Count;
            }
        }

        public List<string>[] getNodeCoordinate(string data)
        {
            string query = "SELECT a.way_id, a.node_id, c.latitude, c.longitude, b.v as way_name FROM way_nodes a, way_tags b, nodes c WHERE a.way_id = b.way_id and a.node_id = c.node_id and b.k = 'name' and b.v LIKE '" + data + "%' group by b.way_id";

            //Create a list to store the result
            List<string>[] list = new List<string>[3];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["way_id"] + "");
                    list[1].Add(dataReader["node_id"] + "");
                    list[2].Add(dataReader["way_name"] + "");
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        public List<WayNodeModel> getWayNodeData()
        {
            string query = "SELECT a.way_id, a.node_id, c.latitude, c.longitude, b.v as way_name FROM way_nodes a, way_tags b, nodes c WHERE a.way_id = b.way_id and a.node_id = c.node_id and b.k = 'name' group by b.v";
            //Create a list to store the result
            List<WayNodeModel> list = new List<WayNodeModel>();
            
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    WayNodeModel wayModel = new WayNodeModel();
                    wayModel.setWayId(dataReader["way_id"].ToString());
                    wayModel.setNodeId(dataReader["node_id"].ToString());
                    wayModel.setWayName(dataReader["way_name"].ToString());
                    list.Add(wayModel);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        public WayNodeModel getWayNodeDataAttribute(String name)
        {
            string query = "SELECT a.way_id, a.node_id, c.latitude, c.longitude, b.v as way_name FROM way_nodes a, way_tags b, nodes c WHERE a.way_id = b.way_id and a.node_id = c.node_id and b.k = 'name' and b.v = '" + name + "' group by b.way_id limit 1";
            //Create a list to store the result
            WayNodeModel wayModel = new WayNodeModel();
                
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    wayModel.setWayId(dataReader["way_id"].ToString());
                    wayModel.setNodeId(dataReader["node_id"].ToString());
                    wayModel.setLatitude(Convert.ToDouble(dataReader["latitude"].ToString()));
                    wayModel.setLongitude(Convert.ToDouble(dataReader["longitude"].ToString()));
                    wayModel.setWayName(dataReader["way_name"].ToString());
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return way to be displayed
                return wayModel;
            }
            else
            {
                return wayModel;
            }
        }

        public List<NodeModel> getNodes()
        {
            string query = "SELECT a.node_id, a.latitude, a.longitude FROM nodes a";
            //Create a list to store the result
            List<NodeModel> list = new List<NodeModel>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    NodeModel nodes = new NodeModel();
                    nodes.setNodeId(dataReader["node_id"].ToString());
                    nodes.setLatitude(Convert.ToDouble(dataReader["latitude"].ToString()));
                    nodes.setLongitude(Convert.ToDouble(dataReader["longitude"].ToString()));
                    list.Add(nodes);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        public NodeModel getNodesFromNodeId(string nodeId)
        {
            string query = "SELECT a.node_id, a.latitude, a.longitude FROM nodes a WHERE a.node_id = '" + nodeId + "'";
            //Create a list to store the result
            NodeModel node = new NodeModel();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    node.setNodeId(dataReader["node_id"].ToString());
                    node.setLatitude(Convert.ToDouble(dataReader["latitude"].ToString()));
                    node.setLongitude(Convert.ToDouble(dataReader["longitude"].ToString()));                    
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return node to be displayed
                return node;
            }
            else
            {
                return node;
            }
        }

        public List<EdgeModel> getEdges()
        {
            string query = "SELECT a.node_from, a.node_to, a.distance FROM node_to_node a";
            //Create a list to store the result
            List<EdgeModel> list = new List<EdgeModel>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    EdgeModel edge = new EdgeModel();
                    edge.setNodeFrom(dataReader["node_from"].ToString());
                    edge.setNodeTo(dataReader["node_to"].ToString());
                    edge.setDistance(Convert.ToDouble(dataReader["distance"].ToString()));
                    list.Add(edge);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        public List<WayNodeModel> getPrimaryWayEdgeForScreenshots()
        {
            string query = "SELECT a.way_id, a.node_id, c.latitude, c.longitude, b.v as way_name FROM way_nodes a, way_tags b, nodes c WHERE a.way_id = b.way_id and a.node_id = c.node_id and b.k = 'name' GROUP BY b.v";
            //Create a list to store the result
            List<WayNodeModel> mainWays = new List<WayNodeModel>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    WayNodeModel model = new WayNodeModel();
                    model.setWayId(dataReader["way_id"].ToString());
                    model.setNodeId(dataReader["node_id"].ToString());
                    model.setLatitude(Convert.ToDouble(dataReader["latitude"].ToString()));
                    model.setLongitude(Convert.ToDouble(dataReader["longitude"].ToString()));
                    model.setWayName(dataReader["way_name"].ToString());
                    mainWays.Add(model);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return mainWays;
            }
            else
            {
                return mainWays;
            }
        }
       
    }
}
