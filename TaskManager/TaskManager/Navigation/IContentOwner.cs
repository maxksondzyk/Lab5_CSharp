namespace TaskManager.Navigation
{
    internal interface IContentOwner
    {
        INavigatable Content { get; set; }
    }
}
