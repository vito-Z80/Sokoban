namespace Objects.Switchers
{
    public abstract class Controlled : MainObject
    {
        public abstract void Activate();
        public abstract void Deactivate();
    }
}