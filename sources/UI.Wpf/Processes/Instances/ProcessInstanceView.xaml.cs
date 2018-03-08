using ReactiveUI;
using System.Windows;

namespace UI.Wpf.Processes
{
	public partial class ProcessInstanceView : IViewFor<IProcessInstanceViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessInstanceView()
		{
			InitializeComponent();

			this.WhenActivated(activator =>
			{
				activator(this.WhenAnyValue(@this => @this.ViewModel).BindTo(this, @this => @this.DataContext));
			});
		}

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IProcessInstanceViewModel), typeof(ProcessInstanceView), new PropertyMetadata(null));

		public IProcessInstanceViewModel ViewModel
		{
			get { return (IProcessInstanceViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IProcessInstanceViewModel)value; }
		}
	}
}
