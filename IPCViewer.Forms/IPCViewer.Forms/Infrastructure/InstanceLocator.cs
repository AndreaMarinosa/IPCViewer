
namespace IPCViewer.Forms.Infrastructure
{
    using IPCViewer.Forms.ViewModels;

    class InstanceLocator
    {
        public MainViewModel Main { get; set; }

        public InstanceLocator()
        {
            this.Main = new MainViewModel();
        }

    }
}
