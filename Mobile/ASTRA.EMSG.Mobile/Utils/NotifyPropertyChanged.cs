using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Mobile.Utils
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Notify(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        public void Notify<TProperty>(Expression<Func<TProperty>> property)
        {
            Notify(ExpressionHelper.GetPropertyName(property));
        }

        public void DelegateEvent<TNotificationSource, TSourcePropertyType, TTargetPropertyType>(TNotificationSource notificationSource, Expression<Func<TSourcePropertyType>> sourceProperty, Expression<Func<TTargetPropertyType>> targetProperty)
            where TNotificationSource : INotifyPropertyChanged
        {
            notificationSource.PropertyChanged +=
                (s, e) => { if (e.PropertyName == ExpressionHelper.GetPropertyName(sourceProperty)) Notify(targetProperty); };
        }

        public void DoActionOnPropertyChanged<TNotificationSource, TSourcePropertyType>(TNotificationSource notificationSource, Expression<Func<TSourcePropertyType>> sourceProperty, Action action)
            where TNotificationSource : INotifyPropertyChanged
        {
            notificationSource.PropertyChanged +=
                (s, e) => { if (e.PropertyName == ExpressionHelper.GetPropertyName(sourceProperty)) action(); };
        }

        public void DelegateEvent<TSourcePropertyType, TTargetPropertyType>(Expression<Func<TSourcePropertyType>> sourceProperty, Expression<Func<TTargetPropertyType>> targetProperty)
        {
            PropertyChanged +=
                (s, e) => { if (e.PropertyName == ExpressionHelper.GetPropertyName(sourceProperty)) Notify(targetProperty); };
        }

        public void DelegateEvent<TNotificationSource, TTargetPropertyType>(TNotificationSource notificationSource, Expression<Func<TTargetPropertyType>> targetProperty)
            where TNotificationSource : INotifyCollectionChanged
        {
            notificationSource.CollectionChanged += (s, e) => Notify(targetProperty);
        }

        public void DelegateAllEvent<TNotificationSource, TTargetPropertyType>(TNotificationSource notificationSource, Expression<Func<TTargetPropertyType>> targetProperty)
            where TNotificationSource : INotifyPropertyChanged
        {
            notificationSource.PropertyChanged += (s, e) => Notify(targetProperty);
        }
    }
}