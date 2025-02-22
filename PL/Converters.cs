﻿using System.Globalization;
using System.Windows.Data;
using System;
using System.Windows.Media;
using BO;

namespace PL;

class ConvertIdToContent : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? "Add" : "Update";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }  
}

internal class IsEnableConverter : IValueConverter
{
    //convert from source property type to target property type
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((int)value != 0)
        {
            return false; 
        }
        else
        {
            return true;
        }
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class IsVisibleConverter : IValueConverter
{
    //convert from source property type to target property type
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value.GetType() != typeof(TaskInList))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class IsCurrentTask : IValueConverter 
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ConvertStatusToColor : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string status = (string)value;
        switch (status)
        {
            case "UNSCHEDULED":
                return Brushes.White;
            case "SCHEDULED":
                return Brushes.Beige;
            case "STARTED":
                return Brushes.Orange;
            case "DONE":
                return Brushes.Green;
            default:
                return Brushes.White;
        }
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
internal class ConvertStatusToForground : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string status = (string)value;
        switch (status)
        {
            case "UNSCHEDULED":
                return Brushes.White;
            case "SCHEDULED":
                return Brushes.Beige;
            case "STARTED":
                return Brushes.Orange;
            case "DONE":
                return Brushes.Green;
            default:
                return Brushes.Black;
        }
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
internal class IsIn : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((ProjectStatus)value == ProjectStatus.IN);            
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
internal class IsBefore : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((ProjectStatus)value == ProjectStatus.BEFORE);
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
internal class IsAfter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((ProjectStatus)value == ProjectStatus.AFTER);
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}