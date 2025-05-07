using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Interactivity;

namespace JumpButtons
{
    public class DataGridNavigationBehavior : Behavior<DataGrid>
    {
        public static readonly DependencyProperty ToolbarProperty =
            DependencyProperty.Register(
                "Toolbar",
                typeof(DataGridToolbar),
                typeof(DataGridNavigationBehavior),
                new PropertyMetadata(null, OnToolbarChanged));

        public DataGridToolbar Toolbar
        {
            get { return (DataGridToolbar)GetValue(ToolbarProperty); }
            set { SetValue(ToolbarProperty, value); }
        }

        public static readonly DependencyProperty EnableKeyboardNavigationProperty =
            DependencyProperty.Register(
                "EnableKeyboardNavigation",
                typeof(bool),
                typeof(DataGridNavigationBehavior),
                new PropertyMetadata(true));

        public bool EnableKeyboardNavigation
        {
            get { return (bool)GetValue(EnableKeyboardNavigationProperty); }
            set { SetValue(EnableKeyboardNavigationProperty, value); }
        }

        private static void OnToolbarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGridNavigationBehavior behavior && behavior.AssociatedObject != null)
            {
                behavior.ConnectToolbarToDataGrid();
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            ConnectToolbarToDataGrid();

            if (EnableKeyboardNavigation)
            {
                AssociatedObject.PreviewKeyDown += AssociatedObject_PreviewKeyDown;
            }
        }

        protected override void OnDetaching()
        {
            if (EnableKeyboardNavigation)
            {
                AssociatedObject.PreviewKeyDown -= AssociatedObject_PreviewKeyDown;
            }

            if (Toolbar != null)
            {
                DataGridToolbar.SetTargetDataGrid(Toolbar, null);
            }

            base.OnDetaching();
        }

        private void ConnectToolbarToDataGrid()
        {
            if (Toolbar != null && AssociatedObject != null)
            {
                DataGridToolbar.SetTargetDataGrid(Toolbar, AssociatedObject);
            }
        }

        private void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (AssociatedObject == null || AssociatedObject.Items.Count == 0)
                return;

            int currentIndex = GetCurrentRowIndex();
            if (currentIndex < 0)
                return;

            bool handled = false;

            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                switch (e.Key)
                {
                    case Key.Home:
                        NavigateToRowIndex(0);
                        handled = true;
                        break;
                    case Key.End:
                        NavigateToRowIndex(AssociatedObject.Items.Count - 1);
                        handled = true;
                        break;
                }
            }

            if (handled)
            {
                e.Handled = true;
            }
        }

        private int GetCurrentRowIndex()
        {
            if (AssociatedObject.SelectedItem == null)
                return -1;

            return AssociatedObject.Items.IndexOf(AssociatedObject.SelectedItem);
        }


        private void NavigateToRowIndex(int index)
        {
            if (AssociatedObject == null || AssociatedObject.Items.Count == 0)
                return;

            if (index < 0)
                index = 0;
            if (index >= AssociatedObject.Items.Count)
                index = AssociatedObject.Items.Count - 1;

            AssociatedObject.SelectedItem = AssociatedObject.Items[index];
            AssociatedObject.ScrollIntoView(AssociatedObject.SelectedItem);
            AssociatedObject.Focus();
        }
    }
}
