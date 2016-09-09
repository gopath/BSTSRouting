namespace BSTSRouting
{
    public partial class Node
    {
        public string Key { get; private set; }

        /// Returns the TNode's Data.
        public object Data { get; set; }

        /// Returns an AdjacencyList of the TNode's neighbors.
        //public AdjacencyList Neighbors { get; private set; }

        /// Returns the TNode's Path Parent.
        public Node PathParent { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        /// Returns the Node's Latitude location.
        public double Latitude { get; set; }

        /// Returns the Node's Longitude location.
        public double Longitude { get; set; }

    }
        
}