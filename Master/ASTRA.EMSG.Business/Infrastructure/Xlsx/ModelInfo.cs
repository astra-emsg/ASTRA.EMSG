namespace ASTRA.EMSG.Business.Infrastructure.Xlsx
{
    public class ModelInfo<TModel>
    {
        public ModelInfo(TModel model, int rowNumber)
        {
            Model = model;
            RowNumber = rowNumber;
        }

        public int RowNumber { get; set; }
        public TModel Model { get; set; }
    }
}