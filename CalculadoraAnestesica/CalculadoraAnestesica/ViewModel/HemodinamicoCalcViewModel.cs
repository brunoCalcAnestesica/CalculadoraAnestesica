using System;
using System.Data;
using System.Reflection;
using System.Windows.Input;
using CalculadoraAnestesica.ViewModel.Base;
using CalculadoraAnestesica.ViewModel.Interfaces;
using ExcelDataReader;
using Xamarin.Forms;
using System.Linq;
using System.Collections.ObjectModel;
using CalculadoraAnestesica.Model;
using System.Collections.Generic;

namespace CalculadoraAnestesica.ViewModel
{
    public class HemodinamicoCalcViewModel : ViewModelBase, IHemodinamicoCalcViewModel
    {
        private DataSet DataSet { get; set; }

        public List<string> PickerItems { get; set; }
        public string SelectedItemPicker { get; set; }

        private ObservableCollection<HemodinamicoResult> hemodinamicoModelList;
        public ObservableCollection<HemodinamicoResult> HemodinamicoModelList
        {
            get { return hemodinamicoModelList; }
            set
            {
                hemodinamicoModelList = value;
                RaisePropertyChanged();
            }
        }

        private double entryIdade;
        public double EntryIdade
        {
            get { return entryIdade; }
            set
            {
                entryIdade = value;
                RaisePropertyChanged();
            }
        }

        private double entryPeso;
        public double EntryPeso
        {
            get { return entryPeso; }
            set
            {
                entryPeso = value;
                RaisePropertyChanged();
            }
        }

        public ICommand ExecuteSearch
        {
            get { return new Command(ExecuteExcelSearch); }
        }

        public HemodinamicoCalcViewModel()
		{
            OpenExcelFile();
            PickerItems = new List<string>
            {
                "Selecione...",
                "Meses",
                "Dias",
                "Anos"
            };

            SelectedItemPicker = "Selecione...";
        }

        private void OpenExcelFile()
        {
            var stream = Assembly
               .GetExecutingAssembly()
               .GetManifestResourceStream(
                    "CalculadoraAnestesica.EmbeddedResources.MedCalcHemodinamico.xlsx"
                );

            var reader = ExcelReaderFactory.CreateReader(stream);

            var conf = new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration
                {
                    UseHeaderRow = true
                }
            };

            DataSet = reader.AsDataSet(conf);
        }

        private void CloseExcelFile()
        {
            DataSet.Dispose();
        }

        private void ExecuteExcelSearch()
        {
            if (DataSet != null)
            {
                if (SelectedItemPicker.Contains("Selecione"))
                    return;

                HemodinamicoModelList = new ObservableCollection<HemodinamicoResult>();

                var tableRows = DataSet
                    .Tables[0]
                    .Rows.Cast<DataRow>()
                    .ToList();

                DataRow item = null;

                if (EntryIdade >= 20)
                {
                    item = tableRows.LastOrDefault();
                }
                else
                {
                    int index = (int)Math.Round((EntryIdade * 365), 0);
                    item = tableRows[index];
                }

                var modelResultFC = new HemodinamicoResult();
                modelResultFC.Text = "FC";
                modelResultFC.Value = (string)item.ItemArray[6];

                var modelResultPA = new HemodinamicoResult();
                modelResultPA.Text = "PA";
                modelResultPA.Value = (string)item.ItemArray[7];

                var modelResultFR = new HemodinamicoResult();
                modelResultFR.Text = "FR";
                modelResultFR.Value = (string)item.ItemArray[8];

                var modelResultMasc = new HemodinamicoResult();
                modelResultMasc.Text = "Mascara";
                modelResultMasc.Value = (string)item.ItemArray[9];

                var modelResultLam = new HemodinamicoResult();
                modelResultLam.Text = "Lamina";
                modelResultLam.Value = (string)item.ItemArray[10];

                var modelResultETT = new HemodinamicoResult();
                modelResultETT.Text = "ETT";
                modelResultETT.Value = (string)item.ItemArray[11];

                HemodinamicoModelList.Add(modelResultFC);
                HemodinamicoModelList.Add(modelResultPA);
                HemodinamicoModelList.Add(modelResultFR);
                HemodinamicoModelList.Add(modelResultMasc);
                HemodinamicoModelList.Add(modelResultLam);
                HemodinamicoModelList.Add(modelResultETT);
            }
        }
    }
}

