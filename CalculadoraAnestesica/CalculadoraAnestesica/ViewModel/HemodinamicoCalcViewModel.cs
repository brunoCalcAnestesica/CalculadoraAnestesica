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
using System.ComponentModel;
using CalculadoraAnestesica.Shared;
using System.Threading.Tasks;
using CalculadoraAnestesica.Constants;
using CalculadoraAnestesica.Helpers;

namespace CalculadoraAnestesica.ViewModel
{
    public class HemodinamicoCalcViewModel : CalculationViewModelBase, IHemodinamicoCalcViewModel
    {
        private DataSet DataSet { get; set; }
        public List<string> PickerItems { get; set; }

        private bool isHemodinamicoLabelVisible = true;
        public bool IsHemodinamicoLabelVisible
        {
            get { return isHemodinamicoLabelVisible; }
            set
            {
                isHemodinamicoLabelVisible = value;
                RaisePropertyChanged();
            }
        }

        private bool isHemodinamicoModelListVisible;
        public bool IsHemodinamicoModelListVisible
        {
            get { return isHemodinamicoModelListVisible; }
            set
            {
                isHemodinamicoModelListVisible = value;
                RaisePropertyChanged();
            }
        }

        private string selectedItemPicker;
        public string SelectedItemPicker
        {
            get { return selectedItemPicker; }
            set
            {
                selectedItemPicker = value;
                RaisePropertyChanged();
            }
        }

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

        private string labelSelectedItemPicker = "Anos";
        public string LabelSelectedItemPicker
        {
            get { return labelSelectedItemPicker; }
            set
            {
                labelSelectedItemPicker = value;
                RaisePropertyChanged();
            }
        }

        public ICommand ExecuteSearch
        {
            get { return new Command(ExecuteExcelSearch); }
        }

        public decimal Weight { get; set; }
        public decimal Age { get; set; }

        public HemodinamicoCalcViewModel()
		{
            OpenExcelFile();
            PickerItems = new List<string>
            {
                "Meses",
                "Dias",
                "Anos"
            };

            SelectedItemPicker = "Anos";

            CreateHemodinamicoModelList();
        }

        private void CreateHemodinamicoModelList()
        {
            HemodinamicoModelList = new ObservableCollection<HemodinamicoResult>();

            var modelResultPesoP5 = new HemodinamicoResult(HemodincamicoModelsConstants.PESO_P5);
            var modelResultPesoP50 = new HemodinamicoResult(HemodincamicoModelsConstants.PESO_P50);
            var modelResultPesoP95 = new HemodinamicoResult(HemodincamicoModelsConstants.PESO_P95);
            var modelResultFC = new HemodinamicoResult(HemodincamicoModelsConstants.FC);
            var modelResultPA = new HemodinamicoResult(HemodincamicoModelsConstants.PA);
            var modelResultFR = new HemodinamicoResult(HemodincamicoModelsConstants.FR);
            var modelResultMasc = new HemodinamicoResult(HemodincamicoModelsConstants.MASCARA);
            var modelResultMascLaringea = new HemodinamicoResult(HemodincamicoModelsConstants.MASCARA_LARINGEA);
            var modelResultLam = new HemodinamicoResult(HemodincamicoModelsConstants.LAMINA);
            var modelResultETT = new HemodinamicoResult(HemodincamicoModelsConstants.ETT);
            var espacoMorto = new HemodinamicoResult(HemodincamicoModelsConstants.ESPACO_MORTO);
            var volumeCoreente = new HemodinamicoResult(HemodincamicoModelsConstants.VOLUME_CORRENTE);
            var ventilacaoMinuto = new HemodinamicoResult(HemodincamicoModelsConstants.VENTILACAO_MINUTO);
            var fluidoDeManutencao = new HemodinamicoResult(HemodincamicoModelsConstants.FLUIDO_DE_MANUTENCAO);
            var volDiario = new HemodinamicoResult(HemodincamicoModelsConstants.VOLUME_DIARIO);
            var taxaFluido = new HemodinamicoResult(HemodincamicoModelsConstants.TAXA_FLUIDO);

            HemodinamicoModelList.Add(modelResultPesoP5);
            HemodinamicoModelList.Add(modelResultPesoP50);
            HemodinamicoModelList.Add(modelResultPesoP95);
            HemodinamicoModelList.Add(modelResultFC);
            HemodinamicoModelList.Add(modelResultPA);
            HemodinamicoModelList.Add(modelResultFR);
            HemodinamicoModelList.Add(modelResultMasc);
            HemodinamicoModelList.Add(modelResultMascLaringea);
            HemodinamicoModelList.Add(modelResultLam);
            HemodinamicoModelList.Add(modelResultETT);
            HemodinamicoModelList.Add(espacoMorto);
            HemodinamicoModelList.Add(volumeCoreente);
            HemodinamicoModelList.Add(ventilacaoMinuto);
            HemodinamicoModelList.Add(fluidoDeManutencao);
            HemodinamicoModelList.Add(volDiario);
            HemodinamicoModelList.Add(taxaFluido);
        }

