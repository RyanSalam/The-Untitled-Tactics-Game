using Tactics.UnitSystem;

namespace Tactics.GridSystem
{
    public class GridObject
    {
        private GridPosition position;
        private UnitObject unit;
        private bool isWalkable = true;
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
        public void SetIsWalkable(bool value)
        {
            isWalkable = value;
        }
        public bool GetIsWalkable()
        {
            return isWalkable;
        }
    }
}


