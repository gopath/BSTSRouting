using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BSTSRouting
{
    public class Graph
    {
        #region Private Member Variables
        private NodeList nodes;
        #endregion

        #region Constructor
        public Graph()
        {
            this.nodes = new NodeList();
        }

        // Creates a new graph class instance based on a list of nodes.
        public Graph(NodeList nodes)
        {
            this.nodes = nodes;
        }
        #endregion

        #region Public Methods
        // Clears out all of the nodes in the graph.
        public virtual void Clear()
        {
            nodes.Clear();
        }

        #region Adding TNode Methods
        // Adds a new node to the graph.
        public virtual Node AddNode(string key, object data)
        {
            // Make sure the key is unique
            if (!nodes.ContainsKey(key))
            {
                Node n = new Node(key, data);
                nodes.Add(n);
                return n;
            }
            else
                throw new ArgumentException("There already exists a node in the graph with key " + key);
        }

        public virtual Node AddNode(string key, object data, int x, int y)
        {
            // Make sure the key is unique
            if (!nodes.ContainsKey(key))
            {
                Node n = new Node(key, data, x, y);
                nodes.Add(n);
                return n;
            }
            else
            {
                throw new ArgumentException("There already exists a node in the graph with key " + key);
            }
        }

        public virtual Node AddNode(string key, object data, double latitude, double longitude)
        {
            // Make sure the key is unique
            if (!nodes.ContainsKey(key))
            {
                Node n = new Node(key, data, latitude, longitude);
                nodes.Add(n);

                return n;
            }
            else
            {
                throw new ArgumentException("There already exists a node in the graph with key " + key);
            }
        }

        // Adds a new node to the graph.
        public virtual void AddNode(Node n)
        {
            // Make sure this node is unique
            if (!nodes.ContainsKey(n.Key))
                nodes.Add(n);
            else
                throw new ArgumentException("There already exists a node in the graph with key " + n.Key);
        }
        #endregion

        #region Adding Edge Methods
        // Adds a directed edge from one node to another.
        public virtual void AddDirectedEdge(string uKey, string vKey)
        {
            AddDirectedEdge(uKey, vKey, 0);
        }

        // Adds a directed, weighted edge from one node to another.
        public virtual void AddDirectedEdge(string uKey, string vKey, int cost)
        {
            // get references to uKey and vKey
            if (nodes.ContainsKey(uKey) && nodes.ContainsKey(vKey))
                AddDirectedEdge(nodes[uKey], nodes[vKey], cost);
            else
                throw new ArgumentException("One or both of the nodes supplied were not members of the graph.");
        }

        // Adds a directed edge from one node to another.
        public virtual void AddDirectedEdge(Node u, Node v)
        {
            AddDirectedEdge(u, v, 0);
        }

        // Adds a directed, weighted edge from one node to another.
        public virtual void AddDirectedEdge(Node u, Node v, int cost)
        {
            // Make sure u and v are Nodes in this graph
            if (nodes.ContainsKey(u.Key) && nodes.ContainsKey(v.Key))
                // add an edge from u -> v
                u.AddDirected(v, cost);
            else
                // one or both of the nodes were not found in the graph
                throw new ArgumentException("One or both of the nodes supplied were not members of the graph.");
        }

        // Adds an undirected edge from one node to another.
        public virtual void AddUndirectedEdge(string uKey, string vKey)
        {
            AddUndirectedEdge(uKey, vKey, 0);
        }

        // Adds an undirected, weighted edge from one node to another.
        public virtual void AddUndirectedEdge(string uKey, string vKey, int cost)
        {
            // get references to uKey and vKey
            if (nodes.ContainsKey(uKey) && nodes.ContainsKey(vKey))
                AddUndirectedEdge(nodes[uKey], nodes[vKey], cost);
            else
                throw new ArgumentException("One or both of the nodes supplied were not members of the graph.");
        }

        // Adds an undirected edge from one node to another.
        public virtual void AddUndirectedEdge(Node u, Node v)
        {
            AddUndirectedEdge(u, v, 0);
        }

        // Adds an undirected, weighted edge from one node to another.
        public virtual void AddUndirectedEdge(Node u, Node v, int cost)
        {
            // Make sure u and v are Nodes in this graph
            if (nodes.ContainsKey(u.Key) && nodes.ContainsKey(v.Key))
            {
                // Add an edge from u -> v and from v -> u
                u.AddDirected(v, cost);
                v.AddDirected(u, cost);
            }
            else
                // one or both of the nodes were not found in the graph
                throw new ArgumentException("One or both of the nodes supplied were not members of the graph.");
        }

        // Adds an undirected, weighted edge from one node to another.
        public virtual void AddUndirectedEdge(Node u, Node v, double cost)
        {
            // Make sure u and v are Nodes in this graph
            if (nodes.ContainsKey(u.Key) && nodes.ContainsKey(v.Key))
            {
                // Add an edge from u -> v and from v -> u
                u.AddDirected(v, cost);
                v.AddDirected(u, cost);
            }
            else
                // one or both of the nodes were not found in the graph
                throw new ArgumentException("One or both of the nodes supplied were not members of the graph.");
        }

        #endregion

        #region Contains Methods
        // Determines if a node exists within the graph.
        public virtual bool Contains(Node n)
        {
            return Contains(n.Key);
        }

        // Determines if a node exists within the graph.
        public virtual bool Contains(string key)
        {
            return nodes.ContainsKey(key);
        }
        #endregion
        #endregion

        #region Public Properties
        // Returns the number of nodes in the graph.
        public virtual int Count
        {
            get
            {
                return nodes.Count;
            }
        }

        // Returns a reference to the set of nodes in the graph.
        public virtual NodeList Nodes
        {
            get
            {
                return this.nodes;
            }
        }
        #endregion
    }
}