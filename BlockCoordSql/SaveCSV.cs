using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using static System.Net.Mime.MediaTypeNames;

namespace ACADCommands
{
    public class SaveCSV
    {
        public async void saveCSV(string text)
        {
            try
            {

            string path = "note1.csv";
            //string text = "Hello World\nHello METANIT.COM";

            // полная перезапись файла 
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                await writer.WriteAsync(text);
            }
                // добавление в файл
                //using (StreamWriter writer = new StreamWriter(path, true))
                //{
                //    await writer.WriteLineAsync("Addition");
                //    await writer.WriteAsync("4,5");
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

