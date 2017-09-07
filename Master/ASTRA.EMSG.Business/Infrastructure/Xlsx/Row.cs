using System.Collections.Generic;

namespace ASTRA.EMSG.Business.Infrastructure.Xlsx
{
    public class Row
    {
        public Row(int rowNumber)
        {
            RowNumber = rowNumber;
            Cells = new List<Cell>();
        }

        public int RowNumber { get; set; }
        public int Length { get { return Cells.Count; } }
        public List<Cell> Cells { get; set; }
    }
}