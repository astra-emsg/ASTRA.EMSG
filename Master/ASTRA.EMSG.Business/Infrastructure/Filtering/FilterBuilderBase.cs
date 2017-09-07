namespace ASTRA.EMSG.Business.Infrastructure.Filtering
{
    public interface IFilterBuilder
    {
        void Initialize(IFilterParameter filterParameter);
    }

    public interface IFilterBuilder<in TFilterParameter> : IFilterBuilder where TFilterParameter : IFilterParameter
    {
        void Initialize(TFilterParameter filterParameter);
    }

    public class FilterBuilderBase<TFilterParameter> : IFilterBuilder<TFilterParameter> where TFilterParameter : IFilterParameter
    {
        private TFilterParameter parameter;

        protected TFilterParameter Parameter
        {
            get { return parameter; }
        }

        public void Initialize(TFilterParameter filterParameter)
        {
            parameter = filterParameter;
        }

        public void Initialize(IFilterParameter filterParameter)
        {
            Initialize((TFilterParameter)filterParameter);
        }
    }
}