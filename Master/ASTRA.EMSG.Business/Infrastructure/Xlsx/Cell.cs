using ClosedXML.Excel;

namespace ASTRA.EMSG.Business.Infrastructure.Xlsx
{
    public class Cell
    {
        private readonly IXLCell xlCell;

        public Cell(IXLCell xlCell)
        {
            this.xlCell = xlCell;

            ColumnId = xlCell.Address.ColumnLetter;
            Id = xlCell.Address.ColumnLetter + xlCell.Address.RowNumber;

            Row = xlCell.Address.RowNumber;
            Column = xlCell.Address.ColumnNumber;
        }

        public string Value
        {
            get { return xlCell.Value.ToString().Trim(); }
            set { xlCell.Value = value; }
        }

        public string Id { get; private set; }
        public int Row { get; private set; }
        public int Column { get; private set; }
        public string ColumnId { get; private set; }

        public bool IsEmpty { get { return string.IsNullOrEmpty(Value); } }
    }
}