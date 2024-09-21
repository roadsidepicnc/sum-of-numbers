namespace CommandManagement
{
    public interface ICommand
    {
        public void Execute();
        public void Complete(bool closeMask = true);
    }
}