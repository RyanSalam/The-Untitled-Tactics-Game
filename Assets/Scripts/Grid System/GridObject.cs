using Tactics.UnitSystem;

namespace Tactics.GridSystem
{
    public class GridObject
    {
        private GridPosition position;
        private UnitObject unit;

        public GridObject(GridPosition position)
        {
            this.position = position;
        }
        public void SetUnit(UnitObject newUnit)
        {
            this.unit = newUnit;
        }
        public void RemoveUnit()
        {
            this.unit = null;
        }
        public UnitObject GetUnit()
        {
            return unit;
        }
        public bool HasUnit()
        {
            return unit != null;
        }
    }
}


