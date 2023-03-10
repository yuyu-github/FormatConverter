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
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;

using FormatConverter.FileTypes;
using Type = FormatConverter.FileTypes;

namespace FormatConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string currentSelectedFileExt = "";

        public MainWindow()
        {
            InitializeComponent();
            
            InitFileTypeList();
        }

        void InitFileTypeList()
        {
            FileTypeList.List = new FileType[] {
                new Text(),
                new Markdown(),
                new Html(),
                new Csv(),
                new Json(),
                new Yaml(),
                new Xml(),
                new Toml(),
                new Jpeg(),
                new Png(),
                new Gif(),
                new WebP(),
                new Svg(),
                new Tiff(),
                new Bmp(),
            };
        }

        private void Window_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
            e.Handled = true;
        }

        private void Window_PreviewDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);
                InputFilePathTextBox.Text = paths[0];
            }
        }

        private void SelectInputFileButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog() { Title = "ファイルを選択" };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                InputFilePathTextBox.Text = dialog.FileName;
            }
        }

        private void InputFilePathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string ext = Path.GetExtension(InputFilePathTextBox.Text);
            if (currentSelectedFileExt != ext)
            {
                currentSelectedFileExt = ext;

                InputTypeComboBox.Items.Clear();
                if (InputFilePathTextBox.Text != "")
                {
                    if (ext != "")
                    {
                        (FileTypeList.ExtensionDictionary.TryGetValue(ext[1..], out var value) ? value : new()).ForEach(i =>
                            InputTypeComboBox.Items.Add(new InputTypeComboBoxItem() { Text = $"{i.Name} ({ext})", Id = i.Id }));

                    }
                    else
                    {
                        foreach (var item in FileTypeList.List)
                        {
                            InputTypeComboBox.Items.Add(new InputTypeComboBoxItem()
                            { Text = $"{item.Name} ({(item.Extensions.Length > 0 ? "." + item.Extensions[0] : "")})", Id = item.Id });
                        }
                    }

                    if (InputTypeComboBox.Items.Count > 0) InputTypeComboBox.SelectedIndex = 0;
                }
            }
        }

        private void InputTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OutputTypeListView.Items.Clear();
            if (InputTypeComboBox.SelectedItem is not null)
            {
                foreach (var type in FileTypeList.IdDictionary.TryGetValue(((InputTypeComboBoxItem)InputTypeComboBox.SelectedItem).Id, out var inputType) ?
                    inputType.GetConvertibleTypes() : new string[1])
                {
                    if (FileTypeList.IdDictionary.TryGetValue(type, out var outputType))
                        OutputTypeListView.Items.Add(new OutputTypeListViewItem()
                        { Text = $"{outputType.Name} ({(outputType.Extensions.Length > 0 ? "." + outputType.Extensions[0] : "")})", Id = outputType.Id });
                }

                if (OutputTypeListView.Items.Count > 0) OutputTypeListView.SelectedIndex = 0;
            }
        }

        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            if (InputTypeComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("変換元タイプが設定されていません", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (OutputTypeListView.SelectedIndex == -1)
            {
                MessageBox.Show("変換先タイプが設定されていません", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!File.Exists(InputFilePathTextBox.Text))
            {
                MessageBox.Show("変換元ファイルが存在しません", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!FileTypeList.IdDictionary.TryGetValue(((InputTypeComboBoxItem)InputTypeComboBox.SelectedItem).Id, out var inputType))
            {
                MessageBox.Show("変換元タイプがが存在しません", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!FileTypeList.IdDictionary.TryGetValue(((OutputTypeListViewItem)OutputTypeListView.SelectedItem).Id, out var outputType))
            {
                MessageBox.Show("変換先タイプがが存在しません", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var dialog = new CommonSaveFileDialog()
            {
                Title = "ファイルを保存",
                DefaultFileName = Path.GetFileNameWithoutExtension(InputFilePathTextBox.Text),
                InitialDirectory = Path.GetDirectoryName(InputFilePathTextBox.Text)
            };
            if (outputType.Extensions.Length > 0)
            {
                dialog.DefaultFileName += "." + outputType.Extensions[0];
                dialog.Filters.Add(new CommonFileDialogFilter($"{outputType.Name}", outputType.Extensions[0]));
            }
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                try
                {
                    inputType.Convert(outputType.Id, InputFilePathTextBox.Text, dialog.FileName);
                }
                catch (ConversionException ex)
                {
                    MessageBox.Show("変換に失敗しました\n\n" + ex.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                catch (Exception ex)
                {
                    string stackTrace = "";
#if DEBUG
                    stackTrace = "\n" + ex.StackTrace;
#endif
                    MessageBox.Show($"エラーが発生しました\n\n{ex.Message}{stackTrace}", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        class InputTypeComboBoxItem
        {
            public string Text { get; set; } = "";
            public string Id { get; set; } = "";

            public override string ToString() => Text;
        }

        class OutputTypeListViewItem
        {
            public string Text { get; set; } = "";
            public string Id { get; set; } = "";

            public override string ToString() => Text;
        }
    }
}
