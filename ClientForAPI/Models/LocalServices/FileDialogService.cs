using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientForAPI.Models.LocalServices
{
    internal class FileDialogService
    {
        private readonly Window _owner;

        public FileDialogService(Window owner)
        {
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
                return null;
            }
            return result;

        }
    }
}

