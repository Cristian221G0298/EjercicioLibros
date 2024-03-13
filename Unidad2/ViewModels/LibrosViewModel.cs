using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Unidad2.Models;

namespace Unidad2.ViewModels
{
    public enum Ventanas { Agregar,Eliminar,Lista}
    public class LibrosViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Libro> Libros { get; set; } = new();
        //Accione de usuario
        public string Error { get; set; } = "";
        public ICommand AgregarCommand { get; set; }
        public ICommand VerAgregarCommand { get; set; }
        public ICommand VerEliminarCommand {  get; set; }
        public ICommand VerEditarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public ICommand GuardarCommand { get; set; }
        //Referenecia Modelo
        public Libro? Libro { get; set; }
        //decide que ventana mostrar
        public Ventanas Ventana { get; set; } = Ventanas.Lista;
        public LibrosViewModel() 
        {
            CancelarCommand= new RelayCommand(Cancelar);
            AgregarCommand = new RelayCommand(Agregar);
            GuardarCommand = new RelayCommand(Guardar);
            VerAgregarCommand = new RelayCommand(VerAgregar);
            VerEliminarCommand = new RelayCommand(VerEliminar);
            VerEditarCommand = new RelayCommand(VerEditar);
            EliminarCommand = new RelayCommand(Eliminar);

            Deserealizar();
        }

        private void Eliminar()
        {
            if(Libro != null)
            {
                Libros.Remove(Libro);
                Serializar();
                Ventana = Ventanas.Lista;
                Actualizar(nameof(Ventana));
            }
        }

        private void VerEditar()
        {
            throw new NotImplementedException();
        }

        private void VerEliminar()
        {
            Ventana = Ventanas.Eliminar;
            Actualizar(nameof(Ventana));
        }

        private void VerAgregar()
        {
            Error = "";
            Libro = new();
            Ventana = Ventanas.Agregar;
            Actualizar(nameof(Ventana));
        }

        private void Guardar()
        {
            throw new NotImplementedException();
        }

        private void Agregar()
        {
            if(Libro != null)
            {
                Error = "";
                if (string.IsNullOrWhiteSpace(Libro.Titulo))
                {
                    Error = "Escriba el título del libro";
                }
                if (string.IsNullOrWhiteSpace(Libro.Autor))
                {
                    Error = "Escriba el nombre del autor";
                }
                if (string.IsNullOrWhiteSpace(Libro.Portada) || !Libro.Portada.StartsWith("http") || !Libro.Portada.EndsWith("jpg"))
                {
                    Error = "Escriba la dirección de una imagen en JPG";
                }
                if(!string.IsNullOrWhiteSpace(Error))
                {
                    Actualizar(nameof(Error));
                    return;
                }
                Libros.Add(Libro);
                Serializar();
                Ventana = Ventanas.Lista;
                Actualizar(nameof(Ventana));
            }
        }

        private void Cancelar()
        {
            Ventana = Ventanas.Lista;
            Actualizar(nameof(Ventana));
        }
        void Serializar()
        {
            File.WriteAllText("Libros.json", JsonSerializer.Serialize(Libros));
        }
        void Deserealizar()
        {
            try
            {
                var datos = JsonSerializer.Deserialize<ObservableCollection<Libro>>(File.ReadAllText("Libros.json"));
                if(datos != null)
                {
                    foreach(var libro in datos)
                    {
                        Libros.Add(libro);
                    }
                }
            }
            catch
            {

            }
        }
        private void Actualizar(string? name)
        {
            PropertyChanged?.Invoke(this, new(name));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
