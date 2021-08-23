using ReactiveUI;

namespace dTerm.UI.Wpf.Views
{
    public abstract class ActivityListBase : BaseUserControl<ActivityListViewModel> { }

    public partial class ActivityList : ActivityListBase
    {
        public ActivityList()
        {
            InitializeComponent();

            this.WhenActivated(bindings =>
            {
                ViewModel ??= new ActivityListViewModel();
            });
        }
    }
}
