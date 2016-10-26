using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Freebuild.WPF
{
    /// <summary>
    /// Base class for controls used to represent data fields
    /// </summary>
    public abstract class FieldControl : LabelledControl
    {
        /// <summary>
        /// ValueChanged event
        /// </summary>
        public event PropertyChangedCallback ValueChanged;

        /// <summary>
        /// Raise a ValueChanged event on this control
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public void RaiseValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PropertyChangedCallback handler = ValueChanged;
            if (handler != null) handler(d, e);
        }

        /// <summary>
        /// Static callback function to raise a ValueChanged event
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FieldControl)d).RaiseValueChanged(d, e);
        }

        /// <summary>
        /// Value dependency property
        /// </summary>
        public static readonly DependencyProperty ValueProperty
            = DependencyProperty.Register("Value", typeof(object), typeof(FieldControl),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnValueChanged)));

        /// <summary>
        /// The value displayed in the field
        /// </summary>
        public object Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /*
        /// <summary>
        /// Timer for delayed update of custom MultiBindings
        /// </summary>
        private DispatcherTimer _DeferredBindingUpdateTimer;

        public FieldControl()
        {
            this.DataContextChanged += HandlesDataContextChanged;
        }

        private void HandlesDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if (e.OldValue != null && e.OldValue is INotifyCollectionChanged)
            {
                INotifyCollectionChanged nCol = (INotifyCollectionChanged)e.OldValue;
                nCol.CollectionChanged -= HandlesContextCollectionChanged;
            }
            if (e.NewValue != null && e.NewValue is INotifyCollectionChanged)
            {
                INotifyCollectionChanged nCol = (INotifyCollectionChanged)e.NewValue;
                nCol.CollectionChanged += HandlesContextCollectionChanged;
            }
        }

        private void HandlesContextCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_DeferredBindingUpdateTimer == null)
            {
                //Initialise the timer:
                _DeferredBindingUpdateTimer = new DispatcherTimer();
                _DeferredBindingUpdateTimer.Interval = new TimeSpan(10000);
                _DeferredBindingUpdateTimer.Tick += HandleDeferredBindingUpdateTimer_Tick;
            }
            _DeferredBindingUpdateTimer.Start();*/
        //May be useful for higher-level situations, but unnecessary here:
        /*LocalValueEnumerator lVE = GetLocalValueEnumerator();
        while (lVE.MoveNext())
        {
            LocalValueEntry current = lVE.Current;
            if (BindingOperations.IsDataBound(this, current.Property))
            {
                MultiBinding mB = BindingOperations.GetMultiBinding(this, current.Property);
                if (mB != null)
                {

                }
            }
        }*/

        //if (newExp != null) newExp.UpdateTarget();
        /*if (e.OldItems != null)
        {
            foreach (object removed in e.OldItems)
            {

            }
        }
        if (e.NewItems != null)
        {
            //Add bindings for new objects:
            foreach (object added in e.NewItems)
            {
                Binding bind = new Binding(cMB.Path);
                bind.Source = added;
                mB.Bindings.Add(bind);
            }
        }*/
        /*    }

        private void HandleDeferredBindingUpdateTimer_Tick(object sender, EventArgs e)
        {
            _DeferredBindingUpdateTimer.Stop();
            MultiBinding mB = BindingOperations.GetMultiBinding(this, ValueProperty);
            if (mB != null && mB is CustomMultiBinding)
            {
                CustomMultiBinding oldMB = (CustomMultiBinding)mB;
                string path = oldMB.Path;
                BindingExpression exp = BindingOperations.GetBindingExpression(this, ValueProperty);
                if (exp != null) exp.UpdateSource();
                CustomMultiBinding newMB = new CustomMultiBinding();
                newMB.Path = path;
                newMB.Converter = oldMB.Converter;

                ICollection col = (ICollection)DataContext;
                foreach (object item in col)
                {
                    Binding bind = new Binding(path);
                    bind.Source = item;
                    newMB.Bindings.Add(bind);
                }
                BindingOperations.SetBinding(this, ValueProperty, newMB);
            }
        }*/
    }
}
