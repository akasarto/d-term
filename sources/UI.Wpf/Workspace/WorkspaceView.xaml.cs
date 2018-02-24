using ReactiveUI;
using System.Windows;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace UI.Wpf.Workspace
{
	/// <summary>
	/// App main workspace.
	/// </summary>
	public partial class WorkspaceView : IViewFor<IWorkspaceViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public WorkspaceView()
		{
			InitializeComponent();

			this.WhenActivated(activator =>
			{
				activator(this.WhenAnyValue(@this => @this.DataContext).BindTo(this, @this => @this.ViewModel));

				activator(ViewModel.OpenGeneralSettingsViewInteraction.RegisterHandler(ctx =>
				{
					var settingsView = new GeneralSettingsView();

					settingsView.Owner = Application.Current.MainWindow;
					settingsView.WindowStartupLocation = WindowStartupLocation.CenterOwner;

					settingsView.ShowDialog();

					ctx.SetOutput(Unit.Default);
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
