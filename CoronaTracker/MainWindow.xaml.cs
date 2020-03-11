using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CoronaTracker
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		#region Fields

		public static readonly RoutedCommand CommandFileExit = new RoutedCommand();
		public static readonly RoutedCommand CommandRefresh = new RoutedCommand();

		#endregion

		#region Properties

		public bool CanRefresh
		{
			get => m_CanRefresh;
			private set
			{
				m_CanRefresh = value;
				OnPropertyChanged(nameof(CanRefresh));
			}
		}
		private bool m_CanRefresh;

		public string Source
		{
			get => m_Source;
			set
			{
				m_Source = value;
				OnPropertyChanged(nameof(Source));
			}
		}
		private string m_Source;

		#endregion

		#region Methods

		#region Commands

		private void CommandFileExit_Executed(object sender, ExecutedRoutedEventArgs e) => Close();

		private void CommandRefresh_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = CanRefresh;

		private void CommandRefresh_Executed(object sender, ExecutedRoutedEventArgs e) => Refresh(Properties.Resources.WebLink_CoronaTrackerApi);

		#endregion

		private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) => ((LineSeries)ChartHistory.Series[0]).ItemsSource = (e.AddedItems[0] as CoronaLocationInfo)?.History;

		private void HyperlinkClick(object sender, RoutedEventArgs e) => Process.Start((e.OriginalSource as Hyperlink).NavigateUri.OriginalString);

		protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		private void Refresh(string location)
		{
			CanRefresh = false;
			DataGridConfirmed.ItemsSource = null;
			DataGridDeaths.ItemsSource = null;
			DataGridRecovered.ItemsSource = null;
			Source = Properties.Resources.WebLink_CoronaTrackerApi;
			using (WebClient webClient = new WebClient())
			{
				webClient.DownloadDataCompleted += WebClient_DownloadDataCompleted;
				webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
				webClient.DownloadDataAsync(new Uri(location));
			}
		}

		private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ChartHistory.Title = ((sender as TabControl).SelectedItem as TabItem).Header;
			((LineSeries)ChartHistory.Series[0]).Title = ChartHistory.Title;
		}

		private void WebClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
		{
			//Dispatcher.Invoke(() => ProgressBarProgress.IsIndeterminate = false);
			Task.Factory.StartNew(new Action(() =>
			{
				using (Stream stream = new MemoryStream(e.Result))
				using (JsonTextReader reader = new JsonTextReader(new StreamReader(stream)))
				{
					JsonSerializer serializer = JsonSerializer.Create();
					CoronaData data = (CoronaData)serializer.Deserialize(reader, typeof(CoronaData));
					// Confirmed
					ListCollectionView collectionView = new ListCollectionView(data.Confirmed.Locations);
					collectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(CoronaLocationInfo.Country)));
					collectionView.SortDescriptions.Add(new SortDescription(nameof(CoronaLocationInfo.Country), ListSortDirection.Ascending));
					collectionView.SortDescriptions.Add(new SortDescription(nameof(CoronaLocationInfo.Province), ListSortDirection.Ascending));
					Dispatcher.Invoke(() => DataGridConfirmed.ItemsSource = collectionView);
					// Deaths
					collectionView = new ListCollectionView(data.Deaths.Locations);
					collectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(CoronaLocationInfo.Country)));
					collectionView.SortDescriptions.Add(new SortDescription(nameof(CoronaLocationInfo.Country), ListSortDirection.Ascending));
					collectionView.SortDescriptions.Add(new SortDescription(nameof(CoronaLocationInfo.Province), ListSortDirection.Ascending));
					Dispatcher.Invoke(() => DataGridDeaths.ItemsSource = collectionView);
					// Recovered
					collectionView = new ListCollectionView(data.Recovered.Locations);
					collectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(CoronaLocationInfo.Country)));
					collectionView.SortDescriptions.Add(new SortDescription(nameof(CoronaLocationInfo.Country), ListSortDirection.Ascending));
					collectionView.SortDescriptions.Add(new SortDescription(nameof(CoronaLocationInfo.Province), ListSortDirection.Ascending));
					Dispatcher.Invoke(() => DataGridRecovered.ItemsSource = collectionView);
				}
			})).ContinueWith(new Action<Task>((task) => Dispatcher.Invoke(() => CanRefresh = true)));
		}

		private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) => Dispatcher.Invoke(() => ProgressBarProgress.Value = e.ProgressPercentage);

		private void Window_Loaded(object sender, RoutedEventArgs e) => Refresh(Properties.Resources.WebLink_CoronaTrackerApi);

		#endregion

		#region Constructor

		public MainWindow()
		{
			DataContext = this;
			InitializeComponent();
		}

		#endregion

		#region Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}