        private void OpenExcelFile()
        {
            Task.Run(() =>
            {
                try
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
                catch (Exception ex)
                {

                }
            });
        }

        private void CloseExcelFile()
        {
            DataSet.Dispose();
        }

        private void ExecuteExcelSearch()
        {
            if (DataSet != null)
            {
                IsHemodinamicoModelListVisible = true;

                var tableRows = DataSet
                    .Tables[0]
                    .Rows.Cast<DataRow>()
                    .ToList();

                DataRow item = null;

                if (LabelSelectedItemPicker == "Anos" &&
                    Age >= 20)
                { 
                    item = tableRows.LastOrDefault();
                }
                else if (LabelSelectedItemPicker == "Anos")
                {
                    int index = GetTableIndex(365);
                    item = tableRows[index];
                }
                else if (LabelSelectedItemPicker == "Meses")
                {
                    int index = GetTableIndex(30);
                    item = tableRows[index];
                }
                else
                {
                    int index = GetTableIndex(1);
                    item = tableRows[index];
                }

                HemodinamicoModelListValues(item);
            }
        }

        private int GetTableIndex(int value)
        {
            int index = (int)Math.Round((Age * value), 0);

            if (index < 0)
                index = 0;

            return index;
        }

        private void HemodinamicoModelListValues(DataRow item)
        {
            string[] arr = new string[]
            {
                HemodincamicoModelsConstants.PESO_P5,
                HemodincamicoModelsConstants.PESO_P50,
                HemodincamicoModelsConstants.PESO_P95,
                HemodincamicoModelsConstants.FC,
                HemodincamicoModelsConstants.PA,
                HemodincamicoModelsConstants.FR,
                HemodincamicoModelsConstants.MASCARA,
                HemodincamicoModelsConstants.LAMINA,
                HemodincamicoModelsConstants.ETT,
                HemodincamicoModelsConstants.ESPACO_MORTO,
                HemodincamicoModelsConstants.VOLUME_CORRENTE,
                HemodincamicoModelsConstants.VENTILACAO_MINUTO,
                HemodincamicoModelsConstants.FLUIDO_DE_MANUTENCAO
            };

            for (int i = 0; i < arr.Length; i++)
            {
                var model = HemodinamicoModelList
                    .FirstOrDefault(x => x.Text == arr[i]);

                if (model is null)
                    break;

                int count = i + 3;

                if (model.Text.Contains("Peso"))
                {
                    string value = item.ItemArray[count].ToString();
                    model.Value = value + " kg";
                    continue;
                }

                if (model.Text == HemodincamicoModelsConstants.ESPACO_MORTO)
                {
                    model.Value = GetValueByPeso(2.2m, "ml");
                    continue;
                }

                if (model.Text == HemodincamicoModelsConstants.VOLUME_CORRENTE)
                {
                    string firstValue = GetValueByPeso(6, "");
                    string secondValue = GetValueByPeso(8, "ml");
                    model.Value = $"{firstValue} - {secondValue}";
                    continue;
                }

                if (model.Text == HemodincamicoModelsConstants.VENTILACAO_MINUTO)
                {
                    string firstValue = GetValueByPeso(72, "");
                    string secondValue = GetValueByPeso(160, "L/min");
                    model.Value = $"{firstValue} - {secondValue}";
                    continue;
                }

                if (model.Text == HemodincamicoModelsConstants.FLUIDO_DE_MANUTENCAO)
                {
                    model.Value = GetValueByPeso(4, "");
                    continue;
                }

                if (model.Text == HemodincamicoModelsConstants.MASCARA)
                {
                    model.Value = item.ItemArray[count].ToString();
                    continue;
                }

                model.Value = item.ItemArray[count].ToString();
            }

            HemodinamicoModelList
                .FirstOrDefault(x => x.Text == HemodincamicoModelsConstants.MASCARA_LARINGEA)
                .Value = GetNumeroMascaraLaringea();

            HemodinamicoModelList
                .FirstOrDefault(x => x.Text == HemodincamicoModelsConstants.VOLUME_DIARIO)
                .Value = GetResultVolumeDiario();
        }

