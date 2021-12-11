using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;

namespace WpfApp1
{
  public static class MethodsForCreateExcel
  {
    public static void CreateCells(ref IXLWorksheet worksheet, List<SecurityThreat> threats)
    {
      for (int i = 0; i < threats.Count; i++)
      {
        for (int j = 0; j < threats[0].ToString().Split('~').Length; j++)
        {
          worksheet.Cell(3 + i, j + 1).Value = threats[i].ToString().Split('~')[j];
          worksheet.Cell(3 + i, j + 1).Style.Alignment.SetWrapText(false);
        }
      }
    }
    public static void CreateHeading(ref IXLWorksheet worksheet, List<string> nameHeading)
    {
      int countColumn = nameHeading.Count();
      for (int cellname = 1; cellname <= countColumn; cellname++)
      {
        worksheet.Cell(2, cellname).Value = nameHeading[cellname - 1];
      }
    }
    public static void CreateBigHeading(ref IXLWorksheet work, string range, string name)
    {
      work.Range(range).Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
      work.Range(range).Merge().Style.Font.Bold = true;
      work.Range(range).Merge().Style.Font.FontSize = 12;
      work.Range(range).Merge().Value = name;
    }
  }
}
