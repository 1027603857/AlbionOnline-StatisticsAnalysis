﻿using StatisticsAnalysisTool.Common;
using StatisticsAnalysisTool.Common.UserSettings;
using StatisticsAnalysisTool.Properties;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace StatisticsAnalysisTool.Trade;

public class TradeMonitoringBindings : INotifyPropertyChanged
{
    private ListCollectionView _tradeCollectionView;
    private ObservableRangeCollection<Trade> _trades = new();
    private string _tradesSearchText;
    private DateTime _datePickerTradeFrom = new(2017, 1, 1);
    private DateTime _datePickerTradeTo = DateTime.UtcNow.AddDays(1);
    private TradeStatsObject _tradeStatsObject = new();
    private TradeOptionsObject _tradeOptionsObject = new();
    private Visibility _isTradeMonitoringPopupVisible = Visibility.Collapsed;
    private GridLength _gridSplitterPosition = GridLength.Auto;
    private int _totalTradeCounts;
    private int _currentTradeCounts;
    private ManuallyTradeMenuObject _tradeManuallyMenuObject = new();
    private bool _isDeleteTradesButtonEnabled = true;

    public TradeMonitoringBindings()
    {
        TradeCollectionView = CollectionViewSource.GetDefaultView(Trades) as ListCollectionView;

        if (TradeCollectionView != null)
        {
            TradeCollectionView.CurrentChanged += UpdateCurrentTradesUi;
            Trades.CollectionChanged += UpdateTotalTradesUi;

            TradeCollectionView.IsLiveSorting = true;
            TradeCollectionView.IsLiveFiltering = true;
            TradeCollectionView.CustomSort = new TradeComparer();
        }
    }

    public ListCollectionView TradeCollectionView
    {
        get => _tradeCollectionView;
        set
        {
            _tradeCollectionView = value;
            OnPropertyChanged();
        }
    }

    public ObservableRangeCollection<Trade> Trades
    {
        get => _trades;
        set
        {
            _trades = value;
            OnPropertyChanged();
        }
    }

    public string TradesSearchText
    {
        get => _tradesSearchText;
        set
        {
            _tradesSearchText = value;
            TradeCollectionView?.Refresh();
            TradeStatsObject.SetTradeStats(TradeCollectionView?.Cast<Trade>().ToList());
            OnPropertyChanged();
        }
    }

    public DateTime DatePickerTradeFrom
    {
        get => _datePickerTradeFrom;
        set
        {
            _datePickerTradeFrom = value;
            TradeCollectionView?.Refresh();
            TradeStatsObject.SetTradeStats(TradeCollectionView?.Cast<Trade>().ToList());
            OnPropertyChanged();
        }
    }

    public DateTime DatePickerTradeTo
    {
        get => _datePickerTradeTo;
        set
        {
            _datePickerTradeTo = value;
            TradeCollectionView?.Refresh();
            TradeStatsObject.SetTradeStats(TradeCollectionView?.Cast<Trade>().ToList());
            OnPropertyChanged();
        }
    }

    public bool IsDeleteTradesButtonEnabled
    {
        get => _isDeleteTradesButtonEnabled;
        set
        {
            _isDeleteTradesButtonEnabled = value;
            OnPropertyChanged();
        }
    }

    public TradeStatsObject TradeStatsObject
    {
        get => _tradeStatsObject;
        set
        {
            _tradeStatsObject = value;
            OnPropertyChanged();
        }
    }

    public ManuallyTradeMenuObject ManuallyTradeMenuObject
    {
        get => _tradeManuallyMenuObject;
        set
        {
            _tradeManuallyMenuObject = value;
            OnPropertyChanged();
        }
    }

    public TradeOptionsObject TradeOptionsObject
    {
        get => _tradeOptionsObject;
        set
        {
            _tradeOptionsObject = value;
            OnPropertyChanged();
        }
    }

    public int TotalTradeCounts
    {
        get => _totalTradeCounts;
        set
        {
            _totalTradeCounts = value;
            OnPropertyChanged();
        }
    }

    public int CurrentTradeCounts
    {
        get => _currentTradeCounts;
        set
        {
            _currentTradeCounts = value;
            OnPropertyChanged();
        }
    }

    public Visibility IsTradeMonitoringPopupVisible
    {
        get => _isTradeMonitoringPopupVisible;
        set
        {
            _isTradeMonitoringPopupVisible = value;
            OnPropertyChanged();
        }
    }

    public GridLength GridSplitterPosition
    {
        get => _gridSplitterPosition;
        set
        {
            _gridSplitterPosition = value;
            SettingsController.CurrentSettings.MailMonitoringGridSplitterPosition = _gridSplitterPosition.Value;
            OnPropertyChanged();
        }
    }

    #region Update ui

    public void UpdateTotalTradesUi(object sender, NotifyCollectionChangedEventArgs e)
    {
        Application.Current.Dispatcher.InvokeAsync(() =>
        {
            TotalTradeCounts = Trades.Count;
        });
    }

    public void UpdateCurrentTradesUi(object sender, EventArgs e)
    {
        Application.Current.Dispatcher.InvokeAsync(() =>
        {
            CurrentTradeCounts = TradeCollectionView.Count;
        });
    }

    #endregion

    #region Filter

    public async Task UpdateFilteredTradesAsync()
    {
        var filteredTrades = await Task.Run(() => Trades.Where(Filter).ToList());

        TradeCollectionView = CollectionViewSource.GetDefaultView(filteredTrades) as ListCollectionView;
        TradeCollectionView?.Refresh();
    }

    private bool Filter(object obj)
    {
        if (TradesSearchText == null)
        {
            return true;
        }

        return obj is Trade trade &&
               trade.Timestamp.Date >= DatePickerTradeFrom.Date &&
               trade.Timestamp.Date <= DatePickerTradeTo.Date &&
               (
                   trade.LocationName != null && trade.LocationName.IndexOf(TradesSearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                   $"T{trade.Item?.Tier}.{trade.Item?.Level}".IndexOf(TradesSearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                   trade.MailTypeDescription.IndexOf(TradesSearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                   trade.Item != null && trade.Item.LocalizedName.IndexOf(TradesSearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                   trade.MailContent.ActualUnitPrice.ToString().IndexOf(TradesSearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                   trade.MailContent.TotalPrice.ToString().IndexOf(TradesSearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                   trade.InstantBuySellContent.UnitPrice.ToString().IndexOf(TradesSearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                   trade.InstantBuySellContent.TotalPrice.ToString().IndexOf(TradesSearchText, StringComparison.OrdinalIgnoreCase) >= 0
               );
    }

    #endregion

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}