using ReactiveUI;
using Splat;
using System.Reactive;
using System.Windows;
using System.Windows.Controls;
using UI.Wpf.Settings;

namespace UI.Wpf.Workspace
{
	/// <summary>
	/// App main workspace.
	/// </summary>
	public partial class WorkspaceView : UserControl, IViewFor<IWorkspaceViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public WorkspaceView()
		{
			InitializeComponent();

			this.WhenActivated(activator =>
			{
				activator(this.WhenAnyValue(@this => @this.ViewModel).BindTo(this, @this => @this.DataContext));

				activator(ViewModel.OpenSettingsInteraction.RegisterHandler(context =>
				{
					var settingsView = Locator.CurrentMutable.GetService<IViewFor<ISettingsViewModel>>() as Window;

					settingsView.Owner = Application.Current.MainWindow;
					settingsView.DataContext = context.Input;

					settingsView.ShowDialog();

					context.SetOutput(Unit.Default);
				}));
			});
		}

		/// <summary>
		/// View model dependency property backing field.
		/// </summary>
		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IWorkspaceViewModel), typeof(WorkspaceView), new PropertyMetadata(null));

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		public IWorkspaceViewModel ViewModel
		{
			get { return (IWorkspaceViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IWorkspaceViewModel)value; }
		}
	}
}
