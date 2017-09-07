using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClosedXML.Excel;

namespace ASTRA.EMSG.Business.Infrastructure.Xlsx
{
    public class XlsxDoc
    {
        private readonly int headerColumnCount;
        public IXLWorksheet worksheet { get; set; }

        public XlsxDoc(int headerColumnCount)
        {
            this.headerColumnCount = headerColumnCount;

            var xlWorkbook = new XLWorkbook();
            worksheet = xlWorkbook.Worksheets.Add("Worksheet");
        }

        public bool TryLoad(Stream stream)
        {
            XLWorkbook xlWorkbook;

            try
            {
                xlWorkbook = new XLWorkbook(stream);
            }
            catch (Exception)
            {
                return false;
            }

            if (xlWorkbook.Worksheets.Count < 1)
                return false;

            worksheet = xlWorkbook.Worksheets.First();

            return true;
        }

        public Cell GetCell(int row, int column)
        {
            return new Cell(worksheet.Cell(row, column));
        }

        public Row HeaderRow { get { return ReadRow(1); } }

        public List<Row> DataMatrix
        {
            get
            {
                var rows = new List<Row>();

                int row = 2;
                while (!IsEndOfTable(row))
                {
                    rows.Add(ReadRow(row));
                    row++;
                }

                return rows;
            }
        }

        public bool IsThereExtraColumns { get { return worksheet.CellsUsed().Any(c => c.Address.ColumnNumber > headerColumnCount); } }

        public Stream Save()
        {
            var memoryStream = new MemoryStream();
            worksheet.Workbook.SaveAs(memoryStream);
            return memoryStream;
        }

        private Row ReadRow(int rowNumber)
        {
            var row = new Row(rowNumber);
            for (int column = 1; column <= headerColumnCount; column++)
                row.Cells.Add(GetCell(rowNumber, column));
            return row;
        }

        private bool IsEndOfTable(int row)
        {
            bool isEndOfTable = true;
            for (int column = 1; column <= headerColumnCount; column++)
                isEndOfTable &= GetCell(row, column).IsEmpty;
            return isEndOfTable;
        }
    }
}