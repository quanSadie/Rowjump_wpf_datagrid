using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace JumpButtons
{
    public class DataGridToolbar : UserControl
    {
        static DataGridToolbar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridToolbar),
                new FrameworkPropertyMetadata(typeof(DataGridToolbar)));
        }

        public static readonly DependencyProperty TargetDataGridProperty =
            DependencyProperty.RegisterAttached(
                "TargetDataGrid",
                typeof(DataGrid),
                typeof(DataGridToolbar),
                new PropertyMetadata(null, OnTargetDataGridChanged));

        public static DataGrid GetTargetDataGrid(DependencyObject obj)
        {
            return (DataGrid)obj.GetValue(TargetDataGridProperty);
        }

        public static void SetTargetDataGrid(DependencyObject obj, DataGrid value)
        {
            obj.SetValue(TargetDataGridProperty, value);
        }

        private DataGrid targetDataGrid;
        public DataGrid TargetDataGrid
        {
            get { return targetDataGrid; }
            set
            {
                if (targetDataGrid != null)
                {
                    targetDataGrid.SelectionChanged -= TargetDataGrid_SelectionChanged;
                    targetDataGrid.Loaded -= TargetDataGrid_Loaded;
                }

                targetDataGrid = value;

                if (targetDataGrid != null)
                {
                    targetDataGrid.SelectionChanged += TargetDataGrid_SelectionChanged;
                    targetDataGrid.Loaded += TargetDataGrid_Loaded;

                    if (targetDataGrid.IsLoaded)
                    {
                        UpdateRowDisplay();
                    }
                }
            }
        }

        public DataGridToolbar()
        {
            // Template is applied via Generic.xaml
        }

        private static void OnTargetDataGridChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGridToolbar toolbar)
            {
                toolbar.TargetDataGrid = e.NewValue as DataGrid;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Attach event handlers to template parts
            AttachButtonEvents();
            AttachTextBoxEvents();
        }

        private void AttachButtonEvents()
        {
            if (GetTemplateChild("PART_FirstButton") is Button firstButton)
            {
                firstButton.Click += FirstButton_Click;
            }

            if (GetTemplateChild("PART_PreviousButton") is Button previousButton)
            {
                previousButton.Click += PreviousButton_Click;
            }

            if (GetTemplateChild("PART_NextButton") is Button nextButton)
            {
                nextButton.Click += NextButton_Click;
            }

            if (GetTemplateChild("PART_LastButton") is Button lastButton)
            {
                lastButton.Click += LastButton_Click;
            }
        }

        private void AttachTextBoxEvents()
        {
            if (GetTemplateChild("PART_CurrentRowTextBox") is TextBox currentRowTextBox)
            {
                currentRowTextBox.KeyDown += CurrentRowTextBox_KeyDown;
            }

            if (GetTemplateChild("PART_TotalRowsTextBlock") is TextBlock totalRowsTextBlock)
            {
                UpdateRowDisplay();
            }
        }

        private void TargetDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateRowDisplay();
        }

        private void TargetDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateRowDisplay();
        }

        private void UpdateRowDisplay()
        {
            if (targetDataGrid == null || !targetDataGrid.IsLoaded)
                return;

            int totalItems = GetTotalItems();
            if (GetTemplateChild("PART_TotalRowsTextBlock") is TextBlock totalRowsTextBlock)
            {
                totalRowsTextBlock.Text = totalItems.ToString();
            }

            int currentIndex = GetCurrentRowIndex();
            if (GetTemplateChild("PART_CurrentRowTextBox") is TextBox currentRowTextBox)
            {
                currentRowTextBox.Text = (currentIndex + 1).ToString();
            }

            bool hasItems = totalItems > 0;
            if (GetTemplateChild("PART_FirstButton") is Button firstButton)
            {
                firstButton.IsEnabled = hasItems && currentIndex > 0;
            }
            if (GetTemplateChild("PART_PreviousButton") is Button previousButton)
            {
                previousButton.IsEnabled = hasItems && currentIndex > 0;
            }
            if (GetTemplateChild("PART_NextButton") is Button nextButton)
            {
                nextButton.IsEnabled = hasItems && currentIndex < totalItems - 1;
            }
            if (GetTemplateChild("PART_LastButton") is Button lastButton)
            {
                lastButton.IsEnabled = hasItems && currentIndex < totalItems - 1;
            }
        }

        private int GetTotalItems()
        {
            if (targetDataGrid?.Items == null)
                return 0;

            return targetDataGrid.Items.Count;
        }

        private int GetCurrentRowIndex()
        {
            if (targetDataGrid == null || targetDataGrid.SelectedItem == null)
                return -1;

            return targetDataGrid.Items.IndexOf(targetDataGrid.SelectedItem);
        }

        private void NavigateToRowIndex(int index)
        {
            if (targetDataGrid == null || targetDataGrid.Items.Count == 0)
                return;

            int totalItems = GetTotalItems();
            if (index < 0)
                index = 0;
            if (index >= totalItems)
                index = totalItems - 1;

            targetDataGrid.SelectedItem = targetDataGrid.Items[index];
            targetDataGrid.ScrollIntoView(targetDataGrid.SelectedItem);
            targetDataGrid.Focus();

            UpdateRowDisplay();
        }

        private void FirstButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToRowIndex(0);
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            int currentIndex = GetCurrentRowIndex();
            if (currentIndex > 0)
            {
                NavigateToRowIndex(currentIndex - 1);
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            int currentIndex = GetCurrentRowIndex();
            int totalItems = GetTotalItems();

            if (currentIndex < totalItems - 1)
            {
                NavigateToRowIndex(currentIndex + 1);
            }
        }

        private void LastButton_Click(object sender, RoutedEventArgs e)
        {
            int totalItems = GetTotalItems();
            if (totalItems > 0)
            {
                NavigateToRowIndex(totalItems - 1);
            }
        }

        private void CurrentRowTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (sender is TextBox textBox && int.TryParse(textBox.Text, out int rowIndex) && rowIndex > 0)
                {
                    NavigateToRowIndex(rowIndex - 1);
                }
                else
                {
                    UpdateRowDisplay();
                }
            }
        }
    }
}
