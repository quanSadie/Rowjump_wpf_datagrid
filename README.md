# Row jump common library for wpf data grid

Usage: 
```
 <JumpButtons:DataGridToolbar x:Name="toolbar" />

 <DataGrid x:Name="UsersDataGrid" 
           Grid.Row="1"
           AutoGenerateColumns="False"
           ItemsSource="{Binding Items}"
           CanUserAddRows="True">

     <i:Interaction.Behaviors>
         <JumpButtons:DataGridNavigationBehavior Toolbar="{Binding ElementName=toolbar}"
                                          EnableKeyboardNavigation="True" />
     </i:Interaction.Behaviors>
     <DataGrid.Columns>
         <DataGridTextColumn Header="ID" Binding="{Binding Id}" IsReadOnly="True" />
         <DataGridTextColumn Header="Name" Binding="{Binding Name}" />
         <DataGridTextColumn Header="Age" Binding="{Binding Age}" />
     </DataGrid.Columns>
 </DataGrid>
```
