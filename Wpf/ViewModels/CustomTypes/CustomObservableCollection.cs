using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Wpf.ViewModels.CustomTypes
{
    public class CustomObservableCollection<T> : ObservableCollection<T> 
        where T : INotifyPropertyChanged
    {
		public event PropertyChangedEventHandler ItemPropertyChanged;

		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add: AttachItems(e.NewItems.Cast<T>()); break;
				case NotifyCollectionChangedAction.Remove: DetachItems(e.OldItems.Cast<T>()); break;
				case NotifyCollectionChangedAction.Replace:
				{
					DetachItems(e.OldItems.Cast<T>());
					AttachItems(e.NewItems.Cast<T>());
					break;
				}
			};

			base.OnCollectionChanged(e);
		}

		private void DetachItems(IEnumerable<T> items)
		{
			foreach (var item in items)
			{
				item.PropertyChanged -= OnItemPropertyChanged;
			}
		}

		private void AttachItems(IEnumerable<T> items)
		{
			foreach (var item in items)
			{
				item.PropertyChanged += OnItemPropertyChanged;
			}
		}

		private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e) =>
			ItemPropertyChanged?.Invoke(sender, e);
	}
}
