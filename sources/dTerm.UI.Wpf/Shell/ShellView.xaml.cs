﻿using dTerm.WinNative;
using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Controls;
using System.Collections.Specialized;
using Dragablz.Dockablz;
using Dragablz;

namespace dTerm.UI.Wpf.Shell
{
	public partial class ShellView : Window
	{
		private object[] _order;
		private ShellViewModel _viewModel = null;

		public ShellView(ShellViewModel viewModel)
		{
			InitializeComponent();
			_viewModel = viewModel;
			DataContext = _viewModel;
			SourceInitialized += ShellView_SourceInitialized;
			Loaded += ShellView_Loaded;

			AddHandler(DragablzItem.DragStarted, new DragablzDragStartedEventHandler(ItemDragStarted), true);
			AddHandler(DragablzItem.DragCompleted, new DragablzDragCompletedEventHandler(ItemDragCompleted), true);
		}

		private void ItemDragStarted(object sender, DragablzDragStartedEventArgs e)
		{
			var item = e.DragablzItem.DataContext;

			System.Diagnostics.Trace.WriteLine($"User started to drag item: {item}.");
		}

		private void ItemDragCompleted(object sender, DragablzDragCompletedEventArgs e)
		{
			var item = e.DragablzItem.DataContext;
			System.Diagnostics.Trace.WriteLine($"User finished dragging item: {item}.");

			if (_order == null) return;

			System.Diagnostics.Trace.Write("Order is now: ");
			foreach (var i in _order)
			{
				System.Diagnostics.Trace.Write(i + " ");
			}
			System.Diagnostics.Trace.WriteLine("");
		}

		private void StackPositionMonitor_OnOrderChanged(object sender, OrderChangedEventArgs e)
		{
			_order = e.NewOrder;
		}

		private void ShellView_Loaded(object sender, RoutedEventArgs e)
		{
			_viewModel.ConsoleInstances.CollectionChanged += ConsoleInstances_CollectionChanged;
		}

		private void ConsoleInstances_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			Layout.TileFloatingItemsCommand.Execute(null, consolesContainer);
		}

		private void TabItem_Click(object sender, RoutedEventArgs e)
		{
			((TabItem)sender).IsSelected = true;
			e.Handled = true;
		}

		private void ShellView_SourceInitialized(object sender, EventArgs e)
		{
			var hwndSource = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
			hwndSource.AddHook(new HwndSourceHook(WndProc));
		}

		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			var message = (WM)msg;

			switch (message)
			{
				/*
				case WM.APPViewHighlight:
					{
						//ShowWindow(hwnd, wParam);
					}
					break;

				case WM.MOUSEACTIVATE:
					{
						handled = true;
						return new IntPtr((int)MouseActivationResult.MA_NOACTIVATE);
					}
				*/

				case WM.SETCURSOR:
					{
						var wlParam = new Win32Param()
						{
							BaseValue = (uint)lParam
						};

						var wMouseMsg = (WM)wlParam.HIWord;

						switch (wMouseMsg)
						{
							case WM.LBUTTONDOWN:
							case WM.RBUTTONDOWN:
							case WM.MBUTTONDOWN:
								{
									ShowWindow(hwnd, wParam);
									handled = true;
								}
								break;
						}
					}
					break;

					/*
				case WM.SYSCOMMAND:
					{
						var uCmdType = (SysCommand)wParam;

						switch (uCmdType)
						{
							case SysCommand.SC_CLOSE:
								{
									// IConsoleService is responsible for closing the window
									//_consoleInstance.Terminate();
									//HideWindow(hwnd);
									handled = true;
								}
								break;

							case SysCommand.SC_MINIMIZE:
								{
									//EventBus.Publish(new HideConsoleMessage(_consoleInstance));
									//HideWindow(hwnd);
									handled = true;
								}
								break;
						}
					}
					break;
					*/
			}

			return IntPtr.Zero;
		}

		private void ShowWindow(IntPtr ownerWindowHandle, IntPtr processWindowHandle)
		{
			NativeMethods.SetForegroundWindow(processWindowHandle);
			//NativeMethods.SetActiveWindow(ownerWindowHandle);
			//NativeMethods.SendMessage(ownerWindowHandle, (uint)WM.NCACTIVATE, new IntPtr(1), IntPtr.Zero);
			//NativeMethods.SendMessage(ownerWindowHandle, (uint)WM.NCPAINT, new IntPtr(1), IntPtr.Zero);

			//EventBus.Publish(new ShowConsoleMessage(_consoleInstance));
		}
	}
}