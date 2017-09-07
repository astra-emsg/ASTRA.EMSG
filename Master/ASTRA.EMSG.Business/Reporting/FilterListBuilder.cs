using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ASTRA.EMSG.Business.Services.Common;

namespace ASTRA.EMSG.Business.Reporting
{
    public interface IFilterListBuilder<TReportParameter> where TReportParameter : EmsgReportParameter
    {
        void AddFilterListItem(Expression<Func<TReportParameter, object>> expressions);
        void AddFilterListItem(Func<TReportParameter, string> key, Func<TReportParameter, string> value);
        void AddFilterListItem(Expression<Func<TReportParameter, object>> key, Func<TReportParameter, string> value);
    }

    public class FilterListBuilder<TReportParameter> : IFilterListBuilder<TReportParameter> where TReportParameter : EmsgReportParameter
    {
        private readonly List<Func<TReportParameter, FilterListPo>> filterListPoProviders = new List<Func<TReportParameter, FilterListPo>>();

        private readonly ILocalizationService localizationService;

        public FilterListBuilder(ILocalizationService localizationService)
        {
            this.localizationService = localizationService;
        }

        public List<FilterListPo> GenerateFilterListPos(TReportParameter parameter)
        {
            return filterListPoProviders.Select(filterListPoProvider => filterListPoProvider(parameter)).Where(po => !string.IsNullOrEmpty(po.Value)).ToList();
        }

        public void AddFilterListItem(Expression<Func<TReportParameter, object>> expressions)
        {
            filterListPoProviders.Add(p =>
                                          {
                                              dynamic value = expressions.Compile().Invoke(p) ?? "";
                                              var valueString = "";
                                              if (value.GetType().IsGenericTypeDefinition && value.GetType().GetGenericTypeDefinition() == typeof(Nullable<>))
                                              {
                                                  value = value.HasValue ? value.Value : "";
                                              }
                                              if (value.GetType().IsEnum)
                                                  valueString = localizationService.GetLocalizedEnum(value);
                                              else if(value is DateTime)
                                                  valueString = ((DateTime)value).ToString("d");
                                              else
                                                  valueString = value.ToString();

                                              return new FilterListPo(localizationService.GetLocalizedModelPropertyText(expressions), valueString);
                                          });
        }

        public void AddFilterListItem(Func<TReportParameter, string> key, Func<TReportParameter, string> value)
        {
            filterListPoProviders.Add(p => new FilterListPo(key(p), value(p)));
        }

        public void AddFilterListItem(Expression<Func<TReportParameter, object>> key, Func<TReportParameter, string> value)
        {
            filterListPoProviders.Add(p => new FilterListPo(localizationService.GetLocalizedModelPropertyText(key), value(p)));
        }
    }
}