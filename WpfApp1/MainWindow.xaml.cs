using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using ClosedXML.Excel;
using System.Net;
using Microsoft.Win32;

namespace WpfApp1
{

  public partial class MainWindow : Window
  {
    private static PagingCollectionView _cview;
    private WebClient client = new WebClient();
    private static string filePath = @"..\..\Files\ThrList.XLSX";

    private static List<string> headingNames = new List<string>();

    private static List<SecurityThreat> NewThreats = new List<SecurityThreat>();
    private static List<SecurityThreat> OldThreats = new List<SecurityThreat>();
    private static List<SecurityThreat> listBefore = new List<SecurityThreat>();
    private static List<SecurityThreat> listAfter = new List<SecurityThreat>();
    private static List<SimpleTherat> simpleThreats = new List<SimpleTherat>();

    public MainWindow()
    {
      InitializeComponent();

      TryReadFile();
    }
    private void TryReadFile()
    {
      try
      {
        OldThreats = ParserExcel(filePath);
      }
      catch (Exception )
      {
        MessageBoxResult result = MessageBox.Show("Не удалось в папке с файлами найти нужный файл.\nХотите его загрузить на ваше устройство?\n (Отказ приведет к закрытию приложения)", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Information);
        switch (result)
        {
          case MessageBoxResult.Yes:
            client.DownloadFile(@"https://bdu.fstec.ru/files/documents/thrlist.xlsx", filePath);
            OldThreats = ParserExcel(filePath);
            break;
          case MessageBoxResult.No:
            Close();
            break;
        }
      }
      finally
      {
        CreatSimpleThreats();
      }
    }
    private void CreatSimpleThreats()
    {
      simpleThreats.Clear();
      foreach (var item in OldThreats)
      {
        simpleThreats.Add(new SimpleTherat(item.Id_UBI, item.Name));
      }
      _cview = new PagingCollectionView(simpleThreats, 15);
      Grid_People.DataContext = _cview;
    }
    private List<SecurityThreat> ParserExcel(string filename)
    {
      headingNames.Clear();
      List<SecurityThreat> items = new List<SecurityThreat>();
      var workbook = new XLWorkbook(filename);
      var worksheet = workbook.Worksheet(1);
      var countRows = worksheet.RangeUsed().RowsUsed().Count();
      var countColumn = worksheet.RangeUsed().ColumnsUsed().Count();

      for (int cellname = 1; cellname <= countColumn - 2; cellname++)
      {
        headingNames.Add(worksheet.Cell(2, cellname).Value.ToString());
      }

      for (int row = 3; row <= countRows; row++)
      {
        items.Add(new SecurityThreat(Convert.ToInt32(worksheet.Cell(row, 1).Value), worksheet.Cell(row, 2).Value.ToString(), worksheet.Cell(row, 3).Value.ToString(), worksheet.Cell(row, 4).Value.ToString(), worksheet.Cell(row, 5).Value.ToString(), Convert.ToInt32(worksheet.Cell(row, 6).Value), Convert.ToInt32(worksheet.Cell(row, 7).Value), Convert.ToInt32(worksheet.Cell(row, 8).Value)));
      }
      return items;
    }
    private void OnNextClicked(object sender, RoutedEventArgs e)
    {
      _cview.MoveToNextPage();
    }

    private void OnPreviousClicked(object sender, RoutedEventArgs e)
    {
      _cview.MoveToPreviousPage();
    }
    private void Button_Update_Click(object sender, RoutedEventArgs e)
    {
      int countUpdate;
      try
      {
        client.DownloadFile(@"https://bdu.fstec.ru/files/documents/thrlist.xlsx", filePath);
        NewThreats = ParserExcel(filePath);
        countUpdate = CompareList();
        OldThreats = NewThreats;
        CreatSimpleThreats();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }
      CreatMessageForUpdate(countUpdate);
    }
    private void CreatMessageForUpdate(int countUpdate)
    {
      if (countUpdate == 0)
      {
        MessageBox.Show("Обновление сведений: УСПЕШНО \n количество обновленных записей 0, записи не обновлялись", "Статус обновления", MessageBoxButton.OK, MessageBoxImage.Information);
      }
      else
      {
        MessageBoxResult result = MessageBox.Show($"Обновление сведений: УСПЕШНО \n количество обновленных записей {countUpdate}.\n Посмотреть обновления?", "Статус обновления", MessageBoxButton.YesNo, MessageBoxImage.Information);
        switch (result)
        {
          case MessageBoxResult.Yes:
            CreatDifferenceWindow();
            break;
          case MessageBoxResult.No:
            break;
        }
      }
    }
    private void CreatDifferenceWindow()
    {
      DifferenceVersion difference = new DifferenceVersion(listBefore, listAfter);
      difference.Show();
    }
    private int CompareList()
    {
      listAfter.Clear();
      listBefore.Clear();
      int countUppdate = 0;
      for (int i = 0; i < NewThreats.Count; i++)
      {
        for (int j = 0; j < OldThreats.Count; j++)
        {
          if (NewThreats[i].Id_UBI == OldThreats[j].Id_UBI)
          {
            if (NewThreats[i] != OldThreats[j])
            {
              listBefore.Add(OldThreats[j]);
              listAfter.Add(NewThreats[i]);
              countUppdate++;
            }
            break;
          }
        }
      }
      return countUppdate;
    }

    private void Button_Read_Click(object sender, RoutedEventArgs e)
    {
      FullReadThreats fullReadThreats = new FullReadThreats(OldThreats);
      fullReadThreats.Show();
    }

    private void Button_Save_Click(object sender, RoutedEventArgs e)
    {
      var workbookSave = new XLWorkbook();
      var worksheetSave = workbookSave.Worksheets.Add("Лист1");
      MethodsForCreateExcel.CreateBigHeading(ref worksheetSave, "A1:E1", "Общая информация");
      MethodsForCreateExcel.CreateBigHeading(ref worksheetSave, "F1:H1", "Последствия");
      MethodsForCreateExcel.CreateHeading(ref worksheetSave, headingNames);
      MethodsForCreateExcel.CreateCells(ref worksheetSave, OldThreats);

      SaveFileDialog saveFileDialog1 = new SaveFileDialog();
      saveFileDialog1.Filter = "Excel Files|*.xlsx;*.xlsm";
      if (saveFileDialog1.ShowDialog() == false)
        return;
      string filename = saveFileDialog1.FileName;
  
      try
      {
        workbookSave.SaveAs(filename);
        MessageBox.Show("Файл сохранен на вашем устройстве");
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }
  }
}
