namespace Game339.Shared.Models 
{
    public class TowerUnit : Unit
    {
        public int Range { get; }
        public int Damage { get; }
        public bool HasActed { get; private set; }

        public TowerUnit(GridPosition position, int range, int damage) : base(position) 
        {
            Range = range;
            Damage = damage;
            HasActed = false; 
        } 

        public void MarkUsed()
        {
            HasActed = true;
        }

        public void ResetTurn()
        {
            HasActed = false;
        } 
    } 
}