        private string GetResultTaxaFluido(decimal volDiario)
        {
            string result = result = (volDiario / 24).ToString();
            return result;
        }

        private string GetResultVolumeDiario()
        {
            decimal result = 0;

            if (Weight < 10)
                result = Math.Round(100 * Weight);

            if (Weight >= 10 && Weight < 20)
                result = 1000 + (50 * (Weight - 10));

            if (Weight >= 20)
                result = 1500 + (20 * (Weight - 20));

            if (result > 2400)
                result = 2400;

            result = Math.Round(result);

            HemodinamicoModelList
               .FirstOrDefault(x => x.Text == HemodincamicoModelsConstants.TAXA_FLUIDO)
               .Value = Math.Round(result / 24).ToString();

            return result.ToString();
        }

        private string GetNumeroMascaraLaringea()
        {
            if (Weight < 5)
                return "nº 1";

            if (Weight >= 5 && Weight < 10)
                return "nº 1.5";

            if (Weight >= 10 && Weight < 20)
                return "nº 2";

            if (Weight >= 20 && Weight < 30)
                return "nº 2.5";

            if (Weight >= 30 && Weight < 50)
                return "nº 3";

            if (Weight >= 50 && Weight < 70)
                return "nº 4";

            if (Weight >= 70 && Weight < 100)
                return "nº 5";

            return "nº 0";
        }

        private string GetValueByPeso(decimal number, string medida)
        {
            return $"{Math.Round(number * Weight, 2).ToString()} {medida}";
        }

        protected override void ViewModelBase_PropertyChanged(object sender,
            PropertyChangedEventArgs e)
        {
            base.ViewModelBase_PropertyChanged(sender, e);

            switch (e.PropertyName)
            {
                case nameof(SelectedItemPicker):
                    SetFilteredSelection();
                    break;
                case nameof(EntryWeight):
                    Weight = decimal.Parse(EntryWeight ?? "0");
                    ExecuteExcelSearch();
                    break;
                case nameof(EntryAge):
                    Age = decimal.Parse(EntryAge ?? "0");
                    ExecuteExcelSearch();
                    break;
                case nameof(IsHemodinamicoModelListVisible):
                    IsHemodinamicoLabelVisible = !IsHemodinamicoModelListVisible;
                    break;
            }
        }

        private void SetFilteredSelection()
        {
            LabelSelectedItemPicker = SelectedItemPicker;
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            if (HasChangedValues)
                ExecuteExcelSearch();
        }
    }
}

