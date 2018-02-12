using Consoles.Core;
using System;
using ReactiveUI;

namespace UI.Wpf.Consoles
{
	public class ConsoleViewModel : BaseViewModel
	{
		//
		private Guid _id;
		private int _index;
		private string _name;
		private string _iconPath;
		private PathBuilder _processPathBuilder;
		private string _processPathExeFilename;
		private string _processPathExeArgs;
		private DateTime _utcCreation;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleViewModel()
		{
		}

		public Guid Id
		{
			get => _id;
			set => this.RaiseAndSetIfChanged(ref _id, value);
		}

		public int Index
		{
			get => _index;
			set => this.RaiseAndSetIfChanged(ref _index, value);
		}

		public string Name
		{
			get => _name;
			set => this.RaiseAndSetIfChanged(ref _name, value);
		}

		public string IconPath
		{
			get => _iconPath;
			set => this.RaiseAndSetIfChanged(ref _iconPath, value);
		}

		public PathBuilder ProcessPathBuilder
		{
			get => _processPathBuilder;
			set => this.RaiseAndSetIfChanged(ref _processPathBuilder, value);
		}

		public string ProcessPathExeFilename
		{
			get => _processPathExeFilename;
			set => this.RaiseAndSetIfChanged(ref _processPathExeFilename, value);
		}

		public string ProcessPathExeStartupArgs
		{
			get => _processPathExeArgs;
			set => this.RaiseAndSetIfChanged(ref _processPathExeArgs, value);
		}

		public DateTime UTCCreation
		{
			get => _utcCreation;
			set => this.RaiseAndSetIfChanged(ref _utcCreation, value);
		}
	}
}
