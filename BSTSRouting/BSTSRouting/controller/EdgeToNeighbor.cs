namespace BSTSRouting
{
    public class EdgeToNeighbor
    {
        #region Public Properties

        public virtual double Cost { get; private set; }

        public virtual Node Neighbor { get; private set; }

        #endregion

        #region Constructors

        public EdgeToNeighbor(Node neighbor)
            : this(neighbor, 0)
        {

        }

        public EdgeToNeighbor(Node neighbor, double cost)
        {
            Cost = cost;
            Neighbor = neighbor;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return string.Format("Neighbor = {0} | Cost = {1}", Neighbor.Key, Cost);
        }

        #endregion
    }
}
