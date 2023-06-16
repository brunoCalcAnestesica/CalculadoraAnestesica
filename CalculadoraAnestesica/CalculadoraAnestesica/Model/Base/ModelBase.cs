using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite;

namespace CalculadoraAnestesica.Model.Base
{
    public class ModelBase : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public string Id { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

