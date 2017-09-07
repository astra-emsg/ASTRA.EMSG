using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ASTRA.EMSG.Map.Services;
using ASTRA.EMSG.Common.DataTransferObjects.EventArgs;
using System.IO;
using System.Reflection;
using System.Web.Script.Serialization;

namespace ASTRA.EMSG.Map
{
    /// <summary>
    /// Interaction logic for MapControl.xaml
    /// </summary>
    public partial class MapControl : UserControl
    {
        private bool isLoadComplete = false;
        public MapControl()
        {
            InitializeComponent();

            this.MapBrowser.AllowDrop = false;
            this.MapBrowser.PreviewKeyDown += new KeyEventHandler(MapBrowser_PreviewKeyDown);
            this.MapBrowser.Navigating += new NavigatingCancelEventHandler(MapBrowser_Navigating);
            this.MapBrowser.LoadCompleted += new LoadCompletedEventHandler(MapBrowser_LoadCompleted);
            this.GotFocus += new RoutedEventHandler(MapControl_GotFocus);
        }

        void MapControl_GotFocus(object sender, RoutedEventArgs e)
        {
            this.MapBrowser.Focus();
        }

        void MapBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            this.isLoadComplete = true;
        }

        private void MapBrowser_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            this.isLoadComplete = false;
        }

        void MapBrowser_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //prevent user Key input
            switch (e.Key)
            {
                case Key.Back:
                case Key.BrowserBack:
                case Key.BrowserFavorites:
                case Key.BrowserForward:
                case Key.BrowserHome:
                case Key.BrowserRefresh:
                case Key.BrowserSearch:
                case Key.BrowserStop:
                case Key.F5:
                    e.Handled = true;
                    break;
                default:
                    break;
            }
        }

        public static readonly DependencyProperty HtmlSourceProperty =
       DependencyProperty.RegisterAttached("HtmlSource", typeof(Object), typeof(MapControl), new UIPropertyMetadata(null, HtmlSourcePropertyChanged));


        public Object HtmlSource
        {
            get
            {
                return GetValue(HtmlSourceProperty);
            }
            set
            {
                SetValue(HtmlSourceProperty, value);
            }
        }

        public static void HtmlSourcePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {            
            MapControl mapControl = o as MapControl;
            mapControl.isLoadComplete = false;
            if (mapControl == null) return;

            if (e.NewValue is string)
            {
                Uri uri = null;
                var valueString = e.NewValue as string;
                if (Uri.TryCreate(valueString, UriKind.Absolute, out uri))
                {
                    mapControl.MapBrowser.Source = uri;
                    return;
                }
                else
                {
                    mapControl.MapBrowser.NavigateToString(valueString);
                    return;
                }
            }
            else if (e.NewValue is Uri)
            {
                Uri uri = e.NewValue as Uri;
                mapControl.MapBrowser.Source = uri;
                return;
            }
            else if (e.NewValue is Stream)
            {
                Stream stream = e.NewValue as Stream;
                mapControl.MapBrowser.NavigateToStream(stream);
            }
        }

        public Object ScriptingObject
        {
            get { return GetValue(ScriptingObjectProperty); }
            set { SetValue(ScriptingObjectProperty, value); }
        }

        public static readonly DependencyProperty ScriptingObjectProperty =
            DependencyProperty.Register("ScriptingObject", typeof(Object), typeof(MapControl), new UIPropertyMetadata(null, ScriptingObjectPropertyChanged));

        public static void ScriptingObjectPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            MapControl mapControl = o as MapControl;
            if (mapControl != null)
            {
                mapControl.MapBrowser.ObjectForScripting = e.NewValue;
            }
        }

        public Object JSEventContainer
        {
            get { return GetValue(JSEventContainerProperty); }
            set { SetValue(JSEventContainerProperty, value); }
        }
        public static readonly DependencyProperty JSEventContainerProperty =
            DependencyProperty.Register("JSEventContainer", typeof(Object), typeof(MapControl), new UIPropertyMetadata(null, JSEventContainerPropertyChanged));


        public static void JSEventContainerPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            MapControl mapControl = o as MapControl;
            Object container = e.NewValue;
            Object oldContainer = e.OldValue;

            //unsubscribe oldEvents
            foreach (var entry in mapControl.handlerDict)
            {
                entry.Key.RemoveEventHandler(oldContainer, entry.Value);
            }

            //subscribe new Events
            if (mapControl != null && container != null)
            {
                Type containerType = container.GetType();
                foreach (EventInfo info in containerType.GetEvents())
                {
                    foreach (Object att in info.GetCustomAttributes(false))
                    {
                        if (att is JSEventHandler)
                        {
                            JSEventHandler jshandler = att as JSEventHandler;
                            ParameterInfo[] actionArgs = info.EventHandlerType.GetMethod("Invoke").GetParameters();
                            if (actionArgs.Length <= 4)
                            {
                                Type helperType = null;
                                Type[] parametertypes = actionArgs.Select(a => a.ParameterType).ToArray();
                                switch (actionArgs.Length)
                                {
                                    case 0:
                                        helperType = typeof(EventHelper);
                                        break;
                                    case 1:
                                        helperType = typeof(EventHelper<>).MakeGenericType(parametertypes);
                                        break;
                                    case 2:
                                        //Events have only 2 Parameters so this should be default
                                        helperType = typeof(EventHelper<,>).MakeGenericType(parametertypes);
                                        break;
                                    case 3:
                                        helperType = typeof(EventHelper<,,>).MakeGenericType(parametertypes);
                                        break;
                                    case 4:
                                        helperType = typeof(EventHelper<,,,>).MakeGenericType(parametertypes);
                                        break;
                                    default:
                                        throw new Exception(string.Format("Delegate {0} contains to many arguments (max 4)", info.Name));
                                }

                                object helper = Activator.CreateInstance(helperType, jshandler.MethodName, mapControl);

                                MethodInfo helperInvoke = helperType.GetMethod("CallInvokeScript", parametertypes);
                                Delegate d = Delegate.CreateDelegate
                                        (info.EventHandlerType, helper, helperInvoke);

                                info.AddEventHandler(container, d);
                                mapControl.handlerDict.Add(info, d);
                            }
                        }
                    }
                }
            }
        }
        private Dictionary<EventInfo, Delegate> handlerDict = new Dictionary<EventInfo, Delegate>();

        public void InvokeScrip(string scriptName, params Object[] args)
        {
            //StreamWriter writer = new StreamWriter(String.Format(@"C:\Temp\{0}.json", scriptName));
            //foreach (object o in args)
            //{
            //    writer.Write(new JavaScriptSerializer().Serialize(o));
            //}
            //writer.Flush();
            //writer.Close();
            if (this.isLoadComplete)
            {
                
                this.Dispatcher.Invoke(new Func<string, object[], object>(this.MapBrowser.InvokeScript),scriptName, args);
            }
            else
            {
                new ScriptInvoker(this.MapBrowser, scriptName, args);
            }
        }
    }
    class ScriptInvoker
    {
        private LoadCompletedEventHandler handler;
        public string methodName { get; private set; }
        public Object[] args {get; private set;}
        public WebBrowser browser { get; private set; }

        public ScriptInvoker(WebBrowser browser, string methodName, Object[] args)
        {
            this.browser = browser;
            this.methodName = methodName;
            this.args = args;
            this.handler = new LoadCompletedEventHandler(browser_LoadCompleted);
            this.browser.LoadCompleted += handler;
        }

        void browser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            this.browser.LoadCompleted -= this.handler;
            this.browser.InvokeScript(this.methodName, this.args);
        }
    }
    class EventHelper
    {
        public string methodName { get; private set; }
        public MapControl mapControl { get; private set; }
        public EventHelper(string methodName, MapControl mapControl)
        {
            this.methodName = methodName;
            this.mapControl = mapControl;
        }
        public void CallInvokeScript()
        {
            mapControl.InvokeScrip(methodName, new object[]{});
        }

    }
    class EventHelper<T1> : EventHelper
    {
        public EventHelper(string methodName, MapControl mapControl)
            : base(methodName, mapControl)
        {

        }

        public void CallInvokeScript(T1 args1)
        {
            var args1json = new JavaScriptSerializer().Serialize(args1);
            mapControl.InvokeScrip(methodName, args1json);
        }

    }
    class EventHelper<T1, T2> : EventHelper
    {
        public EventHelper(string methodName, MapControl mapControl)
            : base(methodName, mapControl)
        {

        }

        public void CallInvokeScript(T1 args1, T2 args2)
        {
            var args1json = new JavaScriptSerializer().Serialize(args1);
            var args2json = new JavaScriptSerializer().Serialize(args2);
            mapControl.InvokeScrip(methodName, args1json, args2json);
        }
    }
    class EventHelper<T1, T2, T3> : EventHelper
    {
        public EventHelper(string methodName, MapControl mapControl)
            : base(methodName, mapControl)
        {

        }

        public void CallInvokeScript(T1 args1, T2 args2, T3 args3)
        {
            var args1json = new JavaScriptSerializer().Serialize(args1);
            var args2json = new JavaScriptSerializer().Serialize(args2);
            var args3json = new JavaScriptSerializer().Serialize(args3);
            mapControl.InvokeScrip(methodName, args1json, args2json, args3json);
        }
    }
    class EventHelper<T1, T2, T3, T4> : EventHelper
    {
        public EventHelper(string methodName, MapControl mapControl)
            : base(methodName, mapControl)
        {

        }
        public void CallInvokeScript(T1 args1, T2 args2, T3 args3, T4 args4)
        {
            var args1json = new JavaScriptSerializer().Serialize(args1);
            var args2json = new JavaScriptSerializer().Serialize(args2);
            var args3json = new JavaScriptSerializer().Serialize(args3);
            var args4json = new JavaScriptSerializer().Serialize(args4);
            mapControl.InvokeScrip(methodName, args1json, args2json, args3json, args4json);
        }
    }
}
