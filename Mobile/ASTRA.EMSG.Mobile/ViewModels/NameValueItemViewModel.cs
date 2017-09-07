using ASTRA.EMSG.Mobile.Utils;

namespace ASTRA.EMSG.Mobile.ViewModels
{
    public class NameValueItemViewModel<TValue> : NotifyPropertyChanged
    {
        public NameValueItemViewModel(string name, TValue value)
        {
            Name = name;
            Value = value;
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; Notify(() => Name); }
        }

        private TValue value;
        public TValue Value
        {
            get { return value; }
            set { this.value = value; Notify(() => Value); }
        }

        public bool Equals(NameValueItemViewModel<TValue> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.name, name) && Equals(other.value, value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(NameValueItemViewModel<TValue>)) return false;
            return Equals((NameValueItemViewModel<TValue>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((name != null ? name.GetHashCode() : 0) * 397) ^ value.GetHashCode();
            }
        }
    }

    public class NameValueItemViewModel : NameValueItemViewModel<string>
    {
        public NameValueItemViewModel(string name, string value) : base(name, value) { }
    }
}