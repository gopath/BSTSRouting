using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BSTSRouting
{    
    class AStar
    {
        private DBConnect connection;
        private List<NodeModel> listNodes;
        private List<EdgeModel> listEdge;

        public Path<Node> calcuateShortestPath(string nodeIdFrom, string nodeIdTo) {
            Path<Node> resultPath = null;
            Graph graph = new Graph();
            DistanceType distanceType = DistanceType.km;
            FillGraphWithNode(graph, distanceType);
            Node start = graph.Nodes[nodeIdFrom];
            Node destination = graph.Nodes[nodeIdTo];

            Console.WriteLine("Start : " + start.ToString());
            Console.WriteLine("End : " + destination.ToString());

            // distance between two neighbours.
            Func<Node, Node, double> distance = (node1, node2) =>
                                                node1.Neighbors.Cast<EdgeToNeighbor>().Single(
                                                    etn => etn.Neighbor.Key == node2.Key).Cost;

            // Heuristic function estimated distance between the last node on path and the destination node.
            Func<Node, double> haversineEstimation =
                n => Haversine.Distance(n, destination, DistanceType.km);

            Path<Node> shortestPath = FindPath(start, destination, distance, haversineEstimation);

            Console.WriteLine("\nThis is the shortest path based on the A* Search Algorithm:\n");

            // Prints the shortest path.
            foreach (Path<Node> path in shortestPath.Reverse())
            {
                if (path.PreviousSteps != null)
                {
                    Console.WriteLine(string.Format("From {0, -15}  to  {1, -15} -> Total cost = {2:#.###} {3}",
                                      path.PreviousSteps.LastStep.Key, path.LastStep.Key, path.TotalCost, distanceType));
                }
            }

            return resultPath;
        }

        private void FillGraphWithNode(Graph graph, DistanceType distanceType) {
            connection = new DBConnect();
            listNodes = connection.getNodes();
            for (int i = 0; i < listNodes.Count; i++)
            {
                Node node = new Node(listNodes[i].getNodeId(), null, listNodes[i].getLatitude(), listNodes[i].getLongitude());
                graph.AddNode(node);                
            }
            FillGraphWithEdge(graph);
        }

        private void FillGraphWithEdge(Graph graph) {
            connection = new DBConnect();
            listEdge = connection.getEdges();
            NodeModel nFrom = new NodeModel();
            NodeModel nTo = new NodeModel();
            for (int i = 0; i < listEdge.Count; i++)
            {
                nFrom = connection.getNodesFromNodeId(listEdge[i].getNodeFrom());
                nTo = connection.getNodesFromNodeId(listEdge[i].getNodeTo());
                Node nodeFrom = new Node(nFrom.getNodeId(), null, nFrom.getLatitude(), nFrom.getLongitude());
                Node nodeTo = new Node(nTo.getNodeId(), null, nTo.getLatitude(), nTo.getLongitude());
                graph.AddUndirectedEdge(nodeFrom, nodeTo, listEdge[i].getDistance());                
            }
        }

        static public Path<TNode> FindPath<TNode>(
            TNode start,
            TNode destination,
            Func<TNode, TNode, double> distance,
            Func<TNode, double> estimate) where TNode : IHasNeighbours<TNode>
        {
            var closed = new HashSet<TNode>();

            var queue = new PriorityQueue<double, Path<TNode>>();

            queue.Enqueue(0, new Path<TNode>(start));

            while (!queue.IsEmpty)
            {
                var path = queue.Dequeue();

                if (closed.Contains(path.LastStep))
                    continue;

                if (path.LastStep.Equals(destination))
                    return path;

                closed.Add(path.LastStep);

                foreach (TNode n in path.LastStep.Neighbours)
                {
                    double d = distance(path.LastStep, n);

                    var newPath = path.AddStep(n, d);

                    queue.Enqueue(newPath.TotalCost + estimate(n), newPath);
                }

            }

            return null;
        }
    }

    sealed partial class Node : IHasNeighbours<Node>
    {
        public IEnumerable<Node> Neighbours
        {
            get
            {
                List<Node> nodes = new List<Node>();

                foreach (EdgeToNeighbor etn in Neighbors)
                {
                    nodes.Add(etn.Neighbor);
                }

                return nodes;
            }
        }
    }
}
