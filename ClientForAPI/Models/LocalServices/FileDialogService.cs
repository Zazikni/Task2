using Avalonia.Controls;
using Serilog;
using System;
using System.Threading.Tasks;

namespace ClientForAPI.Models.LocalServices
{
    public class EmptySelectionError : Exception
    {
        public EmptySelectionError(string message) : base(message)
        {
        }
    }
    internal class FileDialogService
    {
        private readonly Window _owner;

        public FileDialogService(Window owner)
        {
            Log.Information($"Инициализации сервиса выбора файлов.");

            _owner = owner;
        }
        public async Task<string[]> ShowOpenFileDialogAsync(FileDialogFilter filter, bool allowMultiple = false)
        {
            var dialog = new OpenFileDialog
            {
                AllowMultiple = allowMultiple,
            };

            if (filter != null)
            {
                dialog.Filters.Add(filter);
            }

            var result = await dialog.ShowAsync(_owner);
            if (result == null)
            {
                Log.Information($"Пользователь ничего не выбрал.");

                return null;
            }
            return result;

        }
    }
}

