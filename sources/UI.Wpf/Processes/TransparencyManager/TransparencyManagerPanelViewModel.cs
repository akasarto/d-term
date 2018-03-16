using ReactiveUI;
using Splat;
using System;
using System.Reactive.Linq;

namespace UI.Wpf.Processes
{
	public interface ITransparencyManagerPanelViewModel
	{
		byte TransparencyLevel { get; set; }
	}

	public class TransparencyManagerPanelViewModel : ReactiveObject, ITransparencyManagerPanelViewModel
	{
		private readonly IAppState _appState;
		private readonly IReactiveDerivedList<IProcessInstanceViewModel> _processInstances;
		private byte _transparencyLevel;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public TransparencyManagerPanelViewModel(IAppState appState = null)
		{
			_appState = appState ?? Locator.CurrentMutable.GetService<IAppState>();

			_processInstances = _appState.GetProcessInstances().CreateDerivedCollection(
				selector: instance => instance
			);

			_processInstances.ItemsAdded.Subscribe(instance =>
			{
				SetTransparency(instance);
			});

			this.WhenAnyValue(@this => @this.TransparencyLevel).Subscribe(alphaLevel =>
			{
				foreach (var instance in _processInstances)
				{
					SetTransparency(instance);
				}
			});
		}

		public byte TransparencyLevel
		{
			get => _transparencyLevel;
			set => this.RaiseAndSetIfChanged(ref _transparencyLevel, value);
		}

		private void SetTransparency(IProcessInstanceViewModel instance)
		{
			var alphaLevel = Map(_transparencyLevel, 0, 70, 255, 115);

			Win32Api.SetTransparency(instance.ProcessMainWindowHandle, (byte)alphaLevel);
		}

		// https://www.arduino.cc/reference/en/language/functions/math/map/
		private long Map(long x, long in_min, long in_max, long out_min, long out_max)
		{
			return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
		}
	}
}
