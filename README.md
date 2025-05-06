# Rowjump_wpf_datagrid

Usage: 
```
 <JumpButtons:DataGridToolbar x:Name="toolbar" ShowJumpButtons="True" />

 <DataGrid x:Name="UsersDataGrid" 
           Grid.Row="1"
           AutoGenerateColumns="False"
           ItemsSource="{Binding Items}"
           CanUserAddRows="True">

     <i:Interaction.Behaviors>
         <JumpButtons:DataGridNavigationBehavior Toolbar="{Binding ElementName=toolbar}"
                                          JumpCount="5"
                                          EnableKeyboardNavigation="True" />
     </i:Interaction.Behaviors>
     <DataGrid.Columns>
         <DataGridTextColumn Header="ID" Binding="{Binding Id}" IsReadOnly="True" />
         <DataGridTextColumn Header="Name" Binding="{Binding Name}" />
         <DataGridTextColumn Header="Age" Binding="{Binding Age}" />
     </DataGrid.Columns>
 </DataGrid>
```
